using Grpc.Core;
using GrpcService.Protos;

namespace GrpcService.Services;

public class VotacaoGrpcService : VotacaoService.VotacaoServiceBase
{
    private static readonly HashSet<string> _startedElections = new();
    private static readonly Dictionary<string, Dictionary<string, int>> _votes = new();

    public override Task<StartElectionReply> StartElection(StartElectionRequest request, ServerCallContext context)
    {
        _startedElections.Add(request.ElectionId);
        if (!_votes.ContainsKey(request.ElectionId))
            _votes[request.ElectionId] = new Dictionary<string, int>();

        return Task.FromResult(new StartElectionReply
        {
            Started = true,
            Message = $"Election started: {request.ElectionId}"
        });
    }

    public override Task<CastVoteReply> CastVote(CastVoteRequest request, ServerCallContext context)
    {
        if (!_startedElections.Contains(request.ElectionId))
            throw new RpcException(new Status(StatusCode.FailedPrecondition, "Election not started"));

        var map = _votes[request.ElectionId];
        map.TryGetValue(request.Candidate, out var current);
        map[request.Candidate] = current + 1;

        return Task.FromResult(new CastVoteReply
        {
            Accepted = true,
            Message = $"Vote accepted for {request.Candidate}"
        });
    }

    public override Task<GetResultsReply> GetResults(GetResultsRequest request, ServerCallContext context)
    {
        _votes.TryGetValue(request.ElectionId, out var map);
        map ??= new Dictionary<string, int>();

        var reply = new GetResultsReply { ElectionId = request.ElectionId };
        reply.Results.AddRange(map.Select(kv => new ResultItem { Candidate = kv.Key, Votes = kv.Value }));

        return Task.FromResult(reply);
    }
}
