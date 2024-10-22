using Admin_Management.Models;

namespace Admin_Management.Data.Services
{
    public class TestimonialService
    {
        private readonly Admin_dbContext _context;

        public TestimonialService(Admin_dbContext context)
        {
            _context = context;
        }

        public void Insert(tbltestimonial tbltestimonial)
        {
            if(tbltestimonial != null)
            {
                _context.tbl_testimonial.Add(tbltestimonial);
                _context.SaveChanges();
            }
        }
        public tbltestimonial testId(int id)
        {
            return _context.tbl_testimonial.Where(t=>t.Id == id).FirstOrDefault();  
        }


        public void Update(tbltestimonial tbltestimonial)
        {
            if(tbltestimonial != null)
            {
                _context.tbl_testimonial.Update(tbltestimonial);    
                _context.SaveChanges(); 
            }
        }

        public void Delete(tbltestimonial tbltestimonial)
        {
            if (tbltestimonial != null)
            {
                _context.tbl_testimonial.Remove(tbltestimonial);
                _context.SaveChanges();
            }
        }
    }
}
