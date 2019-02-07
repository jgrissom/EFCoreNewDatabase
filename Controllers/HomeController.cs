using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreNewDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EFCoreNewDatabase.Controllers
{
    public class HomeController : Controller
    {
        // this controller depends on the BloggingRepository
        private IBloggingRepository repository;
        public HomeController(IBloggingRepository repo) => repository = repo;

        public IActionResult Index() => View(repository.Blogs.OrderBy(b => b.Url));
        [Authorize(Roles = "Moderate")]
        public IActionResult AddBlog() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderate")]
        public IActionResult AddBlog(Blog model)
        {
            if (ModelState.IsValid)
            {
                if (repository.Blogs.Any(b => b.Url == model.Url))
                {
                    ModelState.AddModelError("", "Url must be unique");
                }
                else
                {
                    repository.AddBlog(model);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        [Authorize(Roles = "Moderate")]
        public IActionResult DeleteBlog(int id)
        {
            repository.DeleteBlog(repository.Blogs.FirstOrDefault(b => b.BlogId == id));
            return RedirectToAction("Index");
        }

        public IActionResult BlogDetail(int id) => View(new PostViewModel { 
                blog = repository.Blogs.FirstOrDefault(b => b.BlogId == id), 
                Posts = repository.Posts.Where(p => p.BlogId == id) });

        public IActionResult AddPost(int id)
        {
            ViewBag.BlogId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPost(int id, Post post)
        {
            post.BlogId = id;
            if (ModelState.IsValid)
            {
                repository.AddPost(post);
                return RedirectToAction("BlogDetail", new { id = id });
            }
            @ViewBag.BlogId = id;
            return View();
        }

        public IActionResult DeletePost(int id)
        {
            Post post = repository.Posts.FirstOrDefault(p => p.PostId == id);
            int BlogId = post.BlogId;
            repository.DeletePost(post);
            return RedirectToAction("BlogDetail", new { id = BlogId });
        }
    }
}
