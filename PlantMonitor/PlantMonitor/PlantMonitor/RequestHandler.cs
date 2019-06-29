using Microsoft.SPOT;
using Maple;

namespace PlantMonitor
{
    public class RequestHandler : RequestHandlerBase
    {
        public event EventHandler GetPlantHumidity = delegate { };

        public void getPlantHumidity()
        {
            GetPlantHumidity(this, EventArgs.Empty);
            StatusResponse();
        }

        protected void StatusResponse()
        {
            Context.Response.ContentType = "application/json";
            Context.Response.StatusCode = 200;
            Send(App.HumidityLogs);
        }
    }
}