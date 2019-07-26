using MwangiBlogApp.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MwangiBlogApp.Controllers
{
    public class HomeController : Controller

    {
        private ApplicationDbContext context = new ApplicationDbContext();
        //Allows this controller to access the database 
        private ApplicationDbContext db = new ApplicationDbContext();




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>{0}</bold>({1})</p><p>Message:</p><p>{2}</p>";

                    var from = model.FromEmail + "<" + WebConfigurationManager.AppSettings["emailto"] + ">";

                    //model.Body = "This is a message from your Blog site. The name and email of the contacting person is above";

                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = "Blog Contact Email",

                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),

                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return View(new EmailModel());
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }


        //[Authorize(Roles =" Admin, Moderator")]
        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr;

            var blogList = IndexSearch(searchStr);

            int pageSize = 2;
            int pageNumber = page ?? 1;


            //var publishedPosts = db.BlogPosts.OrderBy(b => b.Published)
            //                                 .OrderByDescending(b => b.Created)
            //                                 .Take(5)
            //                                 .ToList();

            return View(blogList.ToPagedList(pageNumber, pageSize));

            

        }

        public IQueryable<BlogPost> IndexSearch(string searchStr)

        {
            IQueryable<BlogPost> result = null;
            if (searchStr != null)
            {
                result = db.BlogPosts.AsQueryable();
                result = result.Where(p => p.Title.Contains(searchStr)
                || p.Body.Contains(searchStr)
                || p.Comments.Any(c => c.Body.Contains(searchStr)
                || c.Author.FirstName.Contains(searchStr)
                || c.Author.LastName.Contains(searchStr)
                || c.Author.DisplayName.Contains(searchStr)
                || c.Author.Email.Contains(searchStr)));
            }
            else
            {
                result = db.BlogPosts.AsQueryable();
            }
            return result.OrderByDescending(p => p.Created);
        }

        private object Take(int v)
        {
            throw new NotImplementedException();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Mbutha application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            return View(model);
        }
    }
}