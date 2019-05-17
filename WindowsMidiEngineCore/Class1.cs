using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Midi;
using Windows.Devices.Enumeration;

namespace WindowsMidiEngine
{
    public class MidiEngine
    {
        public static async Task<List<string>> GetInputDevices()
        {
            DeviceInformationCollection inputDevices = await DeviceInformation.FindAllAsync(MidiInPort.GetDeviceSelector());

            List<string> names = new List<string>();
            foreach (DeviceInformation device in inputDevices)
            {
                names.Add(device.Name);
            }

            return names;
        }
    }
}
