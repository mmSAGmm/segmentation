var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Segmentation_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Segmentation_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.Durak_Web>("durak-web");

builder.Build().Run();
