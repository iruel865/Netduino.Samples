using Microsoft.SPOT.Hardware;

namespace TankCar
{
    public class TankController
    {
        const float STOP = 0.75f;
        const float FORWARD = 0.94f;        
        const float BACKWARD = 0.57f;

        protected PWM motorPwmLeft;
        protected PWM motorPwmRight;

        public TankController(Cpu.PWMChannel motorLeftPin, Cpu.PWMChannel motorRightPin)
        {
            motorPwmLeft = new PWM(motorLeftPin, 1000, 0.75f, false);
            motorPwmRight = new PWM(motorRightPin, 1000, 0.75f, false);
        }

        void UpdateMotors(float motorLeftValue, float motorRightValue)
        {
            motorPwmLeft.Stop();
            motorPwmRight.Stop();
            motorPwmLeft.DutyCycle = motorLeftValue;
            motorPwmRight.DutyCycle = motorRightValue;
            motorPwmLeft.Start();
            motorPwmRight.Start();
        }

        public void Stop() { UpdateMotors(STOP, STOP); }

        public void TurnLeft() { UpdateMotors(FORWARD, BACKWARD); }

        public void TurnRight() { UpdateMotors(BACKWARD, FORWARD); }

        public void MoveForward() { UpdateMotors(FORWARD, FORWARD); }

        public void MoveBackward() { UpdateMotors(BACKWARD, BACKWARD); }
    }
}