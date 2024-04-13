using EduBot.Application.Interactors.Login;
using EduBot.Application.Interactors.Register;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using EduBot.Application.Interactors.Logout;

namespace CleanArch.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private IMediator _mediator;
        public AuthController(IMediator mediator) {
            _mediator = mediator;
        }



        [HttpPost("LoginUser")]
        public async Task<ActionResult> Login([FromBody] LoginCommand userInfo) {
            var result = await _mediator.Send(userInfo);

            if (result) {
                return Ok($"User {userInfo.Email} login successfully");
            }
            else {
                ModelState.AddModelError(string.Empty, "Invalid Login attempt.");
                return BadRequest(ModelState);
            }
        }
        

        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser([FromBody] RegisterCommand userInfo) {
            var result = await _mediator.Send(userInfo);

            if (result) {
                return Ok($"User {userInfo.Email} was create successfully");
            }
            else {
                ModelState.AddModelError(string.Empty, "Failed to register user.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("LogoutUser")]
        public async Task<ActionResult> LogoutUser([FromBody] LogoutCommand userInfo)
        {
            var result = await _mediator.Send(userInfo);

            if (result)
            {
                return Ok($"User {userInfo.Email} has logout successfully");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to logout user.");
                return BadRequest(ModelState);
            }
        }
    }
}
