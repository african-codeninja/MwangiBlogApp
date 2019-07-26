using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MwangiBlogApp.Helpers;
using MwangiBlogApp.Models;
using PagedList;
using PagedList.Mvc;

namespace MwangiBlogApp.Controllers
{
    //[Authorize(Roles = "Admin")]

    //[Authorize(Roles = "Admin, Moderator")]
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public object Posts { get; private set; }

        [AllowAnonymous]
        // GET: BlogPosts
        public ActionResult Index(int? page)
        {
                int pageSize = 2;
                int pageNumber = page ?? 1;

            var publishedPosts = db.BlogPosts.OrderBy(b => b.Published).OrderByDescending(b => b.Created).Take(2).ToList();

            return View(publishedPosts.ToPagedList(pageSize, pageNumber));
         
        }
        // GET: BlogPosts/Details/5
        [AllowAnonymous]

        public ActionResult Details(string Slug)
        {
            if (String.IsNullOrWhiteSpace(Slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.FirstOrDefault(p => p.Slug == Slug);

            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,MediaUrl,Published")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                /*instance of class StringUtilities which only has a function  that takes the
                 argument of title from the blogpost instance and passes that value to var Slug
                 variable("which is not yet set
                */
                var Slug = StringUtilities.URLFriendly(blogPost.Title);


                //this line checks if the string is NULL or no value was entered in the input box
                //and throws a custome error
                if (string.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPost);
                }

                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPost.MediaUrl = "/Uploads/" + fileName;
                }

                //I go out to the database and checkes to see if there are any other slugs that looks
                //identical basically checks for uniqueness in the database
                //and throw out a custom error
                if (db.BlogPosts.Any(p => p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogPost);
                }
                //set the value to the variable you declared in 
                //var Slug = StringUtilities.URLFriendly(blogPost.Title);
                blogPost.Slug = Slug;
                blogPost.Created = DateTimeOffset.Now;
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Abstract,slug,Body,MediaUrl,Published,Created,Updated")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                var newSlug = StringUtilities.URLFriendly(blogPost.Title);

               if (db.BlogPosts.Any(p => p.Slug != newSlug))
                    {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPost);
                }
                
                blogPost.Slug = newSlug;
                blogPost.Updated = DateTimeOffset.Now;
                //db.Entry(blogPost).State = DateTimeOffset.Now;
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("edit");
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
