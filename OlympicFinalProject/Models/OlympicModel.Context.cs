﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OlympicFinalProject.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OlympicGames3Entities : DbContext
    {
        public OlympicGames3Entities()
            : base("name=OlympicGames3Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Athlete> Athletes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Medalist> Medalists { get; set; }
        public DbSet<MedalType> MedalTypes { get; set; }
        public DbSet<OlympicEvent> OlympicEvents { get; set; }
        public DbSet<OlympicGame> OlympicGames { get; set; }
        public DbSet<SportsCategory> SportsCategories { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
