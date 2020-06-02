using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class FallBack : Controller
    {
        // כדי שיוכל לנווט בין קומפוננטות 
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
        }
    }
}