namespace Thunders.TechTest.ApiService
{
    public class Features
    {
        public required bool UseMessageBroker { get; set; }
        public required bool UseEntityFramework { get; set; }
        public required ConnectionStrings ConnectionStrings { get; set; }

        public static Features BindFromConfiguration(IConfiguration section)
        {
            var features = section.GetSection("Features").Get<Features>() ??
                throw new InvalidOperationException("No 'Features' section found");
            features.ConnectionStrings = section.GetSection("ConnectionStrings").Get<ConnectionStrings>() ??
                throw new InvalidOperationException("No 'ConnectionStrings' section found");
            return features;
        }
    }

    public class ConnectionStrings
    {
        public required string ThundersTechTestDb { get; set; }
        public required string RabbitMq { get; set; }
    }
}
