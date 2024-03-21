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
	public class EmployeeRepository : GenericRepository<Employee>, IEmployeesRepository
	{
		private readonly MvcAppG01DbContext _dbContext;

		public EmployeeRepository(MvcAppG01DbContext dbContext) : base(dbContext) // ask CLR for creating an instance of DbContext
		{
			_dbContext = dbContext;
		}

		public IQueryable<Employee> GetEmployeesByAddress(string address)
		{
			return _dbContext.Employees.Where(e => e.Address == address);
			
		}

		

		

		
	}
}
