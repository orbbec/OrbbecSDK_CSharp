using System;
using Orbbec;

using Version = Orbbec.Version;

namespace Orbbec
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Print the sdk version number, the sdk version number is divided into major version number, minor version number and revision number
                Console.WriteLine("SDK version: " + Version.GetMajor() + "." + Version.GetMinor() + "." + Version.GetPatch());

                // Create a Context.
                Context ctx = new Context();

                // Query the list of connected devices
                DeviceList devList = ctx.QueryDeviceList();

                // Get the number of connected devices
                if (devList.DeviceCount() == 0)
                {
                    Console.Error.WriteLine("Device not found!");
                    return;
                }

                // Create a device, 0 means the index of the first device
                Device dev = devList.GetDevice(0);

                // Get device information
                DeviceInfo devInfo = dev.GetDeviceInfo();

                // Get the name of the device
                Console.WriteLine("Device name: " + devInfo.Name());

                // Get the pid, vid, uid of the device
                Console.WriteLine("Device pid: " + devInfo.Pid() + " vid: " + devInfo.Vid() + " uid: " + devInfo.Uid());

                // By getting the firmware version number of the device
                string fwVer = devInfo.FirmwareVersion();
                Console.WriteLine("Firmware version: " + fwVer);

                // By getting the serial number of the device
                string sn = devInfo.SerialNumber();
                Console.WriteLine("Serial number: " + sn);

                // By getting the connection type of the device
                string connectType = devInfo.ConnectionType();
                Console.WriteLine("ConnectionType: " + connectType);

                // Get the list of supported sensors
                Console.WriteLine("Sensor types:");
                SensorList sensorList = dev.GetSensorList();
                for(UInt32 i = 0; i < sensorList.SensorCount(); i++)
                {
                    Sensor sensor = sensorList.GetSensor(i);
                    switch (sensor.GetSensorType())
                    {
                        case SensorType.OB_SENSOR_COLOR:
                            Console.WriteLine("\tColor sensor");
                            break;
                        case SensorType.OB_SENSOR_DEPTH:
                            Console.WriteLine("\tDepth sensor");
                            break;
                        case SensorType.OB_SENSOR_IR:
                            Console.WriteLine("\tIR sensor");
                            break;
                        case SensorType.OB_SENSOR_IR_LEFT:
                            Console.WriteLine("\tIR Left sensor");
                            break;
                        case SensorType.OB_SENSOR_IR_RIGHT:
                            Console.WriteLine("\tIR Right sensor");
                            break;
                        case SensorType.OB_SENSOR_GYRO:
                            Console.WriteLine("\tGyro sensor");
                            break;
                        case SensorType.OB_SENSOR_ACCEL:
                            Console.WriteLine("\tAccel sensor");
                            break;
                        default:
                            break;
                    }
                }

                Console.WriteLine("Press ESC to exit! ");

                while (true)
                {
                    // Get the value of the pressed key, if it is the esc key, exit the program
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(-1);
            }
        }
    }
}