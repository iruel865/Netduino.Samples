using Netduino.Foundation.LEDs;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace PlantMonitor
{
    public class App
    {
        protected RgbPwmLed rgbLed;

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
                redLedForwardVoltage: 2.1f,
                greenLedForwardVoltage: 3.0f,
                blueLedForwardVoltage: 3.0f
            );

            rgbLed.SetColor(Netduino.Foundation.Color.Red);
        }

        public void Run()
        {
            rgbLed.SetColor(Netduino.Foundation.Color.Green);
        }
    }
}
