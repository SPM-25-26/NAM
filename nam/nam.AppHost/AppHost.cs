var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlServer("sql")
                    .WithHostPort(1433)
                    .WithLifetime(ContainerLifetime.Persistent)
                    .WithDataVolume()
                    .AddDatabase("db");

var vectordb = builder.AddQdrant("vectordb")
                    .WithLifetime(ContainerLifetime.Persistent)
                    .WithDataVolume();

var server = builder.AddProject<Projects.nam_Server>("server")
            .WithReference(db)
            .WaitFor(db)
            .WithReference(vectordb)
            .WaitFor(vectordb)
            .WithHttpHealthCheck("/health");

var client = builder.AddViteApp("client", "../../nam.client")
            .WithReference(server)
            .WaitFor(server)
            .WithExternalHttpEndpoints()
            .WithNpmPackageInstallation();

var qdrantDataInjection = builder.AddProject<Projects.Datainjection_Qdrant>("datainjection-qdrant")
            .WithReference(vectordb)
            .WaitFor(vectordb)
            .WithReference(server)
            .WaitFor(server);

var sqlDataInjection = builder.AddProject<Projects.DataInjection_SQL>("datainjection-sql")
            .WithReference(db)
            .WaitFor(db);


builder.Build().Run();