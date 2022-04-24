namespace Pinger.Models
{
    public class Log
    {
        public int Id { get; set; }
        public int ServiseId { get; set; }
        public Services Service { get; set; }
        public string PingTime { get; set; }
        public string PingResalt { get; set; }
    }
}
