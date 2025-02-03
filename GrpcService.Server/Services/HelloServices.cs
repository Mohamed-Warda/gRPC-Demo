using Grpc.Core;
using System.Threading.Tasks;

namespace GrpcService.Server.Services
{
    public class HelloServices : HelloService.HelloServiceBase
    {
        // Unary call implementation
        public override Task<Response> UnaryService(Request request, ServerCallContext context)
        {
            var response = new Response()
            {
                Message = $"Hello {request.FirstName} {request.LastName}"
            };
            return Task.FromResult(response);
        }

        // Client streaming implementation
        public override async Task<Response> ClientStreamingService(IAsyncStreamReader<Request> requestStream, ServerCallContext context)
        {
            string fullName = "";

            // Read all requests from the client stream
            await foreach (var request in requestStream.ReadAllAsync())
            {
                fullName += $"{request.FirstName} {request.LastName}, ";
            }

            // Remove the trailing comma and space
            if (fullName.Length > 0)
            {
                fullName = fullName.Substring(0, fullName.Length - 2);
            }

            var response = new Response()
            {
                Message = $"Hello {fullName}"
            };

            return response;
        }

        // Server streaming implementation
        public override async Task ServerStreamingService(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            // Send multiple responses to the client
            for (int i = 1; i <= 5; i++)
            {
                var response = new Response()
                {
                    Message = $"Hello {request.FirstName} {request.LastName} - Response {i}"
                };

                await responseStream.WriteAsync(response);

                // Simulate some delay
                await Task.Delay(1000);
            }
        }

        // Bidirectional streaming implementation
        public override async Task BidirectionalStreamingService(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            // Read requests from the client stream
            await foreach (var request in requestStream.ReadAllAsync())
            {
                // Send a response for each request
                var response = new Response()
                {
                    Message = $"Hello {request.FirstName} {request.LastName}"
                };

                await responseStream.WriteAsync(response);
            }
        }
    }
}