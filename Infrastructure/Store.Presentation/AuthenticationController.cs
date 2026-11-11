
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Abstractions.Auth;
using Store.Shared.DTOS.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    public class AuthenticationController(IAuthenticationService _authenticationService) : ApiBaseController
    {
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login, CancellationToken ct)
        {
            var result = await _authenticationService.LoginAsync(login, ct);
            return ToActionResult(result);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register, CancellationToken ct)
        {
            var result = await _authenticationService.RegisterAsync(register, ct);
            return ToActionResult(result);
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await _authenticationService.CheckEmailExistsAsync(email));
        }

        [Authorize]
        [HttpGet("currentuser")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDTO>> GetCurrnetUser(CancellationToken ct)
        {
            return ToActionResult(await _authenticationService.GetCurrnetUserAsync(GetEmailFromToken(), ct));
        }

        [Authorize]
        [HttpGet("address")]
        [ProducesResponseType(typeof(AddressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AddressDTO>> GetCurrentUserAddress(CancellationToken ct)
        {
            return ToActionResult(await _authenticationService.GetCurrentUserAddressAsync(GetEmailFromToken(), ct));
        }

        [Authorize]
        [HttpPut("address")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO addressDTO, CancellationToken ct)
        {
            return ToActionResult(await _authenticationService.UpSertUserAddressAsync(GetEmailFromToken(), addressDTO, ct));
        }
    }
}
