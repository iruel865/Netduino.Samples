using Microsoft.SPOT;
using Netduino.Foundation.LEDs;
using Netduino.Foundation.Network;
using System.Collections;
using System.Threading;
using Maple;
using N = SecretLabs.NETMF.Hardware.Netduino;
using System;

namespace PlantMonitor
{
    public class App
    {
        public static ArrayList HumidityLogs;

        float humidity;

        Timer timer = null;
        TimerCallback timerCallback = null;

        RgbPwmLed rgbPwmLed;
        LedBarGraph ledBarGraph;

        MapleServer mapleServer;
        DisplayController displayController;
        HumiditySensorController humiditySensorController;

        public App()
        {
            InitializePeripherals();
            InitializeWebServer();

            HumidityLogs = new ArrayList();

            Initializer.InitializeNetwork();
            Initializer.NetworkConnected += OnNetworkConnected;
        }
        void InitializePeripherals()
        {
            ledBarGraph = new LedBarGraph(
                new Microsoft.SPOT.Hardware.Cpu.Pin[10] 
                {
                    N.Pins.GPIO_PIN_D0,
                    N.Pins.GPIO_PIN_D1,
                    N.Pins.GPIO_PIN_D2,
                    N.Pins.GPIO_PIN_D3,
                    N.Pins.GPIO_PIN_D4,
                    N.Pins.GPIO_PIN_D5,
                    N.Pins.GPIO_PIN_D6,
                    N.Pins.GPIO_PIN_D7,
                    N.Pins.GPIO_PIN_D8,
                    N.Pins.GPIO_PIN_D12
                }
            );

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

            displayController = new DisplayController();
            displayController.DrawText("Connecting...");

            humiditySensorController = new HumiditySensorController
            (
                analogPort: N.Pins.GPIO_PIN_A0,
                digitalPort: N.Pins.GPIO_PIN_D13
            );
        }
        void InitializeWebServer()
        {
            var handler = new RequestHandler();
            handler.GetPlantHumidity += OnGetPlantHumidity;

            mapleServer = new MapleServer();
            mapleServer.AddHandler(handler);
        }

        void OnNetworkConnected(object sender, EventArgs e)
        {
            timerCallback = new TimerCallback(OnTimerInterrupt);
            timer = new Timer(timerCallback, null, TimeSpan.FromTicks(0), new TimeSpan(1, 0, 0));
            mapleServer.Start("PlantHost", Initializer.CurrentNetworkInterface.IPAddress);

            displayController.Clear(true);
            displayController.DrawText("Connected!");
            Thread.Sleep(1000);

            rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);
        }

        void OnGetPlantHumidity(object sender, EventArgs e)
        {
            displayController.DrawText("Sensing...");

            rgbPwmLed.StartBlink(Netduino.Foundation.Color.Orange);
            Thread.Sleep(1000);
            rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);

            UpdateHumidity();
        }

        void OnTimerInterrupt(object state)
        {
            rgbPwmLed.StartBlink(Netduino.Foundation.Color.Blue);
            Thread.Sleep(1000);
            rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);

            UpdateHumidity();
        }

        void UpdateHumidity()
        {
            humidity = (humiditySensorController.Read());
            HumidityLogs.Clear();
            HumidityLogs.Add(new HumidityLog() { Humidity = humidity });
            displayController.DrawText(((int)(humidity*100))+"%");

            ledBarGraph.Percentage = humidity;
        }
    }
}