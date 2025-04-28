using MasterPiece.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasterPiece.Controllers
{
    public class BlogController : Controller
    {
        private readonly MyDbContext _context;

        public BlogController(MyDbContext context)
        {
            _context = context;
        }


        public IActionResult Blog()
        {
            var blogs = _context.Blogs.ToList(); 
            return View(blogs); 
        }

        public IActionResult BlogDetails()
        {
            return View();
        }




    }
}
