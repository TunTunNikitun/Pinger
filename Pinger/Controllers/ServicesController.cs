using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        [HttpPost]
        [Route("Add Service")]
       public void ServiceAdd(string name, string url)
        {
            Services serv = new Services
            {
                Name = name,
                Url = url
            };
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Services.Add(serv);
                db.SaveChanges();
            }
        }
        [HttpDelete]
        [Route("Delete Service")]
        public void ServiceDelete(string name)
        {           
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    db.Services.Remove(db.Services.Where(s => s.Name == name).First());
                    db.SaveChanges();
                }
                catch
                {
                    throw new Exception("Service not found");
                }
            }
        }
        [HttpPost]
        [Route("Change ping status")]
        public void PingStatusChange(string name, bool ping)
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                var service=db.Services.Where(s=>s.Name==name).FirstOrDefault();
                service.Status = ping;
                db.SaveChanges();

            }
        }
            [HttpGet]
        [Route("Ping Service")]
        public bool Ping(string name)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var service= db.Services.Where(s=>s.Name == name).FirstOrDefault();
                var result= SiteAvailability(service.Url);
                if(result)
                    return true;
                else
                {
                    return false;
                    Log log = new Log();
                    log.ServiseId=service.Id;
                    log.PingResalt = result.ToString();
                    log.PingTime = DateTime.Now.ToString();
                    db.Log.Add(log);
                    db.SaveChanges();
                }
            }
        }
        private static bool SiteAvailability(string uri)
        {
            bool available;
            try
            {
                var request = WebRequest.Create(uri);
                request.Credentials = CredentialCache.DefaultCredentials;
                var response = (HttpWebResponse)request.GetResponse();
                available = response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                available = false;
            }
            return available;
        }

    }
}
