using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.LEDs;
using Netduino.Foundation.Network;
using Maple;

namespace TankCar
{
    public class App
    {
        Led ledRed;
        Led ledGreen;
        Led ledBlue;

        MapleServer mapleServer;

        TankController tankController;

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
            ledBlue = new Led(Pins.GPIO_PIN_D11);

            tankController = new TankController(PWMChannels.PWM_PIN_D3, PWMChannels.PWM_PIN_D10);

            ledRed.StartBlink();
        }

        void InitializeWebServer()
        {
            var handler = new RequestHandler();
            handler.Stop += OnStop;
            handler.TurnLeft += OnTurnLeft;
            handler.TurnRight += OnTurnRight;
            handler.MoveForward += OnMoveForward;
            handler.MoveBackward += OnMoveBackward;

            mapleServer = new MapleServer();
            mapleServer.AddHandler(handler);
        }

        void OnStop(object sender, EventArgs e)
        {
            Debug.Print("STOP");

            tankController.Stop();
            ledGreen.IsOn = true;
            ledBlue.IsOn = false;
        }

        void OnTurnLeft(object sender, EventArgs e)
        {
            Debug.Print("LEFT");
            tankController.TurnLeft();

            ledGreen.IsOn = false;
            ledBlue.IsOn = true;
        }

        void OnTurnRight(object sender, EventArgs e)
        {
            Debug.Print("RIGHT");
            tankController.TurnRight();

            ledGreen.IsOn = false;
            ledBlue.IsOn = true;
        }

        void OnMoveForward(object sender, EventArgs e)
        {
            Debug.Print("FORWARDS");
            tankController.MoveForward();

            ledGreen.IsOn = false;
            ledBlue.IsOn = true;
        }

        void OnMoveBackward(object sender, EventArgs e)
        {
            Debug.Print("BACKWARDS");
            tankController.MoveBackward();

            ledGreen.IsOn = false;
            ledBlue.IsOn = true;
        }

        void OnNetworkConnected(object sender, EventArgs e)
        {
            Debug.Print("InitializeNetwork()");

            mapleServer.Start("CarHost", Initializer.CurrentNetworkInterface.IPAddress);

            ledRed.Stop();
            ledGreen.IsOn = true;
            tankController.Stop();
        }
    }
}