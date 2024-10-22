namespace Admin_Management.Models
{
    public  class tbltestimonial
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Profession { get; set; }

        public string? Image { get; set; }

        public string? Comments { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? RegDate { get; set; }

    }
}
