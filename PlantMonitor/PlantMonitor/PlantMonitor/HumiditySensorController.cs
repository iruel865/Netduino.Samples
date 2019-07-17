using MSH = Microsoft.SPOT.Hardware;
using SLH = SecretLabs.NETMF.Hardware;
using System.Threading;

namespace PlantMonitor
{
    public class HumiditySensorController
    {
        SLH.AnalogInput _analogPort;
        MSH.OutputPort _digitalPort;

        public HumiditySensorController(MSH.Cpu.Pin analogPort, MSH.Cpu.Pin digitalPort)
        {
            _analogPort = new SLH.AnalogInput(analogPort);
            _digitalPort = new MSH.OutputPort(digitalPort, false);
        }

        float ReadRaw()
        {
            int humidityRaw = 0;

            _digitalPort.Write(true);
            Thread.Sleep(5);
            humidityRaw = _analogPort.Read();
            _digitalPort.Write(false);

            return humidityRaw;
        }

        float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (((toHigh - toLow) * (value - fromLow)) / (fromHigh - fromLow)) - toLow;
        }

        public float Read()
        {
            float sample = 0;
            float humidity;

            for (int i = 0; i < 5; i++)
            {
                sample += (1024 - ReadRaw());
                Thread.Sleep(200);
            }

            humidity = sample / 5;

            float result = Map(humidity, 245, 675, 0, 1);

            if (result > 1)
                return 1;
            else if (result < 0)
                return 0.01f;
            else
                return result;
        }
    }
}