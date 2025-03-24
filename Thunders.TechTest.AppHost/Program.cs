var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var rabbitMqPassword = builder.AddParameter("RabbitMqPassword", true);

var rabbitMq = builder.AddRabbitMQ("rabbitmq", password: rabbitMqPassword)
    .WithExternalHttpEndpoints()
    .WithDataVolume("rabbitmqdatavolume")
    .WithManagementPlugin();

var sqlServerPassword = builder.AddParameter("SqlServerInstancePassword", true);
var sqlServerPort = int.Parse(builder.AddParameter("SqlServerInstancePort", true).Resource.Value);
var sqlServer = builder.AddSqlServer("sqlserverinstance", password: sqlServerPassword, port: sqlServerPort)
    .WithDataVolume("sqlserverinstance-volume");

var database = sqlServer.AddDatabase("ThundersTechTestDb", "ThundersTechTest");

var apiService = builder.AddProject<Projects.Thunders_TechTest_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(rabbitMq)
    .WaitFor(rabbitMq)
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
