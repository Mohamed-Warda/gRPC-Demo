using GrpcService;
using Microsoft.Extensions.DependencyInjection;

namespace Grpc.ClientFactory;

public static class DependencyInjection
{
    public static IServiceCollection AddGrpcService(this IServiceCollection services)
    {
        services.AddGrpcClient<HelloService.HelloServiceClient>(o =>
        {
            o.Address = new Uri("http://localhost:5011");
        });
        services.AddTransient(typeof(GrpcClient<>), typeof(GrpcClient<>));
        return services ;
    }
  
}