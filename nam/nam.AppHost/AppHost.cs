var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlServer("sql")
                    .WithHostPort(1433)
                    .WithLifetime(ContainerLifetime.Persistent)
                    .WithDataVolume()
                    .AddDatabase("db");

var vectordb = builder.AddQdrant("vectordb")
                    .WithLifetime(ContainerLifetime.Persistent)
                    .WithDataVolume();

var dataInjection = builder.AddProject<Projects.DataInjection>("datainjection")
            .WithReference(db)
            .WaitFor(db)
            .WithReference(vectordb)
            .WaitFor(vectordb);

//var server = builder.AddProject<Projects.nam_Server>("server")
//            .WithReference(db)
//            .WaitFor(db)
//            .WithHttpHealthCheck("/health");

//var client = builder.AddViteApp("client", "../../nam.client")
//            .WithReference(server)
//            .WaitFor(server)
//            .WithExternalHttpEndpoints()
//            .WithNpmPackageInstallation();


builder.Build().Run();