using Grad_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Grad_Project.Data;

namespace Grad_Project.Controllers
{
    public class HomeController : Controller
    {
        ProjectItiContext db = new ProjectItiContext();
        public IActionResult categories()
        {
            var cat=db.categories.ToList();
            return View(cat);
        }
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
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