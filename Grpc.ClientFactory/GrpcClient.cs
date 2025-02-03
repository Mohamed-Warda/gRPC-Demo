using Grpc.Core;

namespace Grpc.ClientFactory
{
    public class GrpcClient<TClient> where TClient : ClientBase<TClient>
    {
        private readonly TClient _client;

        public GrpcClient(TClient client)
        {
            _client = client;
        }

        // Unary call
        public async Task<TResponse> CallUnaryMethod<TRequest, TResponse>(
            Func<TClient, TRequest, CallOptions, AsyncUnaryCall<TResponse>> method,
            TRequest request,
            CallOptions options = default)
        {
            return await method(_client, request, options).ResponseAsync;
        }

        // Client streaming call
        public async Task<TResponse> CallClientStreamingMethod<TRequest, TResponse>(
            Func<TClient, CallOptions, AsyncClientStreamingCall<TRequest, TResponse>> method,
            IEnumerable<TRequest> requests,
            CallOptions options = default)
        {
            var call = method(_client, options);

            foreach (var request in requests)
            {
                await call.RequestStream.WriteAsync(request);
            }

            await call.RequestStream.CompleteAsync();
            return await call.ResponseAsync;
        }

        // Server streaming call
        public async Task CallServerStreamingMethod<TRequest, TResponse>(
            Func<TClient, TRequest, CallOptions, AsyncServerStreamingCall<TResponse>> method,
            TRequest request,
            Func<TResponse, Task> onMessage,
            CallOptions options = default)
        {
            var call = method(_client, request, options);

            await foreach (var response in call.ResponseStream.ReadAllAsync())
            {
                await onMessage(response);
            }
        }

        // Bidirectional streaming call
        public async Task CallBidirectionalStreamingMethod<TRequest, TResponse>(
            Func<TClient, CallOptions, AsyncDuplexStreamingCall<TRequest, TResponse>> method,
            Func<AsyncDuplexStreamingCall<TRequest, TResponse>, Task> onStream,
            CallOptions options = default)
        {
            var call = method(_client, options);
            await onStream(call);
        }
    
         
    }
}