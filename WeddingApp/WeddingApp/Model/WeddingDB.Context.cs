﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeddingApp.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WeddingDBEntities : DbContext
    {
        public WeddingDBEntities()
            : base("name=WeddingDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BALLROOM> BALLROOMs { get; set; }
        public virtual DbSet<BALLROOMTYPE> BALLROOMTYPEs { get; set; }
        public virtual DbSet<DISH> DISHES { get; set; }
        public virtual DbSet<DISHTYPE> DISHTYPEs { get; set; }
        public virtual DbSet<EMPLOYEE> EMPLOYEES { get; set; }
        public virtual DbSet<FUNCTION> FUNCTIONS { get; set; }
        public virtual DbSet<INVOICE> INVOICES { get; set; }
        public virtual DbSet<MENU> MENUs { get; set; }
        public virtual DbSet<PARAMETER> PARAMETERs { get; set; }
        public virtual DbSet<PERMISSION> PERMISSIONs { get; set; }
        public virtual DbSet<REPORTDETAIL> REPORTDETAILs { get; set; }
        public virtual DbSet<ROLE> ROLES { get; set; }
        public virtual DbSet<SALESREPORT> SALESREPORTs { get; set; }
        public virtual DbSet<SERVE> SERVEs { get; set; }
        public virtual DbSet<SERVICE> SERVICEs { get; set; }
        public virtual DbSet<SHIFT> SHIFTS { get; set; }
        public virtual DbSet<WEDDING> WEDDINGs { get; set; }
    }
}
