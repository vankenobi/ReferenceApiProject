namespace Reference.Api.Extensions
{
    public class ConsulConfig
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required int Port { get; set; }
        public required string [] Tags { get; set; }
        public required Check Check { get; set; }

    }

    public class Check
    {
        public required string Http { get; set; }
        public required string Interval { get; set; }
        public required string Timeout { get; set; }
    }
}