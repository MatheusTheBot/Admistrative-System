﻿using Domain.Commands.Packages;
using Domain.Entities;
using Domain.Handlers;
using Domain.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("v1/packages")]
[Authorize("Admin")]
public class PackagesController : ControllerBase
{
    private readonly IRepository<Packages> Repo;
    private readonly PackagesHandler Handler;

    public PackagesController(IRepository<Packages> repo, PackagesHandler handler)
    {
        Repo = repo;
        Handler = handler;
    }

    //Queries
    [HttpGet("get/{Id}")]
    public IActionResult GetById([FromRoute] Guid Id)
    {
        var result = Repo.GetById(Id);

        if (result == null)
            return NotFound(new ControllerResult<ControllerBase>(false, "object not found"));

        return Ok(new ControllerResult<ControllerBase>(true, result));
    }

    //Commands
    [HttpPut("update")]
    public IActionResult UpdatePackages([FromBody] UpdatePackageCommand comm)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ControllerResult<ControllerBase>(false, "Invalid command"));

        var result = Handler.Handle(comm);

        if (result.IsSuccess == false)
            return StatusCode(500, new HandlerResult(result.IsSuccess, result.Data));
        return Ok(new HandlerResult(true, result.Data));
    }

    [HttpPut("changeType")]
    public IActionResult ChangeType([FromBody] ChangePackageTypeCommand comm)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ControllerResult<ControllerBase>(false, "Invalid command"));

        var result = Handler.Handle(comm);

        if (result.IsSuccess == false)
            return StatusCode(500, new HandlerResult(result.IsSuccess, result.Data));
        return Ok(new HandlerResult(true, result.Data));
    }
}
