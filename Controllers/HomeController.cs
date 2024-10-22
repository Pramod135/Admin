using Admin_Management.Data;
using Admin_Management.Data.Services;
using Admin_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualBasic;
using System.Diagnostics;
using X.PagedList;

namespace Admin_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly Admin_dbContext _context;
        FrontendService frontedServive;
        public List<TblMenu> listOfMenu { get; set; }

        public List<tbltestimonial> listOfTests { get; set; }   
      
        public HomeController(Admin_dbContext context)
        {
            _context = context;
            frontedServive = new FrontendService(context);
            listOfMenu = new List<TblMenu>();
            listOfMenu = _context.TblMenu.Where(w=>w.Active).OrderBy(x => x.OrderNo).ToList();
            listOfTests = _context.tbl_testimonial.Where(w => w.IsActive == true).Take(2).ToList(); 
        }
        public IActionResult Index()
        {
            ViewBag.menu = listOfMenu;
            var rrdata = frontedServive.GetPageByUrl("home");
            return View(rrdata);
        }
        
        public IActionResult DynamicPage()
        {
            ViewBag.menu = listOfMenu;
            var rrdata = frontedServive.GetPageByUrl("home");
            return View(rrdata);
        }

        [Route("/p/{purl}")]
        public IActionResult Index(string purl)
        {
            ViewBag.menu = listOfMenu;
            var rrdata = frontedServive.GetPageByUrl(purl);
            return View(rrdata);
        }


        //test-blog 
        [Route("/blog/{slug}")]
        public IActionResult blogg(string slug)
        {
            ViewBag.menu = listOfMenu;
            slug = "/blog/" + slug;
            var rrddata = _context.TblBlog.Include(i => i.Cat).Where(w => w.Url == slug).FirstOrDefault();
            if (rrddata != null)
            {
                return View(rrddata);
            }                    
            return View(rrddata);
        }



        [Route("/blogs")]
        public IActionResult Blog(string sortOrder, string currentFilter, string searchString, int? page, int? totalCount)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["UrlSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["TitleSortParm"] = sortOrder == "Date" ? "date_desc1" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var frontedblog = from s in _context.TblBlog
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                frontedblog = frontedblog.Where(s => s.Name.Contains(searchString)
                                      || s.Url.Contains(searchString)
                                      || s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    frontedblog = frontedblog.OrderByDescending(s => s.Name);
                    break;
                case "Url":
                    frontedblog = frontedblog.OrderBy(s => s.Url);
                    break;
                case "date_desc":
                    frontedblog = frontedblog.OrderByDescending(s => s.Url);
                    break;
                case "Title":
                    frontedblog = frontedblog.OrderBy(s => s.Title);
                    break;
                case "date_desc1":
                    frontedblog = frontedblog.OrderByDescending(s => s.Title);
                    break;
                default:
                    frontedblog = frontedblog.OrderBy(s => s.Name);
                    break;
            }
            ViewBag.menu = listOfMenu;
            ViewBag.tttttt = frontedblog;

            ViewBag.ttt = "blog";

            ViewBag.totalCount = frontedblog.Count();
            int pageSize = 4;
            int pageNumber = (page ?? 1);


            var rdata = frontedServive.TimeAgo(DateTime.Now);
            ViewBag.testdate = rdata;
            return View(frontedblog.Include(i => i.Cat).ToList().ToPagedList(pageNumber, pageSize));

            //var rrddaa = _context.TblBlog.Include(i => i.Cat);
            //if (rrddaa != null)
            //{
            //    return View(rrddaa.ToList());
            //}
            //return View(rrddaa);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [Route("/contact")]
        public IActionResult Contact(TblEnquiry tblEnquiry)
        {
            if (ModelState.IsValid)
            {
                _context.TblEnquiry.Add(tblEnquiry);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Contact));
        }


        [Route("/about")]
        public IActionResult About()
        {
            return RedirectToAction(nameof(Index), "Home", "about");
            // return Index("about");
        }


        //[Route("/doctor")]
        //public IActionResult Doctor()
        //{
        //    return Index("doctor");
        //}

        //[Route("/department")]
        //public IActionResult Department()
        //{
        //    return Index("department");
        //}




        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}