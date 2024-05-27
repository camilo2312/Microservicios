namespace ApiGateway.Domain
{
    public class Health
    {
        public string status { get; set; }
        public string version { get; set; }
        public TimeSpan uptime { get; set; }

        public Health()
        {
            version = "1.0";
        }
    }
}
