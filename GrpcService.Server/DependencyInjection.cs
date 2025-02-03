
using GrpcService.Server.Services;

namespace GrpcService.Server;

public static class MapGrpcServicesExtension
{
    public static IEndpointRouteBuilder MapGrpcServices(this IEndpointRouteBuilder app)
    {
        app.MapGrpcService<HelloServices>();
        return app;
    }
}