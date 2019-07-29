using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MwangiBlogApp.Models
{
    public class EmailModel
    {
        [Required, Display(Name = "Name")]
        public String FromName { get; set; }

        [Required, Display(Name = "Email"), EmailAddress]
        public string FromEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required, AllowHtml]
        public string Body { get; set; }
    }
}