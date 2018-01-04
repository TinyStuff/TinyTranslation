using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TinyTranslation;
using TinyTranslation.EFStore;
using TinyTranslation.EFStore.Data;

namespace TranslationWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class TranslationController : Controller
    {
        private TranslationService _service;
        private TranslationDbContext _ctx;

        public TranslationController(TranslationService service, TranslationDbContext ctx) {
            _service = service;
            _ctx = ctx;
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
            var ret = _service.Get(locale, System.Net.WebUtility.UrlDecode(key));
            HandleDbStorage();
            return ret;
        }

        private void HandleDbStorage()
        {
            var dbStorage = _service.GetStorage() as DbStorage;
            if (dbStorage!=null) {
                dbStorage.SaveWithContext(_ctx);
            }
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{locale}/{key}/{value}")]
        public void Update(string locale, string key, string value)
        {
            _service.Update(locale, System.Net.WebUtility.UrlDecode(key), System.Net.WebUtility.UrlDecode(value));
            HandleDbStorage();
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{key}")]
        public void Delete(string key)
        {
            _service.Delete(System.Net.WebUtility.UrlDecode(key));
        }
    }
}