using ColorGuesser.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Collections.Concurrent;

namespace ColorGuesser.Server.Services;

public class TemplateListServiceImpl : TemplateListService.TemplateListServiceBase
{
    private static readonly ConcurrentDictionary<string, string> _items = new();

    public override Task<Item> Add(Item request, ServerCallContext context)
    {
        _items[request.Key] = request.Value;
        return Task.FromResult(new Item { Key = request.Key, Value = request.Value });
    }

    public override Task<Empty> Remove(RemoveRequest request, ServerCallContext context)
    {
        _items.TryRemove(request.Key, out _);
        return Task.FromResult(new Empty());
    }

    public override Task<GetAllResponse> GetAll(Empty request, ServerCallContext context)
    {
        var response = new GetAllResponse();
        foreach (var kvp in _items)
        {
            response.Items.Add(new Item { Key = kvp.Key, Value = kvp.Value });
        }
        return Task.FromResult(response);
    }
}
