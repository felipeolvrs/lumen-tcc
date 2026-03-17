namespace API.Extensions;

public static class EndpointExtensions
{
    public static RouteGroupBuilder SecureEndpoints(this RouteGroupBuilder group)
    {
        group.RequireAuthorization();
        return group;
    }
}