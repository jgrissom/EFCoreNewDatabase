﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreNewDatabase.Models;
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
        public IActionResult AddBlog() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public IActionResult DeleteBlog(int id)
        {
            repository.DeleteBlog(repository.Blogs.FirstOrDefault(b => b.BlogId == id));
            return RedirectToAction("Index");
        }

        public IActionResult BlogDetail(int id) => View(new PostViewModel { 
                blog = repository.Blogs.FirstOrDefault(b => b.BlogId == id), 
                Posts = repository.Posts.Where(p => p.BlogId == id) });

    }
}
