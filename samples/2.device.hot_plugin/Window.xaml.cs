using System;
using System.Windows;
using System.Windows.Threading;

namespace Orbbec
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class HotPluginWindow : Window
    {
        private DeviceList curDeviceList;

        public HotPluginWindow()
        {
            InitializeComponent();

            try
            {
                Context context = new Context();
                context.SetDeviceChangedCallback((removedList, addedList) =>
                {
                    PrintDeviceList("added", addedList);
                    PrintDeviceList("removed", removedList);

                    curDeviceList = context.QueryDeviceList();
                });
                curDeviceList = context.QueryDeviceList();
                PrintDeviceList("connected", curDeviceList);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
            }
        }

        private void PrintDeviceList(string prompt, DeviceList deviceList)
        {
            if (deviceList != null)
            {
                uint count = deviceList.DeviceCount();
                if (count == 0) return;

                Dispatcher.Invoke(() =>
                {
                    DeviceInfoTextBlock.Text = $"{count} device(s) {prompt}";
                    DeviceListBox.Items.Clear();

                    for (uint i = 0; i < count; i++)
                    {
                        var info = $"- UID: {deviceList.Uid(i)}\n" +
                                   $"  VID: 0x{deviceList.Vid(i):X4}\n" +
                                   $"  PID: 0x{deviceList.Pid(i):X4}\n" +
                                   $"  Serial Number: {deviceList.SerialNumber(i)}\n" +
                                   $"  Connection: {deviceList.ConnectionType(i)}";
                        DeviceListBox.Items.Add(info);
                    }

                    if (prompt.Equals("added"))
                    {
                        //Dispatcher.Invoke(() => DeviceInfoTextBlock.Text = "Devices rebooted successfully.");
                        RebootDevicesButton.IsEnabled = true;
                    }
                });
            }
        }

        private void RebootDevicesButton_Click(object sender, RoutedEventArgs e)
        {
            if (curDeviceList == null) return;
            RebootDevicesButton.IsEnabled = false;
            Dispatcher.Invoke(() => DeviceInfoTextBlock.Text = "Rebooting devices...");
            for (uint i = 0; i < curDeviceList.DeviceCount(); i++)
            {
                curDeviceList.GetDevice(i).Reboot();
            }
        }

        private void Control_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (curDeviceList != null)
            {
                curDeviceList.Dispose();
            }
        }
    }
}