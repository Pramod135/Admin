using Admin_Management.Models;

namespace Admin_Management.Data.Services
{
    public class UserService
    {
        private readonly Admin_dbContext _context;

        public UserService(Admin_dbContext context)
        {
            _context = context;
        }

        public void Delete(TblUser User)
        {
            _context.TblUser.Remove(User);
            _context.SaveChanges(); 
        }

        public IEnumerable<TblUser> GetAll()
        {
            return _context.TblUser.ToList();
        }

        public TblUser GetById(int id)
        {
            return _context.TblUser.Where(w => w.Id == id).FirstOrDefault();
        }

        public void Insert(TblUser User)
        {
            _context.TblUser.Add(User);
            _context.SaveChanges();
        }

        public void Update(TblUser User)
        {
            _context.TblUser.Update(User);
            _context.SaveChanges();
        }

        public TblUser LoginUser(TblUser tblUser)
        {
            return _context.TblUser.Where(x => x.UserId == tblUser.UserId && x.Password == tblUser.Password && x.Active == true)?.FirstOrDefault();
        }
    }
}
