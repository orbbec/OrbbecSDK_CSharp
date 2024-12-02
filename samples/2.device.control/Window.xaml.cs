using System;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;

namespace Orbbec
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class ControlWindow : Window
    {
        private Context context;
        private Dictionary<string, Device> devices;
        private List<PropertyItem> propertyItems;

        private Device curDevice;
        private PropertyItem curPropertyItem;

        private StringBuilder stringBuilder;

        public ControlWindow()
        {
            InitializeComponent();

            devices = new Dictionary<string, Device>();
            propertyItems = new List<PropertyItem>();
            stringBuilder = new StringBuilder();

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
                        if (devices.Count > 0)
                        {
                            curDevice = devices.Values.ElementAt(0);
                        }
                        UpdateDeviceSelector();
                    });
                });
                Loaded += async (s, e) => await WaitForDeviceAsync();

                deviceSelector.SelectionChanged += (s, e) => OnDeviceSelect(deviceSelector.SelectedIndex);
                propertySelector.SelectionChanged += (s, e) => OnPropertySelect(propertySelector.SelectedIndex);
                getButton.Click += (s, e) => OnGetProperty();
                setButton.Click += (s, e) => OnSetProperty();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
            }
        }

        private async Task WaitForDeviceAsync()
        {
            while (true)
            {
                await Task.Delay(100);

                context.EnableNetDeviceEnumeration(true);

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
                    break;
                }
                else
                {
                    deviceList.Dispose();
                }
            }
        }

        private void OnDeviceInit()
        {
            if (devices.Count > 0)
            {
                curDevice = devices.Values.ElementAt(0);
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
            if (devices.Count > 0)
            {
                deviceSelector.SelectedIndex = 0;
            }
            else
            {
                deviceSelector.SelectedIndex = -1;
            }
        }

        private void OnDeviceSelect(int index)
        {
            if (index >= 0 && index < devices.Count)
            {
                curDevice = devices.Values.ElementAt(index);
                PrintLog("DeviceSelect: " + curDevice.GetDeviceInfo().Name() + "\n");
                UpdatePropertieItems();
                UpdatePropertySelector();
            }
        }

        private bool IsPrimaryTypeProperty(PropertyItem propertyItem)
        {
            return propertyItem.type == PropertyType.OB_BOOL_PROPERTY || propertyItem.type == PropertyType.OB_INT_PROPERTY
                || propertyItem.type == PropertyType.OB_FLOAT_PROPERTY;
        }

        private void UpdatePropertieItems()
        {
            propertyItems.Clear();
            uint size = curDevice.GetSupportedPropertyCount();

            for (uint i = 0; i < size; i++)
            {
                PropertyItem item = curDevice.GetSupportedProperty(i);
                if (IsPrimaryTypeProperty(item) && item.permission != PermissionType.OB_PERMISSION_DENY)
                {
                    propertyItems.Add(item);
                }
            }
            propertyItems.Sort((item1, item2) => item1.id.CompareTo(item2.id));
        }

        private string PermissionTypeToString(PermissionType type)
        {
            switch (type)
            {
                case PermissionType.OB_PERMISSION_READ:
                    return "[R/_]";
                case PermissionType.OB_PERMISSION_WRITE:
                    return "[_/W]";
                case PermissionType.OB_PERMISSION_READ_WRITE:
                    return "[R/W]";
                default:
                    break;
            }
            return "[_/_]";
        }

        private void UpdatePropertySelector()
        {
            propertySelector.Items.Clear();
            foreach (var property in propertyItems)
            {
                string permission = PermissionTypeToString(property.permission);
                propertySelector.Items.Add(permission + property.id.ToString());
            }
            if (propertyItems.Count > 0)
            {
                propertySelector.SelectedIndex = 0;
            }
        }

        private void OnPropertySelect(int index)
        {
            if (index >= 0 && index < propertyItems.Count)
            {
                curPropertyItem = propertyItems[index];
                PropertyId id = curPropertyItem.id;

                setText.Text = string.Empty;
                getText.Text = string.Empty;

                try
                {
                    switch (curPropertyItem.type)
                    {
                        case PropertyType.OB_BOOL_PROPERTY:
                            setText.ToolTip = $"Bool value(min:0, max:1, step:1)";
                            break;
                        case PropertyType.OB_INT_PROPERTY:
                            var intRange = curDevice.GetIntPropertyRange(id);
                            setText.ToolTip = $"Int Value(min:{intRange.min}, max:{intRange.max}, step:{intRange.step})";
                            break;
                        case PropertyType.OB_FLOAT_PROPERTY:
                            var floatRange = curDevice.GetFloatPropertyRange(id);
                            setText.ToolTip = $"Float Value(min:{floatRange.min}, max:{floatRange.max}, step:{floatRange.step})";
                            break;
                        default:
                            break;
                    }
                    PrintLog("PropertySelect: " + id.ToString() + "\n");
                }
                catch (NativeException e)
                {
                    PrintLog(e.Message);
                }
            }
        }

        private void OnGetProperty()
        {
            if (curDevice == null) return;

            setText.Text = string.Empty;
            getText.Text = string.Empty;

            try
            {
                PropertyId id = curPropertyItem.id;
                switch (curPropertyItem.type)
                {
                    case PropertyType.OB_BOOL_PROPERTY:
                        bool bv = curDevice.GetBoolProperty(id);
                        getText.Text = (bv ? 1 : 0).ToString();
                        break;
                    case PropertyType.OB_INT_PROPERTY:
                        int iv = curDevice.GetIntProperty(id);
                        getText.Text = iv.ToString();
                        break;
                    case PropertyType.OB_FLOAT_PROPERTY:
                        float fv = curDevice.GetFloatProperty(id);
                        getText.Text = fv.ToString();
                        break;
                    default:
                        break;
                }
                PrintLog("GetProperty: " + id.ToString() + " " + getText.Text + "\n");
            }
            catch (NativeException e)
            {
                PrintLog(e.Message);
            }
        }

        private void OnSetProperty()
        {
            if (curDevice == null) return;

            try
            {
                PropertyId id = curPropertyItem.id;
                int value = -1;
                switch (curPropertyItem.type)
                {
                    case PropertyType.OB_BOOL_PROPERTY:
                        if (int.TryParse(setText.Text, out value))
                        {
                            curDevice.SetBoolProperty(id, value == 1);
                        }
                        break;
                    case PropertyType.OB_INT_PROPERTY:
                        if (int.TryParse(setText.Text, out value))
                        {
                            Console.WriteLine(value);
                            curDevice.SetIntProperty(id, value);
                        }
                        break;
                    case PropertyType.OB_FLOAT_PROPERTY:
                        float fv; 
                        if (float.TryParse(setText.Text, out fv))
                        {
                            curDevice.SetFloatProperty(id, fv);
                        }
                        break;
                    default:
                        break;
                }
                PrintLog("SetProperty: " + id.ToString() + " " + setText.Text + "\n");
            }
            catch (NativeException e)
            {
                PrintLog(e.Message);
            }
        }

        private void PrintLog(string msg)
        {
            stringBuilder.AppendLine(msg);
            logText.Text = stringBuilder.ToString();
            logScrollViewer.ScrollToEnd();
        }

        private void Control_Closing(object sender, CancelEventArgs e)
        {
            if (devices != null)
            {
                foreach (var device in devices.Values)
                {
                    device.Dispose();
                }
                devices.Clear();
            }

            if (propertyItems != null)
            {
                propertyItems.Clear();
            }
        }
    }
}