using Netduino.Foundation.Displays;
using Netduino.Foundation.LEDs;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace PlantMonitor
{
    public class App
    {
        protected RgbPwmLed rgbLed;
        protected GraphicsLibrary graphics;

        public App()
        {
            InitializePeripherals();
        }

        private void InitializePeripherals()
        {
            rgbLed = new RgbPwmLed
            (
                redPin:   N.PWMChannels.PWM_PIN_D11,
                greenPin: N.PWMChannels.PWM_PIN_D10,
                bluePin:  N.PWMChannels.PWM_PIN_D9,
                redLedForwardVoltage: 1.05f,
                greenLedForwardVoltage: 1.5f,
                blueLedForwardVoltage: 1.5f
            );

            var display = new SSD1306(0x3c, 400, SSD1306.DisplayType.OLED128x32);
            graphics = new GraphicsLibrary(display);

            rgbLed.SetColor(Netduino.Foundation.Color.Red);
        }

        public void Run()
        {
            rgbLed.SetColor(Netduino.Foundation.Color.Green);

            graphics.CurrentFont = new Font8x12();
            graphics.DrawText(0, 0, "Hello World!");
            graphics.Show();
        }
    }
}
