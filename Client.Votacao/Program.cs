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

var client = new VotacaoService.VotacaoServiceClient(channel);

Console.WriteLine($"[Client.Votacao] Server: {server}");

while (true)
{
    Console.WriteLine("\n1) StartElection  2) CastVote  3) GetResults  0) Exit");
    var opt = Ask("Option: ");

    if (opt == "0") break;

    try
    {
        if (opt == "1")
        {
            var electionId = Ask("ElectionId: ");

            var reply = await client.StartElectionAsync(new StartElectionRequest
            {
                ElectionId = electionId
            });

            Console.WriteLine($"started={reply.Started}");
            Console.WriteLine(reply.Message);
        }
        else if (opt == "2")
        {
            var electionId = Ask("ElectionId: ");
            var voterId = Ask("VoterId: ");
            var candidate = Ask("Candidate: ");

            var reply = await client.CastVoteAsync(new CastVoteRequest
            {
                ElectionId = electionId,
                VoterId = voterId,
                Candidate = candidate
            });

            Console.WriteLine($"accepted={reply.Accepted}");
            Console.WriteLine(reply.Message);
        }
        else if (opt == "3")
        {
            var electionId = Ask("ElectionId: ");

            var reply = await client.GetResultsAsync(new GetResultsRequest
            {
                ElectionId = electionId
            });

            Console.WriteLine($"Election: {reply.ElectionId}");
            if (reply.Results.Count == 0)
            {
                Console.WriteLine("(no votes yet)");
            }
            else
            {
                foreach (var r in reply.Results)
                {
                    Console.WriteLine($"{r.Candidate}: {r.Votes}");
                }
            }
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
