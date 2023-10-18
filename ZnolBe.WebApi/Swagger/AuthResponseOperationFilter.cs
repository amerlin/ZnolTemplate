using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Smash.WebApi.Swagger;

#pragma warning disable CS1591 // Manca il commento XML per il tipo o il membro visibile pubblicamente
public class AuthResponseOperationFilter : IOperationFilter
#pragma warning restore CS1591 // Manca il commento XML per il tipo o il membro visibile pubblicamente
{
    private readonly IAuthorizationPolicyProvider authorizationPolicyProvider;

#pragma warning disable CS1591 // Manca il commento XML per il tipo o il membro visibile pubblicamente
    public AuthResponseOperationFilter(IAuthorizationPolicyProvider authorizationPolicyProvider)
#pragma warning restore CS1591 // Manca il commento XML per il tipo o il membro visibile pubblicamente
    {
        this.authorizationPolicyProvider = authorizationPolicyProvider;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fallbackPolicy = authorizationPolicyProvider.GetFallbackPolicyAsync().GetAwaiter().GetResult();

        var requireAutheticatedUser = fallbackPolicy?.Requirements
            .Any(r => r is DenyAnonymousAuthorizationRequirement) ?? false;

        var requiredAuthorization = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .Any(a => a is AuthorizeAttribute) ?? false;

        var allowAnonymous = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .Any(a => a is AllowAnonymousAttribute) ?? false;

        if ((requireAutheticatedUser || requiredAuthorization) && !allowAnonymous)
        {
            operation.Responses.TryAdd(StatusCodes.Status401Unauthorized.ToString(), GetResponse(HttpStatusCode.Unauthorized.ToString()));
            operation.Responses.TryAdd(StatusCodes.Status403Forbidden.ToString(), GetResponse(HttpStatusCode.Forbidden.ToString()));
        }
    }

    private static OpenApiResponse GetResponse(string description)
    => new()
    {
        Description = description,
        Content = new Dictionary<string, OpenApiMediaType>
        {
            [MediaTypeNames.Application.Json] = new()
            {
                Schema = new()
                {
                    Reference = new()
                    {
                        Id = nameof(ProblemDetails),
                        Type = ReferenceType.Schema
                    }
                }
            }
        }
    };
}

