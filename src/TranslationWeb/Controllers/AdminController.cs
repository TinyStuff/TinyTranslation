using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TinyTranslation;

namespace TranslationWeb.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private TinyTranslation.TranslationService _service;

        public AdminController(TinyTranslation.TranslationService service)
        {
            _service = service;
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{key}/{to}")]
        public async Task<string> Translate(string key, string to)
        {
            var r = await _service.AutoTranslate(key, to);
            return r;
        }

    }
}