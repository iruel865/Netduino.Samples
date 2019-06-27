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

        protected DS3231 dS3231;
        protected RgbPwmLed rgbPwmLed;        

        protected Timer timer = null;
        protected TimerCallback timerCallback = null;

        protected MapleServer mapleServer;
        protected DisplayController displayController;
        protected HumiditySensorController humiditySensorController;

        public App()
        {
            InitializePeripherals();
            InitializeWebServer();

            Initializer.InitializeNetwork();
            Initializer.NetworkConnected += OnNetworkConnected;
        }
        void InitializePeripherals()
        {
            dS3231 = new DS3231(0x68, 100);
            dS3231.CurrentDateTime = new DateTime(2019, 6, 26, 23, 20, 00);

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
                N.Pins.GPIO_PIN_D7
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
            //_server.Start("PlantHost", Initializer.CurrentNetworkInterface.IPAddress);            

            displayController.Clear(true);
            displayController.DrawText("Connected!");
            Thread.Sleep(1000);

            rgbPwmLed.SetColor(Netduino.Foundation.Color.Green);

            AppLoop();
        }
        void AppLoop()
        {
            displayController.Clear(true);

            Thread thread = new Thread(() =>
            {                
                while (true)
                {
                    Thread.Sleep(500);
                    displayController.DrawText(dS3231.CurrentDateTime.ToString("hh:mm:ss tt"));
                }
            });
            thread.Start();
        }        
    }
}