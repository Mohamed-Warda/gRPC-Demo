using Grpc.ClientFactory;
using GrpcService;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using WebApi.Client.Dtos;

namespace WebApi.Client.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        private readonly GrpcClient<HelloService.HelloServiceClient> _client;

        public HelloController(GrpcClient<HelloService.HelloServiceClient> client)
        {
            _client = client;
        }

        // Unary call example
        [HttpPost("unary")]
        public async Task<IActionResult> Create(HelloDto dto)
        {
            var request = new Request()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var response = await _client.CallUnaryMethod<Request, Response>(
                (c, r, o) => c.UnaryServiceAsync(r, o),
                request
            );

            return Ok(response);
        }

        // Client streaming call example
        [HttpPost("client-streaming")]
        public async Task<IActionResult> ClientStreaming(IEnumerable<HelloDto> dtos)
        {
            var requests = new List<Request>();
            foreach (var dto in dtos)
            {
                requests.Add(new Request
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                });
            }

            var response = await _client.CallClientStreamingMethod<Request, Response>(
                (c, o) => c.ClientStreamingService(o),
                requests
            );

            return Ok(response);
        }

        // Server streaming call example
        [HttpPost("server-streaming")]
        public async Task<IActionResult> ServerStreaming(HelloDto dto)
        {
            var request = new Request()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var responses = new List<Response>();

            await _client.CallServerStreamingMethod<Request, Response>(
                (c, r, o) => c.ServerStreamingService(r, o),
                request,
                async (response) =>
                {
                    responses.Add(response);
                    await Task.CompletedTask; 
                }
            );

            return Ok(responses);
        }

        // Bidirectional streaming call example
        [HttpPost("bidirectional-streaming")]
        public async Task<IActionResult> BidirectionalStreaming(IEnumerable<HelloDto> dtos)
        {
            var requests = new List<Request>();
            foreach (var dto in dtos)
            {
                requests.Add(new Request
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                });
            }

            var responses = new List<Response>();

            await _client.CallBidirectionalStreamingMethod<Request, Response>(
                (c, o) => c.BidirectionalStreamingService(o),
                async (call) =>
                {
                    // Write requests to the stream
                    foreach (var request in requests)
                    {
                        await call.RequestStream.WriteAsync(request);
                    }
                    await call.RequestStream.CompleteAsync();

                    // Read responses from the stream
                    await foreach (var response in call.ResponseStream.ReadAllAsync())
                    {
                        responses.Add(response);
                    }
                }
            );

            return Ok(responses);
        }
    }
}