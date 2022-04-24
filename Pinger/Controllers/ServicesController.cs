using Microsoft.AspNetCore.Mvc;
using Pinger.Models;

namespace Pinger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        [HttpPost]
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
    }
}
