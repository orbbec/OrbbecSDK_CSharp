using System.Collections;
using System.Collections.Generic;

namespace Orbbec
{
    public class TypeHelper
    {
        public static StreamType ConvertSensorTypeToStreamType(SensorType sensorType)
        {
            return obNative.ob_sensor_type_to_stream_type(sensorType);
        }
    }
}
