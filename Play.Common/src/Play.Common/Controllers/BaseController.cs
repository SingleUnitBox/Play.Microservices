using Microsoft.AspNetCore.Mvc;

namespace Play.Common.Controllers;

[ApiController]
[Route("/[controller]")]
public class BaseController : ControllerBase
{
    public ActionResult<TModel> OkOrNotFound<TModel>(TModel model)
    {
        if (model is null)
        {
            return NotFound();
        }
        
        return Ok(model);
    }
}