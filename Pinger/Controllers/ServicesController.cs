using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApplication3.Models;
using Hangfire;
using System.Collections.Generic;

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
        public void Ping(string name)
        {
            RecurringJob.AddOrUpdate(() => BackgroundPinger.Ping(), Cron.Minutely);
            
            //using (ApplicationContext db = new ApplicationContext())
            //{
            //    var service= db.Services.Where(s=>s.Name == name).FirstOrDefault();
            //    var result= SiteAvailability(service.Url);
            //    if(result)
            //        return true;
            //    else
            //    {
            //        return false;
            //        Log log = new Log();
            //        log.ServiseId=service.Id;
            //        log.PingResalt = result.ToString();
            //        log.PingTime = DateTime.Now.ToString();
            //        db.Log.Add(log);
            //        db.SaveChanges();
            //    }
            //}
        }
        [HttpGet]
        [Route("Services list")]
        public List<Services> ServicesList()
        {
            using(var db = new ApplicationContext())
            {
                var resalt = db.Services.ToList();
                return resalt;
            }
        }
        [HttpGet]
        [Route("Log")]
        public List<Log> ShowLog()
        {
            using (var db = new ApplicationContext())
            {
                var resalt = db.Log.ToList();
                return resalt;
            }
        }
    }
}
