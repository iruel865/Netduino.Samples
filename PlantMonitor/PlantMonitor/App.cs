using Microsoft.SPOT;
using Netduino.Foundation.Displays;
using Netduino.Foundation.LEDs;
using Netduino.Foundation.Network;
using Netduino.Foundation.RTCs;
using System.Threading;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace PlantMonitor
{
    public class App
    {
        protected DS3231 dS3231;
        protected RgbPwmLed rgbPwmLed;
        protected GraphicsLibrary graphicsLibrary;

        public App()
        {
            InitializePeripherals();
        }

        private void InitializePeripherals()
        {
            dS3231 = new DS3231(0x68, 100);
            //dS3231.CurrentDateTime = new DateTime(2019, 6, 23, 23, 43, 00);

            rgbPwmLed = new RgbPwmLed
            (
                redPin:   N.PWMChannels.PWM_PIN_D11,
                greenPin: N.PWMChannels.PWM_PIN_D10,
                bluePin:  N.PWMChannels.PWM_PIN_D9,
                redLedForwardVoltage:   1.05f,
                greenLedForwardVoltage: 1.5f,
                blueLedForwardVoltage:  1.5f
            );
            rgbPwmLed.StartPulse(Netduino.Foundation.Color.Red);

            var display = new SSD1306(0x3C, 400, SSD1306.DisplayType.OLED128x32);
            graphicsLibrary = new GraphicsLibrary(display);
        }

        public void Run()
        {
            Initializer.InitializeNetwork();
            Initializer.NetworkConnected += OnNetworkConnected;
        }
        private void OnNetworkConnected(object sender, EventArgs e)
        {
            //_timerCallback = new TimerCallback(OnTimerInterrupt);
            //_timer = new Timer(_timerCallback, null, TimeSpan.FromTicks(0), new TimeSpan(0, 30, 0));
            //_server.Start("PlantHost", Initializer.CurrentNetworkInterface.IPAddress);
            //_rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);

            rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);

            AppLoop();
        }
        private void AppLoop()
        {
            Thread thread = new Thread(() =>
            {                
                while (true)
                {
                    Thread.Sleep(500);
                    DrawText();
                }
            });
            thread.Start();
        }

        private void DrawText()
        {
            graphicsLibrary.Clear();
            graphicsLibrary.DrawRectangle(0, 0, 128, 32);
            graphicsLibrary.CurrentFont = new Font8x12();
            graphicsLibrary.DrawText(20, 12, dS3231.CurrentDateTime.ToString("hh:mm:ss tt"));
            graphicsLibrary.Show();
        }
    }
}