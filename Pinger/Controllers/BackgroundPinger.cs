﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebApplication3.Models;


namespace WebApplication3.Controllers
{
    //public interface ICustomServiceStopper
    //{
    //    Task StopAsync(CancellationToken token = default);
    //}
    public static class BackgroundPinger/* : BackgroundService, ICustomServiceStopper*/
    {
        public static void Ping()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
               var servicesToPing = db.Services.Where(s=> s.Status==true).ToList();
                foreach (var service in servicesToPing)
                {
                    if(!SiteAvailability(service.Url))
                    {
                        Log log = new Log();
                        log.ServiseId = service.Id;
                        log.PingResalt = "false";
                        log.PingTime = DateTime.Now.ToString();
                        db.Log.Add(log);
                        db.SaveChanges();
                    }
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
        //private readonly ILogger<BackgroundPinger> _logger;
        //private Timer _timer;
        //public BackgroundPinger(ILogger<BackgroundPinger> logger)
        //{
        //    this._logger = logger;
        //}
        //public void Dispose()
        //{
        //    _timer?.Dispose();
        //}
        //public Task StartAsync(CancellationToken cancellationToken)
        //{
        //    _timer=new Timer(o=>_logger.LogInformation("ping"),null,TimeSpan.Zero,TimeSpan.FromSeconds(5));
        //    return Task.CompletedTask;
        //}
        //Task ICustomServiceStopper.StopAsync(CancellationToken token) => base.StopAsync(token);

        ////public Task StopAsync(CancellationToken cancellationToken)
        ////{
        ////    return Task.CompletedTask;
        ////}

        //protected override Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    _timer = new Timer(o => _logger.LogInformation("ping"), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        //    return Task.CompletedTask;
        //    //throw new NotImplementedException();
        //}
    }
}
