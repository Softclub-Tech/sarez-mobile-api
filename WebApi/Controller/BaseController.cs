using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[Route("[controller]"), Authorize]
public class BaseController : ControllerBase
{
    [NonAction]
    protected List<string> ModelStateErrors() =>
        ModelState.SelectMany(e => e.Value!.Errors.Select(er => er.ErrorMessage)).ToList();
}