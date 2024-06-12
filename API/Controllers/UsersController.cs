using API.Data;
using API.Entities;
using API.Interfaces;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper) 
    {
       _userRepository = userRepository;
       _mapper = mapper;
    }

    // [AllowAnonymous] // a directive to specify that a particular action/controller does not require authentication.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    // ActionResult is a base class that can represent various HTTP responses, 
    // such as 200 OK, 400 Bad Request, etc.
    {
        var users = await _userRepository.GetMembersAsync();
        // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
        return Ok(users);
    }

    [HttpGet("{username}")]// /api/users/lisa
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        return await _userRepository.GetMemberAsync(username);
        // var userToReturn = _mapper.Map<MemberDto>(user);
        // return Ok(userToReturn);
        // don't have to write this anymore because we handled it inside UserRepository already 
    }

}