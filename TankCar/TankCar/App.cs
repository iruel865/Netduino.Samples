using System;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.LEDs;
using Netduino.Foundation.Network;

namespace TankCar
{
    public class App
    {
        Led ledRed;
        Led ledGreen;

        public App()
        {
            InitializePeripherals();

            Initializer.InitializeNetwork();
            Initializer.NetworkConnected += OnNetworkConnected;
        }

        void InitializePeripherals()
        {
            ledRed = new Led(Pins.GPIO_PIN_D13);
            ledGreen = new Led(Pins.GPIO_PIN_D12);

            ledRed.StartBlink();
        }

        void OnNetworkConnected(object sender, EventArgs e)
        {
            Debug.Print("InitializeNetwork()");

            //mapleServer.Start("CarHost", Initializer.CurrentNetworkInterface.IPAddress);

            ledRed.Stop();
            ledGreen.IsOn = true;
            //carController.Stop();
        }

    }
}