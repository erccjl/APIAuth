using APIAUTH.Aplication.DTOs;
using APIAUTH.Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIAUTH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : GenericController<IUserService, UserDto>
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
            : base(userService)
        {
            _userService = userService;
        }

    }
}
