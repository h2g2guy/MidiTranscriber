using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Midi;
using Windows.Devices.Enumeration;

namespace WindowsMidiEngineUWP
{
    public class MidiEngine
    {
        public static List<string> GetInputDevices()
        {
            Task<DeviceInformationCollection> getDevices = DeviceInformation.FindAllAsync(MidiInPort.GetDeviceSelector()).AsTask();
            getDevices.Wait();
            DeviceInformationCollection inputDevices = getDevices.Result;

            List<string> names = new List<string>();
            foreach (DeviceInformation device in inputDevices)
            {
                names.Add(device.Name);
            }

            return names;
        }
    }
}
