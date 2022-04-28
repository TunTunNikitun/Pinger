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
       public void ServiceAdd(string name, string url, bool ping)
        {
            Services serv = new Services
            {
                Name = name,
                Url = url,
                Status=ping
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
                    db.Services.Remove(db.Services.AsQueryable().Where(s => s.Name == name).First());
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
                var service=db.Services.AsQueryable().Where(s=>s.Name==name).FirstOrDefault();
                service.Status = ping;
                db.SaveChanges();
            }            
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
