using System;
using Microsoft.SPOT;
using System.Threading;
using Netduino.Foundation.Displays;
using Netduino.Foundation;
using SecretLabs.NETMF.Hardware.Netduino;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Servos;
using Netduino.Foundation.Relays;

namespace ServoSonar
{
    public class App
    {
        Relay relay;

        //const int DRAW_TIME = 500;
        //protected ILI9163 tftDisplay;
        //protected GraphicsLibrary display;
        //protected Servo servo;

        public App()
        {            
            InitializePeripherals();
            //DrawShapes();
            //TestServo();

            TestRelay();
        }
        protected void InitializePeripherals()
        {
            relay = new Relay(Pins.GPIO_PIN_D7);

            //tftDisplay = new ILI9163
            //(
            //    chipSelectPin: Pins.GPIO_PIN_D3,
            //    dcPin: Pins.GPIO_PIN_D7,
            //    resetPin: Pins.GPIO_PIN_D6,
            //    width: 128,
            //    height: 160,
            //    spiModule: SPI.SPI_module.SPI1,
            //    speedKHz: 15000
            //);
            //tftDisplay.ClearScreen(31);
            //tftDisplay.Refresh();
            //display = new GraphicsLibrary(tftDisplay);

            //servo = new Servo(PWMChannels.PWM_PIN_D10, NamedServoConfigs.Ideal180Servo);
            //servo.RotateTo(0);
        }

        protected void TestRelay()
        {
            while (true)
            {
                relay.IsOn = !relay.IsOn;
                Thread.Sleep(5000);
            }
        }

        //protected void TestServo()
        //{
        //    int _rotationAngle = 0;
        //    bool _isRotating = true;
        //    Thread _animationThread = new Thread(() =>
        //    {
        //        while (_isRotating)
        //        {
        //            while (_rotationAngle < 180)
        //            {
        //                if (!_isRotating)
        //                    break;
        //                _rotationAngle++;
        //                servo.RotateTo(_rotationAngle);
        //                Thread.Sleep(5);
        //            }
        //            while (_rotationAngle > 0)
        //            {
        //                if (!_isRotating)
        //                    break;
        //                _rotationAngle--;
        //                servo.RotateTo(_rotationAngle);
        //                Thread.Sleep(5);
        //            }
        //        }
        //    });
        //    _animationThread.Start();
        //}

        /*protected void DrawShapes()
        {
            display.Clear(true);

            // Filled Rectangles
            display.DrawRectangle(0, 0, 127, 159, Color.Red);
            //display.DrawRectangle(37, 6, 20, 20, Color.Red, true);
            //display.DrawRectangle(65, 9, 14, 14, Color.Red, true);
            //display.DrawRectangle(87, 12, 8, 8, Color.Red, true);
            //display.DrawRectangle(103, 15, 2, 2, Color.Red, true);
            display.Show();

            // Empty Rectangles
            //display.DrawRectangle(3, 32, 26, 26, Color.Red);
            //display.DrawRectangle(37, 35, 20, 20, Color.Red);
            //display.DrawRectangle(65, 38, 14, 14, Color.Red);
            //display.DrawRectangle(87, 41, 8, 8, Color.Red);
            //display.DrawRectangle(103, 44, 2, 2, Color.Red);
            //display.Show();

            // Filled Circles
            //display.DrawCircle(16, 73, 13, Color.Green, true);
            //display.DrawCircle(47, 73, 10, Color.Green, true);
            //display.DrawCircle(72, 73, 7, Color.Green, true);
            //display.DrawCircle(91, 73, 4, Color.Green, true);
            //display.DrawCircle(104, 73, 1, Color.Green, true);
            //display.Show();

            // Empty Circles
            //DrawArc(64, 99, 70, Color.Green, true);
            DrawArc(5, 5, 5, Color.Green);
            display.Show();

            // Horizontal, vertical and specific lines
            //for (int i = 0; i < 9; i++)
            //    display.DrawHorizontalLine(3, 123 + (i * 4), 26, Color.Blue);
            //for (int i = 0; i < 7; i++)
            //    display.DrawVerticalLine(37 + (i * 4), 123, 33, Color.Blue);
            //display.DrawLine(70, 131, 94, 147, Color.Blue);
            //display.DrawLine(70, 123, 94, 155, Color.Blue);
            //display.DrawLine(78, 123, 86, 155, Color.Blue);
            //display.DrawLine(86, 123, 78, 155, Color.Blue);
            //display.DrawLine(94, 123, 70, 155, Color.Blue);
            //display.DrawLine(94, 131, 70, 147, Color.Blue);
            //display.DrawLine(70, 139, 94, 139, Color.Blue);
            //display.Show


            Thread.Sleep(5000);
        }*/

       
       
    }
}