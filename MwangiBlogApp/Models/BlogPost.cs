using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MwangiBlogApp.Models
{
    public class BlogPost
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(30),MinLength(2)]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [MaxLength(50),MinLength(2)]
        [Display(Name = "Shortened Blog Title")]
        public string Abstract { get; set; }
        public string Slug { get; set; }
        [AllowHtml]
        [Required]
        [MaxLength(400),MinLength(2)]
        public string Body { get; set; }
        public string MediaUrl { get; set; }
        public bool Published { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        //virtual Nav section
        public virtual ICollection<Comment> Comments { get; set; }

        public BlogPost()
        {
            this.Comments = new HashSet<Comment>();
        }
    }
}