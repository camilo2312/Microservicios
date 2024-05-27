namespace ApiLogs.Model
{
    public class LogsDTO
    {
        public string Application { get; set; }
        public string LogType { get; set; }
        public string Module { get; set; }
        public DateTime Timestamp { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
    }
}
