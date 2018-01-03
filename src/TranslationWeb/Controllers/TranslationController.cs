using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TinyTranslation;

namespace TranslationWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class TranslationController : Controller
    {
        private TranslationService _service;

        public TranslationController(TranslationService service) {
            _service = service;
        } 
        
        [HttpGet()]
        public TranslationTable GetLanguages()
        {
            return _service.GetTableData();
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{locale}")]
        public IDictionary<string, string> Get(string locale)
        {
            return _service.GetTranslations(locale);
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{locale}/{key}")]
        public string Get(string locale, string key)
        {
            return _service.Get(locale, System.Net.WebUtility.UrlDecode(key));
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{locale}/{key}/{value}")]
        public void Update(string locale, string key, string value)
        {
            _service.Update(locale, System.Net.WebUtility.UrlDecode(key), System.Net.WebUtility.UrlDecode(value));
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{key}")]
        public void Delete(string key)
        {
            _service.Delete(System.Net.WebUtility.UrlDecode(key));
        }
    }
}