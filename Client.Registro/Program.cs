using Grpc.Net.Client;
using Grpc.Core;
using GrpcService.Protos;

static string Ask(string label)
{
    Console.Write(label);
    return Console.ReadLine()?.Trim() ?? "";
}

var server = Environment.GetEnvironmentVariable("GRPC_SERVER") ?? "http://127.0.0.1:5080";
using var channel = GrpcChannel.ForAddress(server);

var client = new RegistroService.RegistroServiceClient(channel);

Console.WriteLine($"[Client.Registro] Server: {server}");

while (true)
{
    Console.WriteLine("\n1) CreateVoter  2) GetVoter  0) Exit");
    var opt = Ask("Option: ");

    if (opt == "0") break;

    try
    {
        if (opt == "1")
        {
            var name = Ask("Name: ");
            var doc = Ask("Document: ");

            var reply = await client.CreateVoterAsync(new CreateVoterRequest
            {
                Name = name,
                Document = doc
            });

            Console.WriteLine($"OK voterId={reply.VoterId}");
            Console.WriteLine(reply.Message);
        }
        else if (opt == "2")
        {
            var voterId = Ask("VoterId: ");

            var reply = await client.GetVoterAsync(new GetVoterRequest
            {
                VoterId = voterId
            });

            Console.WriteLine($"voterId={reply.VoterId}");
            Console.WriteLine($"name={reply.Name}");
            Console.WriteLine($"document={reply.Document}");
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }
    catch (RpcException ex)
    {
        Console.WriteLine($"RPC Error: {ex.Status.StatusCode} - {ex.Status.Detail}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
