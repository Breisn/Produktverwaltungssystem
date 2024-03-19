using Microsoft.AspNetCore.Mvc;
using ProductManagementSystem.Models;
using ProductManagementSystem.ViewModels;

namespace ProductManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Account model)
        {
            if (!ModelState.IsValid)
            {
                // Rückgabe aller Validierungsfehler
                return BadRequest(ModelState);
            }

            var result = await _accountService.RegisterNewUser(model);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Rückgabe aller Validierungsfehler
                return BadRequest(ModelState);
            }

            var result = await _accountService.VerifyUserLogin(model.LoginCredential, model.Passwort);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
