using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AllInOneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MidiInPort deepMind;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (deepMind != null)
            {
                deepMind.Dispose();
            }
        }

        private async void GrabDeepMind_Click(object sender, RoutedEventArgs e)
        {
            await EnumerateDevices();
        }

        private async Task EnumerateDevices()
        {
            var inputDevices = await DeviceInformation.FindAllAsync(MidiInPort.GetDeviceSelector());

            deepMind = null;

            foreach (DeviceInformation device in inputDevices)
            {
                if (device.Name.Contains("DeepMind"))
                {
                    if (deepMind != null)
                    {
                        deepMind.Dispose();
                    }
                    deepMind = await MidiInPort.FromIdAsync(device.Id);
                    this.textBlock.Text = "Captured device!";
                }
            }

            if (deepMind == null)
            {
                return;
            }

            deepMind.MessageReceived += DeepMind_MessageReceived;
        }

        private async void DeepMind_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
        {
            if (args.Message.Type == MidiMessageType.NoteOn)
            {
                var noteOnMessage = (MidiNoteOnMessage)args.Message;

                await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    this.textBlock.Text = noteOnMessage.Note + ", velocity " + noteOnMessage.Velocity;
                }
                );
            }
        }
    }
}
