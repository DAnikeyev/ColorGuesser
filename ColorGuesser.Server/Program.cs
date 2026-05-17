using ColorGuesser.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

// Note: In production, restrict AllowAnyOrigin to specific trusted origins.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
    });
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.MapGrpcService<TemplateListServiceImpl>().EnableGrpcWeb().RequireCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod()
          .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding"));

app.MapFallbackToFile("index.html");

app.Run();
