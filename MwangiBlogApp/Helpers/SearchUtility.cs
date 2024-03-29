﻿using MwangiBlogApp.Models;
using System.Linq;

namespace MwangiBlogApp
{
    public class SearchUtility
    {


        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Produces a serch result when given on a user interface
        /// By Author:
        /// </summary>

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

    }
}