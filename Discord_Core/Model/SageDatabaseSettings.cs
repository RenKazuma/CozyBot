namespace Discord_Core.Model
{
    public class AppSettings
    {
        public SageDatabaseSettings SageDatabase { get; set; }
    }

    public class SageDatabaseSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
