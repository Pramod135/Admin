﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Admin_Management.Models
{
    public partial class TblMenu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string RawHtml { get; set; }
        public string Groups { get; set; }
        public bool Active { get; set; }
        public int? OrderNo { get; set; }
        public DateTime Regdate { get; set; }
    }
}