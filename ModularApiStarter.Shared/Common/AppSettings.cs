namespace ModularApiStarter.Shared.Common
{
    public class AppSettings
    {
        public required ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public required string SqlServer { get; set; }
    }
}