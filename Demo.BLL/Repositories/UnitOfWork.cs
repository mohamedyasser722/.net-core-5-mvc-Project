using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{

		public IDepartmentRepository DepartmentRepository { get; }

		public IEmployeesRepository EmployeesRepository { get; }
		private readonly MvcAppG01DbContext _dbContext;

		public UnitOfWork(MvcAppG01DbContext dbcontext)	// ASK CLR to create an instance of DbContext
        {
			EmployeesRepository = new EmployeeRepository(dbcontext);
			DepartmentRepository = new DepartmentRepository(dbcontext);
			_dbContext = dbcontext;
        }

		public async Task<int> CompleteAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}
	}
}
