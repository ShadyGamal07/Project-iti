using Grad_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Grad_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Data.SouqContext db = new Data.SouqContext();
            var categs = db.Categories.ToList();
            return View(categs);
        }

        public IActionResult Categories()
        {
         
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}