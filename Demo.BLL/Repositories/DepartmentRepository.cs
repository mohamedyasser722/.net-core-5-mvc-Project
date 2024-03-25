using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department> ,IDepartmentRepository
    {
        private readonly MvcAppG01DbContext _dbContext;
        public DepartmentRepository(MvcAppG01DbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            //dbcontext = new MvcAppG01DbContext();  // connect to database, but every time we create a new instance of DepartmentRepository, we create a new connection to the database and this is not good for performance
            // we will use dependency injection to inject the dbcontext into the DepartmentRepository
            // clr will help us to inject the dbcontext into the DepartmentRepository by using the constructor 

        }
       
    }
}
