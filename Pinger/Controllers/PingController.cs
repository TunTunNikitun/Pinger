using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.API_Services;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        [HttpPost]
        [Route("Start Background Ping")]
        public void BackgroundPingStarting()
        {
   
           
            RecurringJob.AddOrUpdate(() => BackgroundPinger.Ping(), Cron.Minutely);
        }
        [HttpPost]
        [Route("Stop Background Ping")]
        public void BackgroundPingStop()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }
        }
    }
}
