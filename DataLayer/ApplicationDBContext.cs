using ExamOn.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(string connectionString)
               : base()
        {
            _connectionString = connectionString;
        }

        private string _connectionString;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public virtual DbSet<tblactivityHistory> tblactivityHistories { get; set; }
        public virtual DbSet<tblAnswer> tblAnswers { get; set; }
        public virtual DbSet<tblBatch> tblBatches { get; set; }
        public virtual DbSet<tblClass> tblClasses { get; set; }
        public virtual DbSet<tblEmailCrediential> tblEmailCredientials { get; set; }
        public virtual DbSet<tblexam> tblexams { get; set; }
        public virtual DbSet<tblexamLive> tblexamLives { get; set; }
        public virtual DbSet<tblexamQuestion> tblexamQuestions { get; set; }
        public virtual DbSet<tblexamSection> tblexamSections { get; set; }
        public virtual DbSet<tblexamStudent> tblexamStudents { get; set; }
        public virtual DbSet<tbllogin> tbllogins { get; set; }
        public virtual DbSet<tblloginType> tblloginTypes { get; set; }
        public virtual DbSet<tblQuestion> tblQuestions { get; set; }
        public virtual DbSet<tblSection> tblSections { get; set; }
        public virtual DbSet<tblshift> tblshifts { get; set; }
        public virtual DbSet<tbltenant> tbltenants { get; set; }
        public virtual DbSet<tbluserProfile> tbluserProfiles { get; set; }

        public virtual DbSet<tblForgotPasswordMailCounter> TblForgotPasswordMailCounters { get; set; }

    }
}