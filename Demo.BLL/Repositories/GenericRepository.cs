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
		public void Add(T entity)
		{
			_dbContext.Add(entity);
			
		}

		public void Delete(T entity)
		{
			_dbContext.Remove(entity);
			
		}

		public IEnumerable<T> GetAll()
		{
			if(typeof(T) == typeof(Employee))
			{
				return (IEnumerable<T>) _dbContext.Employees.Include(E => E.Department).ToList();		// this is wrong we should use a design pattern called Specification Pattern to solve this problem.
			}
			return _dbContext.Set<T>().ToList();
		}

		public T GetById(int Id)
		{
			return _dbContext.Set<T>().Find(Id);  // or return _dbContext.Find<T>(Id);;
		}

		public void Update(T entity)
		{
			_dbContext.Update(entity);
			
		}
	}
	
}
