using Admin_Management.Data;
using Admin_Management.Data.Services;
using Admin_Management.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Drawing.Printing;
using System.Net.NetworkInformation;
using X.PagedList;

namespace Admin_Management.Controllers
{
    public class AdminController : Controller
    {
        private readonly Admin_dbContext _context;
        private readonly IWebHostEnvironment _environment;

        PageService pageServive;
        BlogService blogService;
        UserService userService;
        MediaService uploadservice;
        MenuService menuService;
        TestimonialService testimonialService;

        TblUser uModel = new TblUser();

        public AdminController(Admin_dbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            pageServive = new PageService(context);
            blogService = new BlogService(context);
            userService = new UserService(context);
            menuService = new MenuService(context);
            testimonialService = new TestimonialService(context);   
            uploadservice = new MediaService(environment);
        }

        public IActionResult Page()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Admin", new { area = "" });
            }

            var rrddaa = pageServive.GetAll();
            GetSessionValue();
            return View(rrddaa);

        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Admin", new { area = "" });
            }
            GetSessionValue();
            return View();
        }

        [Route("/admin/manage-page")]
        public IActionResult ManagePage()
        {
            GetSessionValue();
            return View();
        }
        [HttpPost]
        [Route("/admin/manage-page")]
        public IActionResult ManagePage(TblPage tblPage)
        {
            tblPage.RegDate=DateTime.Now;   
            tblPage.IsActive = true;
            pageServive.Insert(tblPage);
            GetSessionValue();
            return RedirectToAction(nameof(AllPages));
        }


        //true or false
        [HttpPost]
        [Route("/Admin/UpdatePageStatus")]
        public IActionResult UpdatePageStatus(int id, bool pagestatus)
        {
            var updatestatus = _context.TblPage.Where(w => w.Id == id).FirstOrDefault();
            if (updatestatus != null)
            {
                updatestatus.IsActive = !pagestatus;
                _context.TblPage.Update(updatestatus);
            }
            _context.SaveChanges();
            return RedirectToAction("AllPages");
        }


        [Route("/admin/all-pages")]
        public IActionResult AllPages(string sortOrder, string currentFilter, string searchString, int? page, int? totalCount)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["UrlSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var admin = from s in _context.TblPage
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                admin = admin.Where(s => s.Name.Contains(searchString)
                                      || s.Url.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    admin = admin.OrderByDescending(s => s.Name);
                    break;
                case "Url":
                    admin = admin.OrderBy(s => s.Url);
                    break;
                case "date_desc":
                    admin = admin.OrderByDescending(s => s.Url);
                    break;
                default:
                    admin = admin.OrderBy(s => s.Name);
                    break;
            }
            ViewBag.totalCount = admin.Count();
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            GetSessionValue();
            return View(admin.ToList().ToPagedList(pageNumber, pageSize));

            //GetSessionValue();
            //return View(_context.TblPage.ToList());//RedirectToAction("Index");
        }


        [HttpPost]
        [Route("/admin/search-module")]
        public IActionResult SearcModule(string Module, string SearchString)
        {
            if (Module == "Page")
            {
                return Redirect("/admin/all-pages?SearchString=" + SearchString);
            }
            if (Module == "Blog")
            {
                return Redirect("/admin/manage-blog?SearchString=" + SearchString);
            }
            if (Module == "Enquiry")
            {
                return Redirect("/admin/manage-enquery?SearchString=" + SearchString);
            }
            else
            {
                return RedirectToAction("/admin");
            }
        }

        public IActionResult EditPage(int id)
        {
            var tblPage = pageServive.GetById(id);

            GetSessionValue();
            return View(tblPage);
        }
        [HttpPost]
        public IActionResult EditPage(TblPage tblPage)
        {
            tblPage.IsActive = true;
            tblPage.RegDate = DateTime.Now;

            pageServive.Update(tblPage);

            GetSessionValue();
            return RedirectToAction(nameof(AllPages));
        }
        //public IActionResult DeletePage(int id)
        //{
        //    var tblPage = pageServive.GetById(id);
        //    GetSessionValue();
        //    return View(tblPage);
        //}
        [HttpPost]
        public IActionResult DeletePage(TblPage tblPage)
        {
            pageServive.Delete(tblPage);
            GetSessionValue();
            return RedirectToAction(nameof(AllPages));
        }



        //blog section//
        [Route("/admin/manage-blog")]
        public IActionResult ManageBlog(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["UrlSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var blog = from s in _context.TblBlog
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                blog = blog.Where(s => s.Name.Contains(searchString)
                                      || s.Cat.CatName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    blog = blog.OrderByDescending(s => s.Name);
                    break;
                case "Url":
                    blog = blog.OrderBy(s => s.Cat.CatName);
                    break;
                case "date_desc":
                    blog = blog.OrderByDescending(s => s.Cat.CatName);
                    break;
                default:
                    blog = blog.OrderBy(s => s.Name);
                    break;
            }
            ViewBag.totalCount = blog.Count();
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            GetSessionValue();
            return View(blog.Include(i => i.Cat).ToList().ToPagedList(pageNumber, pageSize));

            //var rData = _context.TblBlog.Include(i=>i.Cat);
            //if (rData != null)
            //{
            //    return View(rData.ToList());
            //}
            //return View();     
        }



        //IsActive true or false
        [HttpPost]
        [Route("/Admin/UpdateBlogStatus")]
        public IActionResult UpdateBlogStatus(int id, bool blogstatus)
        {
            var updatestatus = _context.TblBlog.Where(w => w.Id == id).FirstOrDefault();
            if (updatestatus != null)
            {
                updatestatus.IsActive = !blogstatus;
                _context.TblBlog.Update(updatestatus);
            }
            _context.SaveChanges();
            return RedirectToAction("ManageBlog");

        }

        public IActionResult AddBlog()
        {
            var catlist = _context.TblCategory.ToList();

            ViewBag.cattbl = new SelectList(catlist, "Id", "CatName");

            GetSessionValue();
            return View();
        }


        [HttpPost]
        [Route("/admin/addblog")]
        public IActionResult AddBlog(TblBlog tblBlog, IFormFile? FileData = null)
        {
            tblBlog.IsActive = true;
            tblBlog.RegDate = DateTime.Now;
            if (FileData != null)
            {
                string folder = "/Content/Image/";
                folder += FileData.FileName;
                var uploadPath = _environment.WebRootPath + folder;
                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    FileData.CopyTo(fileStream);
                }
                tblBlog.ImagePath = folder;
            }
            blogService.Insert(tblBlog);
            GetSessionValue();
            return RedirectToAction(nameof(ManageBlog));
        }

        public IActionResult EditBlog(int id)
        {
            var tblBlog = blogService.blogId(id);

            //dropdowm bind data//
            var catlist = _context.TblCategory.ToList();
            ViewBag.cattbl = new SelectList(catlist, "Id", "CatName", id.ToString());
            ViewBag.DefaultBrand = id.ToString();

            GetSessionValue();
            return View(tblBlog);
        }

        [HttpPost]
        public IActionResult EditBlog(TblBlog tblBlog, IFormFile? FileData = null)
        {
            GetSessionValue();
            if (FileData != null)
            {
                tblBlog.ImagePath = uploadservice.UploadSingle(FileData, "/upload/user/");
            }

            tblBlog.RegDate = DateTime.Now;
            tblBlog.IsActive = true;
            blogService.Update(tblBlog);
            GetSessionValue();
            return RedirectToAction(nameof(ManageBlog));
        }


        //public IActionResult DeleteBlog(int id)
        //{
        //    var tblBlog = blogService.blogId(id);
        //    GetSessionValue();
        //    return View(tblBlog);
        //}

        [HttpPost]
        public IActionResult DeleteBlog(TblBlog tblBlog)
        {
            blogService.Delete(tblBlog);
            GetSessionValue();
            return RedirectToAction(nameof(ManageBlog));
        }

        [Route("/admin/manage-enquery")]
        public IActionResult ContactEnquery(string sortOrder, string currentFilter, string searchString, int? page, int? totalCount)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["UrlSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var enquiry = from s in _context.TblEnquiry
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                enquiry = enquiry.Where(s => s.Name.Contains(searchString)
                                      || s.Mobile.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    enquiry = enquiry.OrderByDescending(s => s.Name);
                    break;
                case "Url":
                    enquiry = enquiry.OrderBy(s => s.Mobile);
                    break;
                case "date_desc":
                    enquiry = enquiry.OrderByDescending(s => s.Mobile);
                    break;
                default:
                    enquiry = enquiry.OrderBy(s => s.Name);
                    break;
            }


            ViewBag.totalCount = enquiry.Count();
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            GetSessionValue();
            return View(enquiry.ToList().ToPagedList(pageNumber, pageSize));



            //var rrddaa = _context.TblEnquiry.ToList();
            //GetSessionValue();
            //return View(rrddaa);
        }

        //public IActionResult DeleteEnquiry(int? id)
        //{
        //    var tblenquiry = _context.TblEnquiry
        //       .FirstOrDefault(m => m.Id == id);
        //    if (tblenquiry == null)
        //    {
        //        return NotFound();
        //    }
        //    GetSessionValue();
        //    return View(tblenquiry);
        //}


        [HttpPost]
        [Route("/admin/delete-enquiry")]
        public IActionResult DeleteEnquiry(int id)
        {
            var tblenquiry = _context.TblEnquiry.Find(id);
            if (tblenquiry != null)
            {
                _context.TblEnquiry.Remove(tblenquiry);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(ContactEnquery));
        }

        [Route("/admin/manage-category")]
        [Route("/admin/manage-category/{Id}")]
        public IActionResult ManageCotegory(int Id)
        {
           
            if (Id > 0)
            {
                //id record data one row
                ViewBag.catmodel = _context.TblCategory.Where(w => w.Id == Id).FirstOrDefault();
            }
            GetSessionValue();
            return View(_context.TblCategory.ToList());
        }

        [HttpPost]
        [Route("/admin/manage-category")]
        public IActionResult ManageCotegory(TblCategory tblCategory)
        {
            tblCategory.RegDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (tblCategory.Id > 0)
                {
                    tblCategory.RegDate = DateTime.Now;
                    _context.TblCategory.Update(tblCategory);
                }
                else
                {
                    _context.TblCategory.Add(tblCategory);
                }
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(ManageCotegory));
        }

        //[Route("/admin/delete-category")]
        //public IActionResult DeleteCategory(int? id)
        //{
        //    if (id == null || _context.TblCategory == null)
        //    {
        //        return NotFound();
        //    }
        //    var tblcat = _context.TblCategory
        //        .FirstOrDefault(m => m.Id == id);
        //    if (tblcat == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(tblcat);
        //}

        [HttpPost]
        [Route("/admin/delete-category")]
        public IActionResult DeleteCategory(int id)
        {

            var tblcat = _context.TblCategory.Find(id);
            if (tblcat != null)
            {
                _context.TblCategory.Remove(tblcat);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(ManageCotegory));
        }

        //user//
        [Route("/admin/all-userpage")]
        public IActionResult AllUserPage()
        {
            return View(_context.TblUser.ToList());
        }

        [Route("/admin/user-page")]
        public IActionResult UserPage()
        {
            return View();
        }

        [HttpPost]
        [Route("/admin/user-page")]
        public IActionResult UserPage(TblUser tblUser, IFormFile? FileData = null)
        {
            if (FileData != null)
            {
                tblUser.ImagePath = uploadservice.UploadSingle(FileData, "/upload/user/");
            }

            tblUser.RegDate = DateTime.Now;
            tblUser.Active = true;
            if (tblUser != null)
            {
                userService.Insert(tblUser);
            }
            GetSessionValue();
            return RedirectToAction(nameof(AllUserPage));
        }

        [Route("/admin/profile")]
        public IActionResult EditUser()
        {
            GetSessionValue();

            var tblUser = userService.GetById(uModel.Id);
            return View(tblUser);
        }

        [HttpPost]
        [Route("/admin/profile")]
        public IActionResult EditUser(TblUser tblUser, IFormFile? FileData = null)
        {
            GetSessionValue();

            if (FileData != null)
            {
                tblUser.ImagePath = uploadservice.UploadSingle(FileData, "/upload/user/");
            }
            tblUser.Active = true;
            tblUser.RegDate = DateTime.Now;
            userService.Update(tblUser);

            return RedirectToAction(nameof(EditUser));
        }

        [Route("/admin/testimonial-list")]
        public IActionResult TestmonialList()
        {
            var rdata = _context.tbl_testimonial.ToList();
            return View(rdata); 
        }
        
        [Route("/admin/create-testimonial")]
        public IActionResult CreateTestmonial()
        {
           return View();   
        }

        [HttpPost]
        [Route("/admin/create-testimonial")]
        public IActionResult CreateTestmonial(tbltestimonial tbltestimonial, IFormFile? FileData = null)
        {
            if (FileData != null)
            {
                tbltestimonial.Image = uploadservice.UploadSingle(FileData, "/upload/user/");
            }

            if (tbltestimonial != null)
            {
                testimonialService.Insert(tbltestimonial);  
            }
            return RedirectToAction(nameof(TestmonialList));
        }


        [Route("/admin/edit-testimonial")]
        public IActionResult EditTestmonial(int id)
        {
          var rdata= testimonialService.testId(id);
            return View(rdata);
        }

        [HttpPost]
        [Route("/admin/edit-testimonial")]
        public IActionResult EditTestmonial(tbltestimonial tbltestimonial)
        {
            if (tbltestimonial != null)
            {
                testimonialService.Update(tbltestimonial);
            }
            return RedirectToAction(nameof(TestmonialList));
        }

        [HttpPost]
        public IActionResult DeleteTestimonial(tbltestimonial tbltestimonial)
        {
            if (tbltestimonial != null)
            {
                testimonialService.Delete(tbltestimonial);
            }
            return RedirectToAction("TestmonialList");
        }  

        [Route("/admin/change-Password")]
        public IActionResult ChangePassword()
        {
            GetSessionValue();
            return View();
        }

        [HttpPost]
        [Route("/admin/change-Password")]
        public IActionResult ChangePassword(string oldpass, string newpass, string conpass)
        {
            if (conpass == null || newpass == null || conpass == null)
            {
                makemsg("All fields are required.");
                return RedirectToAction(nameof(ChangePassword));
            }
            if (conpass != newpass)
            {
                makemsg("confirm pass does not match");

                return RedirectToAction(nameof(ChangePassword));
            }

            GetSessionValue();

            if (uModel.Password != oldpass)
            {
                return RedirectToAction(nameof(ChangePassword));
            }
            uModel.Password = newpass;
            userService.Update(uModel);
            return RedirectToAction(nameof(ChangePassword));
        }


        [HttpPost]
        [Route("/admin/create-subscribe")]
        public IActionResult CreateSubscribe(TblSubscribe tblSubscribe)
        {
            if (ModelState.IsValid)
            {
                if(tblSubscribe.Email==null)
                {

                }

                Tools.GetNameFromEmail(tblSubscribe.Email);
                tblSubscribe.Name = Tools.GetNameFromEmail(tblSubscribe.Email);

                tblSubscribe.Active = true;
                tblSubscribe.Regdate = DateTime.Now;
                _context.Add(tblSubscribe);
                _context.SaveChanges();
                return RedirectToAction(nameof(Subscribe));
            }

            return RedirectToAction(nameof(Subscribe));
        }



        [Route("/admin/media-manager")]
        public IActionResult MediaManager()
        {

            List<string> listOfMedia = new List<string>();
            GetSessionValue();
            listOfMedia = uploadservice.GetMediaFiles("/upload/media/");

            return View(listOfMedia);
        }

        [HttpPost]
        [Route("/admin/media-manager")]
        public IActionResult MediaManager(IFormFile[] FilesData)
        {
            if (FilesData != null)
            {
                foreach (var FileData in FilesData)
                {
                    uploadservice.UploadSingle(FileData, "/upload/media/");
                }
            }
            return RedirectToAction("MediaManager");
        }

        [Route("/admin/menu-list")]
        public IActionResult MenuList()
        {
            GetSessionValue();
            var rrdata = _context.TblMenu.ToList();
            return View(rrdata);
        }

        public IActionResult Menu()
        {
            GetSessionValue();
            return View();
        }
        [HttpPost]
        [Route("/admin/menu")]
        public IActionResult Menu(TblMenu tblMenu)
        {
            tblMenu.Regdate = DateTime.Now; 
            tblMenu.Active=true;    
            menuService.CreateMunu(tblMenu);      
            return RedirectToAction(nameof(MenuList));
        }

        [Route("/admin/update-menu")]
        public IActionResult UpdateMenu(int id)
        {
            GetSessionValue();

            var tblmenu = menuService.menuId(id);
            return View(tblmenu);
        }


        [HttpPost]
        [Route("/admin/update-menu")]
        public IActionResult UpdateMenu(TblMenu tblMenu)
        {
            GetSessionValue();
            tblMenu.Active = true;
            tblMenu.Regdate = DateTime.Now;
            menuService.Update(tblMenu);
            return RedirectToAction(nameof(MenuList));

        }

    

        //public IActionResult DeleteMenu(int id)
        //{
        //    GetSessionValue();
        //    var tblmenu = menuService.menuId(id);
        //    GetSessionValue();
        //    return View(tblmenu);
        //}

        [HttpPost]
        public IActionResult DeleteMenu(TblMenu tblMenu)
        {
            menuService.Delete(tblMenu);
            GetSessionValue();
            return RedirectToAction(nameof(MenuList));
        }


        //IsActive true or false
        [HttpPost]
        [Route("/Admin/UpdateMenuStatus")]
        public IActionResult UpdateMenuStatus(int id, bool menustatus)
        {
            GetSessionValue();
            var updatestatus = _context.TblMenu.Where(w => w.Id == id).FirstOrDefault();
            if (updatestatus != null)
            {
                updatestatus.Active = menustatus;
                _context.TblMenu.Update(updatestatus);
            }
            _context.SaveChanges();
            return RedirectToAction("MenuList");

        }

  
        [Route("/admin/subscribe")]
        public IActionResult Subscribe()
        {
            GetSessionValue();
            var rdata = _context.TblSubscribe.ToList();
            return View(rdata);  
        }



        [Route("/admin/create-subscribe")]
        public IActionResult CreateSubscribe()
        {
            GetSessionValue();
            return View();
        }

      






        [HttpPost]
        [Route("/admin/delete-subscriber")]
        public IActionResult DeleteSubscriber(int id)
        {
            var tblsub = _context.TblSubscribe.Find(id);
            if (tblsub != null)
            {
                _context.TblSubscribe.Remove(tblsub);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Subscribe));
        }

        [HttpPost]
        [Route("/Admin/UpdateSubscribeStatus")]
        public IActionResult UpdateSubscribeStatus(int id, bool substatus)
        {
            
            var updatestatus = _context.TblSubscribe.Where(w => w.Id == id).FirstOrDefault();
            if (updatestatus != null)
            {
                updatestatus.Active = substatus;
                _context.TblSubscribe.Update(updatestatus);
            }
         
            _context.SaveChanges();
            return RedirectToAction(nameof(Subscribe));
        }

        public IActionResult Login()
        {
            ShowMessage();
            return View();
        }

        [HttpPost]
        [Route("/admin/login")]
        public IActionResult Login(TblUser tblUser)
        {           
            if (tblUser.UserId == null)
            {
                makemsg("userid required*");
                return RedirectToAction("Login");
            }
            if (tblUser.Password == null)
            {
                makemsg("password required*");
                return RedirectToAction("Login");
            }

            TblUser userdata = userService.LoginUser(tblUser);

            if (userdata == null)
            {
                makemsg("UserName and password is invalid*");
                return RedirectToAction("Login");
            }

            HttpContext.Session.SetString("UserId", userdata.UserId);
            HttpContext.Session.SetInt32("Id", userdata.Id);
            HttpContext.Session.SetString("Email", userdata.Email);
            HttpContext.Session.SetString("ImagePath", userdata.ImagePath);       

            ViewBag.userId = userdata.UserId;
            ViewBag.id = userdata.Id;
            ViewBag.email = userdata.Email;
            ViewBag.imagePath = userdata.ImagePath;
            return RedirectToAction(nameof(Index)); 
        }
       
        //error message show in login //
        void makemsg(string msg)
        {
            HttpContext.Session.SetString("msg", msg);
        }
        void ShowMessage()
        {
            ViewBag.msg = HttpContext.Session.GetString("msg");
            HttpContext.Session.Remove("msg");
        }
        void GetSessionValue()
        {
            ShowMessage();
            int Id = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            //ViewBag.msg = HttpContext.Session.GetString("msg");
            //HttpContext.Session.Remove("msg");

            if (Id > 0)
            {
                uModel = _context.TblUser.Where(w => w.Id == Id).AsNoTracking().FirstOrDefault();
                if (uModel != null)
                {
                    ViewBag.UserId = uModel.UserId;
                    ViewBag.Id = uModel.Id;
                    ViewBag.Email = uModel.Email;
                    ViewBag.imagePath = uModel.ImagePath;
                }
                else
                {
                    RedirectToAction("Login");                 
                }
            }
            else
            {
                RedirectToAction("Login");
            }
        }

        //public IActionResult ReadCookies()
        //{
        //    ViewBag.Username = Request.Cookies["Username"].ToString();
        //    return View("Login");
        //}
      

        [Route("/admin/logOut")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult DashBoard()
        {
            return View();
        }
    }
}









//if (userdata.UserId == tblUser.UserId && userdata.Password == tblUser.Password)
//{
//    List<Claim> claims = new List<Claim>();
//    {                    
//        new Claim(ClaimTypes.NameIdentifier, loginModel.Username);
//        Response.Cookies.Append("Username", loginModel.Username);
//        ViewBag.saved = "cookies saved";

//        new Claim("OtherProperties", "Example Role");
//    }
//    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

//    AuthenticationProperties properties = new AuthenticationProperties()
//    {
//        AllowRefresh = true,
//        //IsPersistent = loginModel.KeepLogin
//    };
//    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

//    return RedirectToAction("Index", "Home");

//    // return View();
//}