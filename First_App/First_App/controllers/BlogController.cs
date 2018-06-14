using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace First_App.controllers
{
    public class BlogController : Controller
    {
        // GET: /<controller>/
        public string Index()
        {
            return "This is the Blog";
        }
        public IActionResult Post(int id)
        {
            return new ContentResult { Content = id.ToString() };
        }
    }
}
