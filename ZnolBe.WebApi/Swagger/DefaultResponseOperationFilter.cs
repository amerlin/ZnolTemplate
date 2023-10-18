using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ZnolBe.WebApi.Swagger;

#pragma warning disable CS1591 // Manca il commento XML per il tipo o il membro visibile pubblicamente
public class DefaultResponseOperationFilter : IOperationFilter
#pragma warning restore CS1591 // Manca il commento XML per il tipo o il membro visibile pubblicamente
{
#pragma warning disable CS1591 // Manca il commento XML per il tipo o il membro visibile pubblicamente
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
#pragma warning restore CS1591 // Manca il commento XML per il tipo o il membro visibile pubblicamente
        => operation.Responses.TryAdd("default", GetResponse("Unexpected error"));

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
