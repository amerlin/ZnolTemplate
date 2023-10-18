using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ZnolBe.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ControllerBase: Microsoft.AspNetCore.Mvc.ControllerBase
{

}
