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

        protected Timer _timer = null;
        protected TimerCallback _timerCallback = null;

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
            InitializeWebServer();

            HumidityLogs = new ArrayList();

            Initializer.InitializeNetwork();
            Initializer.NetworkConnected += OnNetworkConnected;

            //AppLoop();
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
                N.Pins.GPIO_PIN_A0,
                N.Pins.GPIO_PIN_D13
            );
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
                displayController.DrawText("Sensing...");

                rgbPwmLed.StartBlink(Netduino.Foundation.Color.Orange);
                Thread.Sleep(1000);
                rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);

                displayController.DrawText("Connected!");
            });
            _animationThread.Start();
        }

        void OnNetworkConnected(object sender, EventArgs e)
        {
            _timerCallback = new TimerCallback(OnTimerInterrupt);
            _timer = new Timer(_timerCallback, null, TimeSpan.FromTicks(0), new TimeSpan(1, 0, 0));
            mapleServer.Start("PlantHost", Initializer.CurrentNetworkInterface.IPAddress);            

            displayController.Clear(true);
            displayController.DrawText("Connected!");
            Thread.Sleep(1000);

            rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);
        }

        void OnTimerInterrupt(object state)
        {
            float sample = 0;
            for (int i = 0; i < 5; i++)
            {
                sample += (humiditySensorController.Read() / 100);
                Thread.Sleep(200);
            }

            float humidity = sample / 5;

            if (humidity > 1)
                ledBarGraph.Percentage = 1;
            else if (humidity < 0)
                ledBarGraph.Percentage = 0;
            else
                ledBarGraph.Percentage = humidity;

            HumidityLogs.Clear();
            HumidityLogs.Add(new HumidityLog()
            {               
                Humidity = humidity
            });

            Thread _animationThread = new Thread(() =>
            {
                rgbPwmLed.StartBlink(Netduino.Foundation.Color.Blue);
                Thread.Sleep(1000);
                rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);
            });
            _animationThread.Start();
        }

        //void AppLoop()
        //{
        //    Thread thread = new Thread(() =>
        //    {
        //        while (true)
        //        {
        //            float humidity = (humiditySensorController.Read() / 100);

        //            if (humidity > 1)
        //                ledBarGraph.Percentage = 1;
        //            else if (humidity < 0)
        //                ledBarGraph.Percentage = 0;
        //            else
        //                ledBarGraph.Percentage = humidity;

        //            Debug.Print("Humidity = " + humidity);
        //            Thread.Sleep(500);
        //        }
        //    });
        //    thread.Start();
        //}
    }
}