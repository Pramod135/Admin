using Admin_Management.Models;

namespace Admin_Management.Data.Services
{
    public class MenuService
    {
        private readonly Admin_dbContext _context;

        public MenuService(Admin_dbContext context)
        {
            _context = context;
        }

        public void CreateMunu(TblMenu tblMenu)
        {
            if (tblMenu != null)
            {
                _context.TblMenu.Add(tblMenu);
                _context.SaveChanges();
            } 
        }

        public TblMenu menuId(int id)
        {
            return _context.TblMenu.Where(w => w.Id == id).FirstOrDefault();
        }

        public void Update(TblMenu tblMenu)
        {
            _context.TblMenu.Update(tblMenu);
            _context.SaveChanges();
        }

        public void Delete(TblMenu tblMenu)
        {
            _context.TblMenu.Remove(tblMenu);
            _context.SaveChanges();
        }
    }
}
