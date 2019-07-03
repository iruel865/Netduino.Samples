using Microsoft.SPOT;
using Netduino.Foundation.Displays;
using Netduino.Foundation.LEDs;
using Netduino.Foundation.Network;
using Netduino.Foundation.RTCs;
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

        protected RgbPwmLed rgbPwmLed;
        protected LedBarGraph ledBarGraph;

        protected Timer timer = null;
        protected TimerCallback timerCallback = null;

        protected MapleServer mapleServer;
        protected DisplayController displayController;
        protected HumiditySensorController humiditySensorController;

        public App()
        {
            InitializePeripherals();
            //InitializeWebServer();

            //Initializer.InitializeNetwork();
            //Initializer.NetworkConnected += OnNetworkConnected;

            AppLoop();
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

            //humiditySensorController = new HumiditySensorController
            //(
            //    N.Pins.GPIO_PIN_A0,
            //    N.Pins.GPIO_PIN_D7
            //);
        }
        void InitializeWebServer()
        {
            var handler = new RequestHandler();
            handler.GetPlantHumidity += OnGetPlantHumidity;

            mapleServer = new MapleServer();
            mapleServer.AddHandler(handler);
        }
        void OnGetPlantHumidity(object sender, EventArgs e)
        {
            Thread _animationThread = new Thread(() =>
            {
                rgbPwmLed.StartBlink(Netduino.Foundation.Color.Orange);
                Thread.Sleep(1000);
                rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);
            });
            _animationThread.Start();
        }

        void OnNetworkConnected(object sender, EventArgs e)
        {
            //_timerCallback = new TimerCallback(OnTimerInterrupt);
            //_timer = new Timer(_timerCallback, null, TimeSpan.FromTicks(0), new TimeSpan(0, 30, 0));
            mapleServer.Start("PlantHost", Initializer.CurrentNetworkInterface.IPAddress);            

            displayController.Clear(true);
            displayController.DrawText("Connected!");
            Thread.Sleep(1000);

            rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);

            //AppLoop();
        }

        void AppLoop()
        {
            Thread thread = new Thread(() =>
            {
                bool state = true;

                while (true)
                {
                    state = true;

                    for (int i = 0; i < ledBarGraph.Count; i++)
                    {
                        ledBarGraph.SetLed(i, state);
                        Thread.Sleep(500);
                    }

                    state = false;

                    for (int i = ledBarGraph.Count - 1; i >= 0 ; i--)
                    {
                        ledBarGraph.SetLed(i, state);
                        Thread.Sleep(500);
                    }
                }
            });
            thread.Start();
        }        
    }
}