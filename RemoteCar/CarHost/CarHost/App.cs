using Microsoft.SPOT;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Motors;
using Maple;
using Netduino.Foundation.Network;
using Netduino.Foundation.LEDs;

namespace CarHost
{
    public class App
    {
        Led ledRed;
        Led ledGreen;
        Led ledBlue;

        MapleServer mapleServer;

        HBridgeMotor motorLeft;
        HBridgeMotor motorRight;

        CarController carController;

        public App()
        {
            InitializePeripherals();
            InitializeWebServer();

            Initializer.InitializeNetwork();
            Initializer.NetworkConnected += OnNetworkConnected;
        }
        void InitializePeripherals()
        {
            ledRed = new Led(N.Pins.GPIO_PIN_D13);
            ledGreen = new Led(N.Pins.GPIO_PIN_D12);
            ledBlue = new Led(N.Pins.GPIO_PIN_D11);

            motorLeft = new HBridgeMotor(N.PWMChannels.PWM_PIN_D3, N.PWMChannels.PWM_PIN_D5, N.Pins.GPIO_PIN_D4);
            motorRight = new HBridgeMotor(N.PWMChannels.PWM_PIN_D6, N.PWMChannels.PWM_PIN_D10, N.Pins.GPIO_PIN_D7);
            carController = new CarController(motorLeft, motorRight);

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

            carController.Stop();
            ledGreen.IsOn = true;
            ledBlue.IsOn = false;
        }

        void OnTurnLeft(object sender, EventArgs e)
        {
            Debug.Print("LEFT");
            carController.TurnLeft();

            ledGreen.IsOn = false;
            ledBlue.IsOn = true;
        }

        void OnTurnRight(object sender, EventArgs e)
        {
            Debug.Print("RIGHT");
            carController.TurnRight();

            ledGreen.IsOn = false;
            ledBlue.IsOn = true;
        }

        void OnMoveForward(object sender, EventArgs e)
        {
            Debug.Print("FORWARDS");
            carController.MoveForward();

            ledGreen.IsOn = false;
            ledBlue.IsOn = true;
        }

        void OnMoveBackward(object sender, EventArgs e)
        {
            Debug.Print("BACKWARDS");
            carController.MoveBackward();

            ledGreen.IsOn = false;
            ledBlue.IsOn = true;
        }

        void OnNetworkConnected(object sender, EventArgs e)
        {
            Debug.Print("InitializeNetwork()");

            mapleServer.Start("CarHost", Initializer.CurrentNetworkInterface.IPAddress);

            ledRed.Stop();
            ledGreen.IsOn = true;
            carController.Stop();
        }
    }
}