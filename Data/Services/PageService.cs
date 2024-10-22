using Admin_Management.Models;

namespace Admin_Management.Data.Services
{
    public class PageService
    {
        private readonly Admin_dbContext _context;

        public PageService(Admin_dbContext context)
        {
            _context = context;
        }

        public void Delete(TblPage page)
        {
            _context.TblPage.Remove(page);
            _context.SaveChanges();
        }

        public IEnumerable<TblPage> GetAll()
        {
            return _context.TblPage.ToList();
        }

        public TblPage GetById(int id)
        {
            return _context.TblPage.Where(w => w.Id == id).FirstOrDefault();
        }

        public void Insert(TblPage page)
        {
            _context.TblPage.Add(page);
            _context.SaveChanges();
        }

        public void Update(TblPage page)
        {
            _context.TblPage.Update(page);
         
            _context.SaveChanges();
        }


    }
}
