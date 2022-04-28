using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.API_Services;
using Microsoft.Extensions.Options;

namespace WebApplication3.Controllers
{
    public static class BackgroundPinger
    { 
        public static void Ping()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                
               var servicesToPing = (db.Services.AsQueryable().Where(s=> s.Status==true)).ToList();
                if (servicesToPing.Count > 0)
                {
                    foreach (var service in servicesToPing)
                    {
                        var code = SiteAvailability(service.Url);
                        if (code!="OK")
                        {
                            Log log = new Log();
                            log.ServiseId = service.Id;
                            log.PingResalt = code.ToString();
                            log.PingTime = DateTime.Now.ToString();
                            db.Log.Add(log);
                            db.SaveChanges();
                            SendPing(service, code);
                        }
                    }
                }
            }
        }
        private static void SendPing(Services service, string code)
        {
            NotificationModel not = new NotificationModel();
            not.Title = "Connecting Error";
            not.IsAndroiodDevice = true;
            not.Body = service.Name + " "+ code;
            using (ApplicationContext db = new ApplicationContext())
            {
                not.DeviceId = db.AndroidDevices.AsQueryable().Select(x => x.DeviceId).ToList();
                    }
            FcmNotificationSetting settings = new FcmNotificationSetting();
            settings.SenderId = "842859204982";
            settings.ServerKey = "AAAAxD5S0XY:APA91bF89LSiUMaJghaQEO2UXLHAqErxYFvsQ005YDTkQdPHF6v7gug53o3vwE0PIF7YGObpc58ZvNbts-9I3alW1WbSMdnUlGNfc1k1fN-PQXMozcn3KS_lkwOBs2NtMDHfswfhxKxJ";
  
            NotificationService s = new NotificationService(settings);
            
            s.SendNotification(not);
        }
        private static string SiteAvailability(string uri)
        {
            string available;
            try
            {
                var request = WebRequest.Create(uri);
                request.Credentials = CredentialCache.DefaultCredentials;
                var response = (HttpWebResponse)request.GetResponse();
                available = response.StatusCode.ToString();

            }
            catch(WebException webEx)
            {
                available = webEx.Status.ToString();
            }
            return available;
        }
       
    }
}
