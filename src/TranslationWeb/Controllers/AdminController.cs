using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TinyTranslation;
using TranslationWeb.Data.Models;
using TranslationWeb.Services;

namespace TranslationWeb.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        readonly TinyTranslation.TranslationService service;
        readonly UserManager<ApplicationUser> userManager;
        readonly SignInManager<ApplicationUser> signInManager;
        readonly TokenService token;

        public AdminController(TinyTranslation.TranslationService service, UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager, TokenService token)
        {
            this.token = token;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.service = service;
        }
#if DEBUG
        [HttpGet("adduser/{user}")]
        public async Task<IdentityResult> AddUser(string user, [FromQuery]string password)
        {
            var r = await userManager.CreateAsync(new ApplicationUser()
            {
                UserName = user
            }, password);
            return r;
        }
#endif
        [HttpGet("login/{user}")]
        public async Task<string> Login(string user, [FromQuery]string password)
        {
            var r = await signInManager.PasswordSignInAsync(user, password, true, false);
            if (r.Succeeded)
                return token.GenerateToken(user);
            Response.StatusCode = 401;
            return string.Empty;
        }

        [HttpGet("{key}/{to}")]
        public async Task<string> Translate(string key, string to)
        {
            var r = await service.AutoTranslate(key, to);
            return r;
        }

    }
}