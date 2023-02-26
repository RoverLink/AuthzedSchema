using ArchimedesPermissionsTest;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Archimedes.Domain.Entities.Permission.Schema;
using SpiceDb;
using SpiceDb.Models;

// This is just to keep the server address and token private
var builder = new ConfigurationBuilder()
    .AddUserSecrets(typeof(Secrets).Assembly)
    .AddEnvironmentVariables();
var configurationRoot = builder.Build();

var secrets = configurationRoot.GetSection("AuthZed").Get<Secrets>();

if (secrets is null)
    throw new ArgumentException("Invalid secrets configuration");

var client = new SpiceDbClient(secrets.ServerAddress, secrets.Token, "archtest");
var schema = typeof(ZedConfig).Assembly.ReadResourceAsync("archimedes-schema-001.zed").Result;

client.DeleteRelationshipsAsync(new RelationshipFilter { Type = "group" }).Wait();
client.DeleteRelationshipsAsync(new RelationshipFilter { Type = "organization" }).Wait();
client.DeleteRelationshipsAsync(new RelationshipFilter { Type = "platform" }).Wait();

client.ImportSchemaFromStringAsync(schema).GetAwaiter().GetResult();

var schema_read = client.ReadSchema();

if (client.CheckPermission(ZedUser.WithId("abcd").GroupCanJoin(ZedGroup.WithId("abcd"))).HasPermission)
{

}

Console.WriteLine("Done");