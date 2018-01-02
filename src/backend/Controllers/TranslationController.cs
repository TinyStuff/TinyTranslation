using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    public class TranslationController : Controller
    {
        [HttpGet()]
        public TranslationTable GetLanguages()
        {
            return Helper.TranslationsService.GetTableData();
        }

        [HttpGet("{locale}")]
        public IDictionary<string, string> Get(string locale)
        {
            return Helper.TranslationsService.GetTranslations(locale);
        }

        [HttpGet("{locale}/{key}")]
        public string Get(string locale, string key)
        {
            return Helper.TranslationsService.Get(locale, System.Net.WebUtility.UrlDecode(key));
        }

        [HttpPut("{locale}/{key}/{value}")]
        public void Update(string locale, string key, string value)
        {
            Helper.TranslationsService.Update(locale, System.Net.WebUtility.UrlDecode(key), System.Net.WebUtility.UrlDecode(value));
        }

        [HttpDelete("{key}")]
        public void Delete(string key)
        {
            Helper.TranslationsService.Delete(System.Net.WebUtility.UrlDecode(key));
        }
    }
}