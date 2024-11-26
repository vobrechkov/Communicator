using Microsoft.AspNetCore.Routing;

namespace Communicator.Shared.Endpoints;

public interface IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app);
}