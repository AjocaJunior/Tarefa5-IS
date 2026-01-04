using Grpc.Core;
using GrpcService.Protos;

namespace GrpcService.Services;

public class RegistroGrpcService : RegistroService.RegistroServiceBase
{
    private static readonly Dictionary<string, (string Name, string Document)> _voters = new();

    public override Task<CreateVoterReply> CreateVoter(CreateVoterRequest request, ServerCallContext context)
    {
        var voterId = Guid.NewGuid().ToString("N");
        _voters[voterId] = (request.Name, request.Document);

        return Task.FromResult(new CreateVoterReply
        {
            VoterId = voterId,
            Message = $"Voter created: {request.Name}"
        });
    }

    public override Task<GetVoterReply> GetVoter(GetVoterRequest request, ServerCallContext context)
    {
        if (_voters.TryGetValue(request.VoterId, out var data))
        {
            return Task.FromResult(new GetVoterReply
            {
                VoterId = request.VoterId,
                Name = data.Name,
                Document = data.Document
            });
        }

        throw new RpcException(new Status(StatusCode.NotFound, "Voter not found"));
    }
}
