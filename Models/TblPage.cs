﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Admin_Management.Models
{
    public partial class TblPage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string About { get; set; }
        public string PageContent { get; set; }
        public DateTime RegDate { get; set; }
        public bool IsActive { get; set; }
    }
}