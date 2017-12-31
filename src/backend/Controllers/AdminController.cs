using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        [HttpGet("{key}/{to}")]
        public async Task<string> Translate(string key, string to)
        {
            var r = await Helper.TranslationsService.AutoTranslate(key, to);
            return r;
        }

        [HttpGet("{key}")]
        public string Test(string key)
        {
            return "sklep";
        }
    }
}