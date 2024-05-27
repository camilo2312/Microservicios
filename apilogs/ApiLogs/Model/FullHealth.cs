namespace ApiLogs.Model
{
    public class FullHealth
    {
        public string status { get; set; }
        public List<Health> checks { get; set; }

        public FullHealth()
        {
            checks = new List<Health>();
        }
    }
}
