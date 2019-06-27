using System.Threading;

namespace PlantMonitor
{
    public class Program
    {
        public static void Main()
        {
            App app = new App();
            app.Run();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}