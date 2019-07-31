using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MwangiBlogApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public string AuthorId { get; set; }
        [AllowHtml]
        [Display(Name = "Enter Comment")]
        public string Body { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string UpdateReason { get; set; }
        //virtual Navigation section
        public virtual BlogPost BlogPost { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}