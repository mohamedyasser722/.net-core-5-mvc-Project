using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
    public class MvcAppG01DbContext : DbContext
    {
        public MvcAppG01DbContext(DbContextOptions<MvcAppG01DbContext> options) : base(options)
        {
        }
        // i am not going to use this method because i haved done it in the startup.cs

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{ 
        //    //base.OnConfiguring(optionsBuilder);

        //    optionsBuilder.UseSqlServer("Server=.; Database = MvcAppG01Db; Trusted_Connection = true"); // ;MultipleActiveResultSets = true : this allows multiple queries to be executed on the same connection
        //}

        public  DbSet<Department> Departments { get; set; }

    }
}
