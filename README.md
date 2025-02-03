# gRPC Demo with .NET and gRPC Client Factory

## Overview
This project demonstrates the four types of gRPC communication using .NET and a custom gRPC Client Factory implementation. It provides an implementation of:

1. **Unary RPC** - A single request and response.
2. **Server Streaming RPC** - A single request and multiple streamed responses.
3. **Client Streaming RPC** - Multiple streamed requests and a single response.
4. **Bidirectional Streaming RPC** - Both client and server stream messages.

## Technologies Used
- .NET 8
- gRPC
- Custom gRPC Client Factory

## Project Structure

```
gRPC-Demo/
│── GrpcService.Server/    # gRPC Server Implementation
│── Grpc.ClientFactory/    # gRPC Client Custom Client Abstraction
│── WebApi.Client/         # gRPC Client Implementation
│── Protos/                # .proto files for defining gRPC services
│── README.md              # Documentation
```
