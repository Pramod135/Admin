
using Admin_Management.Models;

namespace Admin_Management.Data.Services
{
    public class BlogService
    {
        private readonly Admin_dbContext _context;


        public BlogService(Admin_dbContext context)
        {

            _context = context;
        }

    

        public void Insert(TblBlog tblBlog)
        {
            if(tblBlog != null)
            {
                _context.TblBlog.Add(tblBlog);
                _context.SaveChanges();
            }
        }

        public TblBlog blogId(int id)
        {
            return _context.TblBlog.Where(w => w.Id == id).FirstOrDefault();
        }
        public void Update(TblBlog tblBlog)
        {
            _context.TblBlog.Update(tblBlog);
            _context.SaveChanges();
        }

        public void Delete(TblBlog tblBlog)
        {
            _context.TblBlog.Remove(tblBlog);
            _context.SaveChanges();
        }

    }
}
