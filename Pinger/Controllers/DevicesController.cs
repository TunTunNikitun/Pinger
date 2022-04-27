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
    public class DevicesController : ControllerBase
    {
        [HttpPost]
        [Route("Add device")]
        public void AddDevice(string deviceId, string deviceName)
        {
            var device = new AndroidDevice
            {
                DeviceId = deviceId,
                Name = deviceName
            };
            using(ApplicationContext db = new ApplicationContext())
            {
                db.AndroidDevices.Add(device);
                db.SaveChanges();
            }
        }

        [HttpPost]
        [Route("Delete device")]
        public void DeleteDevice(string deviceName)
        {            
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    db.AndroidDevices.Remove(db.AndroidDevices.AsQueryable().Where(s => s.Name == deviceName).First());
                    db.SaveChanges();
                }
                catch
                {
                    throw new Exception("Device not found");
                }
            }
        }

        [HttpGet]
        [Route("Device list")]
        public List<AndroidDevice> DeviceList()
        {
            using (var db = new ApplicationContext())
            {
                var resalt = db.AndroidDevices.ToList();
                return resalt;
            }
        }
    }
}
