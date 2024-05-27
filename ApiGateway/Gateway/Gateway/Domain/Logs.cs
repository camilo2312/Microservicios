namespace Gateway.Domain
{
    public class Logs
    {
        public string Application { get; set; }
        public string LogType { get; set; }
        public string Module { get; set; }
        public DateTime Timestamp { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
    }
}
