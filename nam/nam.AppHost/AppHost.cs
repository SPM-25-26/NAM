var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlServer("sql")
                    .WithHostPort(1433)
                    .WithLifetime(ContainerLifetime.Persistent)
                    .WithDataVolume()
                    .AddDatabase("db");
var qdrant = builder.AddQdrant("qdrant")
                    .WithLifetime(ContainerLifetime.Persistent)
                    .WithDataVolume();

var server = builder.AddProject<Projects.nam_Server>("server")
            .WithReference(db)
            .WithReference(qdrant)
            .WaitFor(db)
            .WaitFor(qdrant)
            .WithHttpHealthCheck("/health");

var client = builder.AddViteApp("client", "../../nam.client")
            .WithReference(server)
            .WaitFor(server)
            .WithExternalHttpEndpoints()
            .WithNpmPackageInstallation();

builder.Build().Run();