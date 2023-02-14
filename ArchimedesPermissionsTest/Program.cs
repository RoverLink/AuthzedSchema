using ArchimedesPermissionsTest;
using Microsoft.Extensions.Configuration;
using System.Reflection;

// This is just to keep the server address and token private
var builder = new ConfigurationBuilder()
    .AddUserSecrets(typeof(Secrets).Assembly)
    .AddEnvironmentVariables();
var configurationRoot = builder.Build();

var secrets = configurationRoot.GetSection("AuthZed").Get<Secrets>();

if (secrets is null)
    throw new ArgumentException("Invalid secrets configuration");

var client = new SpiceDb.Client(secrets.ServerAddress, secrets.Token);
var schema = Assembly.GetExecutingAssembly().ReadResourceAsync("archimedes-schema-001.zed").Result;

client.ImportSchemaFromStringAsync(schema, "archtest").GetAwaiter().GetResult();

var schema_read = client.ExportSchema();

if (client.CheckPermission(ZedUser.WithId("abcd").GroupCanJoin(ZedGroup.WithId("abcd"))).HasPermission)
{

}

Console.WriteLine("Done");