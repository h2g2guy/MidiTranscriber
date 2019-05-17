using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Midi;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using System.Threading;
using System.Linq;

namespace WindowsMidiEngine
{
    public class MidiEngine
    {
        public static IEnumerable<string> GetInputDevices()
        {
            IAsyncOperation<DeviceInformationCollection> inputDevices = DeviceInformation.FindAllAsync(MidiInPort.GetDeviceSelector());

            ManualResetEvent opCompletedEvent = new ManualResetEvent(false);

            inputDevices.Completed = (a, b) => { opCompletedEvent.Set(); };

            opCompletedEvent.WaitOne();

            // Check the status of the operation
            if (inputDevices.Status == AsyncStatus.Error)
            {
                DeviceInformationCollection deploymentResult = inputDevices.GetResults();
                Console.WriteLine("Error code: {0}", inputDevices.ErrorCode);
            }
            else if (inputDevices.Status == AsyncStatus.Canceled)
            {
                Console.WriteLine("Installation canceled");
            }
            else if (inputDevices.Status == AsyncStatus.Completed)
            {
                Console.WriteLine("Installation succeeded");
            }
            else
            {
                Console.WriteLine("Installation status unknown");
            }

            return inputDevices.GetResults().Select(device => device.Name);
        }
    }
}
