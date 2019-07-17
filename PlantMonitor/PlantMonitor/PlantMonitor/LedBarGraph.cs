using System;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.LEDs;
using System.Threading;

namespace PlantMonitor
{
    /// <summary>
    /// Represents an LED bar graph composed on multiple LEDs
    /// </summary>
    public class LedBarGraph
    {
        /// <summary>
        /// The number of the LEDs in the bar graph
        /// </summary>
        public int Count { get { return _leds.Length; } }

        /// <summary>
        /// A value between 0 and 1 that controls the number of LEDs that are activated
        /// </summary>
        public float Percentage { set { SetPercentage(value); } }

        protected Led[] _leds;

        /// <summary>
        /// Create an LedBarGraph instance from an array of IDigitalOutputPort
        /// </summary>
        public LedBarGraph(Cpu.Pin[] pins)
        {
            _leds = new Led[pins.Length];

            for (int i = 0; i < pins.Length; i++)
            {
                _leds[i] = new Led(pins[i]);
            }
        }

        /// <summary>
        /// Set the LED state
        /// </summary>
        /// <param name="index">index of the LED</param>
        /// <param name="isOn"></param>
        public void SetLed(int index, bool isOn)
        {
            _leds[index].IsOn = isOn;
        }

        void Reset()
        {
            for (int i = 1; i <= Count; i++)
            {
                SetLed(i - 1, false);
            }
        }

        /// <summary>
        /// Set the percentage of LEDs that are on starting from index 0
        /// </summary>
        /// <param name="percentage"></param>
        void SetPercentage(float percentage) //assume 0 - 1
        {
            if (percentage < 0 || percentage > 1)
                throw new ArgumentOutOfRangeException();

            Reset();

            float value = percentage * Count;

            for (int i = 1; i <= Count; i++)
            {
                if (i <= value)
                {
                    SetLed(i - 1, true);
                }
                else
                {
                    SetLed(i - 1, false);
                }

                Thread.Sleep(100);
            }
        }
    }
}