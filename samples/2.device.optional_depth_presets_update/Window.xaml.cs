using System;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Text;
using System.Linq;
using System.ComponentModel;

namespace Orbbec
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class OptionalDepthPresetsUpdateWindow : Window
    {
        private Context context;
        private Dictionary<string, Device> devices;
        private Device curDevice;

        private string[] curFilePaths;

        public OptionalDepthPresetsUpdateWindow()
        {
            InitializeComponent();

            devices = new Dictionary<string, Device>();

            try
            {
                context = new Context();
                context.SetDeviceChangedCallback((removedList, addedList) =>
                {
                    for (uint i = 0; i < removedList.DeviceCount(); i++)
                    {
                        string rmSN = removedList.SerialNumber(i);
                        if (devices.ContainsKey(rmSN))
                        {
                            devices.Remove(rmSN);
                        }
                    }

                    for (uint i = 0; i < addedList.DeviceCount(); i++)
                    {
                        string serialNumber = addedList.SerialNumber(0);
                        if (!devices.ContainsKey(serialNumber))
                        {
                            devices.Add(serialNumber, addedList.GetDevice(i));
                        }
                    }

                    Dispatcher.Invoke(() =>
                    {
                        UpdateDeviceSelector();
                    });
                });
                DeviceList deviceList = context.QueryDeviceList();
                if (deviceList.DeviceCount() > 0)
                {
                    for (uint i = 0; i < deviceList.DeviceCount(); i++)
                    {
                        var device = deviceList.GetDevice(i);
                        string sn = deviceList.SerialNumber(i);
                        if (!devices.ContainsKey(sn))
                        {
                            devices.Add(sn, device);
                        }
                    }

                    deviceList.Dispose();
                    OnDeviceInit();
                }
                else
                {
                    deviceList.Dispose();
                }

                deviceSelector.SelectionChanged += (s, e) => OnDeviceSelect(deviceSelector.SelectedIndex);
                filePickerButton.Click += (s, e) => OnFilePickerSelect();
                filePathTextBox.TextChanged += (s, e) => OnFilePathChange();
                presetUpdateButton.Click += (s, e) => PresetUpdate();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
            }
        }

        private void OnDeviceInit()
        {
            if (devices.Count > 0)
            {
                OnDeviceSelect(0);
                UpdateDeviceSelector();
            }
        }

        private void UpdateDeviceSelector()
        {
            deviceSelector.Items.Clear();
            foreach (var device in devices.Values)
            {
                deviceSelector.Items.Add(device.GetDeviceInfo().Name());
            }
            deviceSelector.SelectedIndex = devices.Count > 0 ? 0 : -1;
        }

        private void OnDeviceSelect(int index)
        {
            if (index >= 0 && index < devices.Count)
            {
                curDevice = devices.Values.ElementAt(index);
                if (!IsPresetSupported(curDevice))
                {
                    MessageBox.Show("The device you selected does not support preset. Please select another one");
                    deviceInfoTextBlock.Text = "";
                    presetInfoTextBlock.Text = "";
                    return;
                }
                UpdateInfo();
            }
            else
            {
                deviceInfoTextBlock.Text = "";
                presetInfoTextBlock.Text = "";
            }
        }

        private bool IsPresetSupported(Device device)
        {
            PresetList presetList = device.GetAvailablePresetList();
            return presetList != null && presetList.Count() > 0;
        }

        private void OnFilePickerSelect()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Binary files (*.bin)|*.bin",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                curFilePaths = openFileDialog.FileNames;
                filePathTextBox.Text = string.Join("; ", curFilePaths);
            }
        }

        private void OnFilePathChange()
        {
            Console.WriteLine(string.Join("; ", curFilePaths));
        }

        private async void PresetUpdate()
        {
            foreach (var file in curFilePaths)
            {
                if (!file.EndsWith(".bin"))
                {
                    Console.WriteLine("invalid presets file: " + file);
                    return;
                }
            }

            presetUpdateButton.IsEnabled = false;
            try
            {
                var upgradeTask = new TaskCompletionSource<bool>();
                bool firstCall = true;

                curDevice.UpdateOptionalDepthPresets(curFilePaths, curFilePaths.Length, (state, msg, percent) =>
                {
                    PresetUpdateCallback(firstCall, state, msg, percent);
                    firstCall = false;

                    if (state == UpgradeState.STAT_DONE || state == UpgradeState.STAT_DONE_WITH_DUPLICATES)
                    {
                        upgradeTask.TrySetResult(true);
                    }
                    else if (state == UpgradeState.ERR_VERIFY || state == UpgradeState.ERR_PROGRAM || state == UpgradeState.ERR_ERASE ||
                            state == UpgradeState.ERR_FLASH_TYPE || state == UpgradeState.ERR_IMAGE_SIZE || state == UpgradeState.ERR_OTHER ||
                            state == UpgradeState.ERR_DDR || state == UpgradeState.ERR_TIMEOUT)
                    {
                        upgradeTask.TrySetResult(false);
                    }
                });

                bool success = await upgradeTask.Task;

                if (success)
                {
                    presetUpdateButton.IsEnabled = true;
                    curDevice.Reboot();
                    Console.WriteLine("Update successful!");
                }
                else
                {
                    presetUpdateButton.IsEnabled = true;
                    Console.WriteLine("Update failed!");
                }
            }
            catch (Exception e)
            {
                presetUpdateButton.IsEnabled = true;
                Console.WriteLine("The update was interrupted! An error occurred!");
                Console.WriteLine(e);
            }
        }

        private void UpdateInfo()
        {
            UpdateDeviceInfo();
            UpdatePresetInfo();
        }

        private void UpdateDeviceInfo()
        {
            DeviceInfo deviceInfo = curDevice.GetDeviceInfo();
            if (deviceInfo != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Device name: {deviceInfo.Name()}");
                sb.AppendLine($"Device pid: {deviceInfo.Pid()}");
                sb.AppendLine($"Firmware version: {deviceInfo.FirmwareVersion()}");
                sb.AppendLine($"Serial number: {deviceInfo.SerialNumber()}");
                sb.Append("--------------------------------------");

                deviceInfoTextBlock.Text = sb.ToString();
            }
        }

        private void UpdatePresetInfo()
        {
            var presetList = curDevice.GetAvailablePresetList();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Preset count: {presetList.Count()}");
            for (uint i = 0; i < presetList.Count(); i++)
            {
                sb.AppendLine($" - {presetList.GetName(i)}");
            }
            sb.AppendLine($"Current preset: {curDevice.GetCurrentPresetName()}");

            string key = "PresetVer";
            if (curDevice.IsExtensionInfoExist(key))
            {
                sb.AppendLine($"Preset version: {curDevice.GetExtensionInfo(key)}");
            }

            presetInfoTextBlock.Text = sb.ToString();
        }

        private void PresetUpdateCallback(bool firstCall, UpgradeState state, string message, byte percent)
        {
            if (!firstCall)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 3); // Move cursor up 3 lines
            }

            Console.Clear();
            Console.WriteLine($"Progress: {percent}%");

            Console.Write("Status  : ");
            switch (state)
            {
                case UpgradeState.STAT_VERIFY_SUCCESS:
                    Console.WriteLine("Image file verification success");
                    break;
                case UpgradeState.STAT_FILE_TRANSFER:
                    Console.WriteLine("File transfer in progress");
                    break;
                case UpgradeState.STAT_DONE:
                    Console.WriteLine("Update completed");
                    break;
                case UpgradeState.STAT_DONE_WITH_DUPLICATES:
                    Console.WriteLine("Update completed, duplicated presets have been ignored");
                    break;
                case UpgradeState.STAT_IN_PROGRESS:
                    Console.WriteLine("Update in progress");
                    break;
                case UpgradeState.STAT_START:
                    Console.WriteLine("Starting the update");
                    break;
                case UpgradeState.STAT_VERIFY_IMAGE:
                    Console.WriteLine("Verifying image file");
                    break;
                default:
                    Console.WriteLine("Unknown status or error");
                    break;
            }

            Console.WriteLine($"Message : {message}");
        }

        private void Control_Closing(object sender, CancelEventArgs e)
        {
            foreach (var device in devices.Values)
            {
                device.Dispose();
            }
            devices.Clear();
        }
    }
}