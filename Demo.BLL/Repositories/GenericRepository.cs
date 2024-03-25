using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly MvcAppG01DbContext _dbContext;
		public GenericRepository(MvcAppG01DbContext dbContext) 
		{ 
			_dbContext = dbContext;
		}
		public async Task AddAsync(T entity)
		{
			await _dbContext.AddAsync(entity);
			
		}

		public void Delete(T entity)
		{
			_dbContext.Remove(entity);
			
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			if(typeof(T) == typeof(Employee))
			{
				return (IEnumerable<T>) await _dbContext.Employees.Include(E => E.Department).ToListAsync();		// this is wrong we should use a design pattern called Specification Pattern to solve this problem.
			}
			return await _dbContext.Set<T>().ToListAsync();
		}

		public async Task<T> GetByIdAsync(int Id)
		{
			return await _dbContext.Set<T>().FindAsync(Id);  // or return _dbContext.Find<T>(Id);;
		}

		public void Update(T entity)
		{
			_dbContext.Update(entity);
			
		}

		
	}
	
}
