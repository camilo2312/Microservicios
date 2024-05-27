namespace ApiHealth.Domain
{
    public class ResponseServices
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EndPoint { get; set; }
        public string Emails { get; set; }
        public object Health { get; set; }
    }
}
