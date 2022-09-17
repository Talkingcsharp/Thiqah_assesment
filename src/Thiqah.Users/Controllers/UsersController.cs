using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Thiqah.Shared.Exceptions;
using Thiqah.Users.Application.Users;
using Thiqah.Users.Controllers.ViewModels;

namespace Thiqah.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersCommand _usersCommand;
        private readonly IUsersQuery _usersQuery;

        public UsersController(IUsersCommand usersCommand, IUsersQuery usersQuery)
        {
            _usersCommand = usersCommand;
            _usersQuery = usersQuery;
        }

        [HttpGet("/api/user/{id:int}")]
        [ProducesResponseType(typeof(UserViewModel),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ExceptionViewModel), 404)]
        public async Task<ActionResult<UserViewModel>> User([FromRoute] int id)
        {
            return Ok(UserViewModel.FromUser(await _usersQuery.GetUser(id)));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<UserViewModel>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<List<UserViewModel>>> Users()
        {
            var output = await _usersQuery.ListUsers();
            if(output is null || output.Count == 0)
            {
                return NoContent();
            }

            return Ok(output.Select(s => UserViewModel.FromUser(s)).ToList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(int),(int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ExceptionViewModel),400)]
        [ProducesResponseType(typeof(ExceptionViewModel),409)]
        public async Task<ActionResult<int>> Create([FromBody]CreateUserViewModel input)
        {
            var created = await _usersCommand.CreateUser(input.ToUser());

            return Created("/api/user/" + created, created);
        }
    }
}
