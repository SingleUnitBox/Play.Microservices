using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Queries;
using Play.Common.Controllers;
using Play.Items.Application.Commands;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;

namespace Play.Items.Api.Controllers;

public class ElementsController(
    IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ElementDto>>> GetElemets()
        => Ok(await queryDispatcher.QueryAsync(new GetElements()));
}