using MwangiBlogApp.Models;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MwangiBlogApp.Controllers
{
    [Authorize]
    //[RequireHttps]
    //[Authorize(Roles = "Admin, Moderator")]
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }


        //[Authorize(Roles = " Admin, Moderator")]

        public ActionResult Index()
        {
            return View();

        }



        private object SearchUtility(/*object p*/)
        {
            throw new NotImplementedException();
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