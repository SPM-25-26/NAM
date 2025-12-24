var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlServer("sql")
                    .WithLifetime(ContainerLifetime.Persistent)
                    .WithDataVolume()
                    .AddDatabase("db");
var qdrant = builder.AddQdrant("qdrant");

var server = builder.AddProject<Projects.nam_Server>("server")
            .WithReference(db)
            .WithReference(qdrant)
            .WaitFor(db)
            .WaitFor(qdrant)
            .WithHttpHealthCheck("/health");

var client = builder.AddViteApp("client", "../../nam.client")
            .WithReference(server)
            .WaitFor(server)
            .WithNpmPackageInstallation();

builder.Build().Run();
