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
        public DepartmentRepository(MvcAppG01DbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            //dbcontext = new MvcAppG01DbContext();  // connect to database, but every time we create a new instance of DepartmentRepository, we create a new connection to the database and this is not good for performance
            // we will use dependency injection to inject the dbcontext into the DepartmentRepository
            // clr will help us to inject the dbcontext into the DepartmentRepository by using the constructor 

        }
        public int Add(Department department)
        {
            _dbContext.Add(department);

            return _dbContext.SaveChanges(); ;
            
        }

        public int Delete(Department department)
        {
            _dbContext.Remove(department);
            return _dbContext.SaveChanges();
        }

       

        public IEnumerable<Department> GetAll()
        {
            return _dbContext.Departments.ToList();
        }

        public Department GetById(int departmentId)
        {
           return _dbContext.Departments.Find(departmentId);    // check if it is already in the cach then
        }

        public int Update(Department department)
        {
            _dbContext.Update(department);
            return _dbContext.SaveChanges();
        }
    }
}
