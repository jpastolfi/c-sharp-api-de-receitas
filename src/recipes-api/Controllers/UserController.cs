using Microsoft.AspNetCore.Mvc;
using recipes_api.Services;
using recipes_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace recipes_api.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{    
    public readonly IUserService _service;
    
    public UserController(IUserService service)
    {
        this._service = service;        
    }

    // 6 - Sua aplicação deve ter o endpoint GET /user/:email
    [HttpGet("{email}", Name = "GetUser")]
    public IActionResult Get(string email)
    {                
        User requestedUser = _service.GetUser(email);
        return IsUserValid(email, Ok(requestedUser));
    }

    // 7 - Sua aplicação deve ter o endpoint POST /user
    [HttpPost]
    public IActionResult Create([FromBody]User user)
    {
        _service.AddUser(user);
        return CreatedAtAction("Create", user);
    }

    // "8 - Sua aplicação deve ter o endpoint PUT /user
    [HttpPut("{email}")]
    public IActionResult Update(string email, [FromBody]User user)
    {
        try {
            if (email != user.Email)
                return BadRequest();
            User requestedUser = _service.GetUser(email);
            if (!IsUserValid(email))
                return NotFound();
            _service.UpdateUser(user);
            return Ok(user);
        } catch {
            return BadRequest();
        }
    }

    // 9 - Sua aplicação deve ter o endpoint DEL /user
    [HttpDelete("{email}")]
    public IActionResult Delete(string email)
    {
        /* if (!IsUserValid(email))
            return NotFound();
        _service.DeleteUser(email); */
        User requestedUser = _service.GetUser(email);
        if (requestedUser == null)
            return NotFound();
        _service.DeleteUser(email);
        return NoContent();
    } 

    public bool IsUserValid(string email)
    {
        return _service.GetUser(email) != null;
    }
    public IActionResult IsUserValid(string email, IActionResult action)
    {
        return _service.GetUser(email) == null ? NotFound() : action;
    }
}