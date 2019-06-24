using Netduino.Foundation.Displays;
using Netduino.Foundation.LEDs;
using Netduino.Foundation.RTCs;
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
                redLedForwardVoltage: 1.05f,
                greenLedForwardVoltage: 1.5f,
                blueLedForwardVoltage: 1.5f
            );
            rgbPwmLed.SetColor(Netduino.Foundation.Color.Red);

            var display = new SSD1306(0x3C, 400, SSD1306.DisplayType.OLED128x32);
            graphicsLibrary = new GraphicsLibrary(display);
        }

        public void Run()
        {
            rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);

            graphicsLibrary.DrawRectangle(0, 0, 128, 32);
            graphicsLibrary.CurrentFont = new Font8x12();
            graphicsLibrary.DrawText(8, 12, dS3231.CurrentDateTime.ToString("HH:mm dd/MM/yy"));
            graphicsLibrary.Show();
        }
    }
}
