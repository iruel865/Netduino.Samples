using Microsoft.SPOT.Hardware;

namespace TankCar
{
    public class CarController
    {
        const float STOP = 0.75f;
        const float FORWARD = 0.94f;        
        const float BACKWARD = 0.57f;

        protected PWM motorLeft;
        protected PWM motorRight;

        public CarController(PWM motorLeft, PWM motorRight)
        {
            this.motorLeft = motorLeft;
            this.motorRight = motorRight;
        }

        void UpdateMotors(float motorLeftValue, float motorRightValue)
        {
            motorLeft.Stop();
            motorRight.Stop();
            motorLeft.DutyCycle = motorLeftValue;
            motorRight.DutyCycle = motorRightValue;
            motorLeft.Start();
            motorRight.Start();
        }

        public void Stop() { UpdateMotors(STOP, STOP); }

        public void TurnLeft() { UpdateMotors(FORWARD, BACKWARD); }

        public void TurnRight() { UpdateMotors(BACKWARD, FORWARD); }

        public void MoveForward() { UpdateMotors(FORWARD, FORWARD); }

        public void MoveBackward() { UpdateMotors(BACKWARD, BACKWARD); }
    }
}