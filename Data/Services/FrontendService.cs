using Admin_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin_Management.Data.Services
{
    public class FrontendService
    {
        private readonly Admin_dbContext _context;
        public FrontendService(Admin_dbContext context)
        {

            _context = context;
        }

        public TblPage GetPageByUrl(string purl)
        {
            return _context.TblPage.Where(w => w.Url == purl)?.FirstOrDefault();
        }

        public TblBlog GetBlogByUrl(string burl)
        {
            return _context.TblBlog.Where(w => w.Url == burl)?.FirstOrDefault();
        }

        public string TimeAgo(DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago..", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago..", timeSpan.Minutes) :
                    "about a minute ago..";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago..", timeSpan.Hours) :
                    "about an hour ago..";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago..", timeSpan.Days) :
                    "yesterday..";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago..", timeSpan.Days / 30) :
                    "about a month ago..";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago..", timeSpan.Days / 365) :
                    "about a year ago..";
            }

            return result;
        }


    }
}
