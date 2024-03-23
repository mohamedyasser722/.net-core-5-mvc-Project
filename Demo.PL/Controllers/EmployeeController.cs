using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;

namespace Demo.PL.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IEmployeesRepository _employeesRepository;
		private readonly IDepartmentRepository _departmentRepository;
		public EmployeeController(IEmployeesRepository employeesRepository , IDepartmentRepository departmentRepository)	// ask CLR to create an Object from a class Implements IEmployeesRepository and inject it into the constructor
		{
			_employeesRepository = employeesRepository;
			_departmentRepository = departmentRepository;

		}
		public IActionResult Index()
		{
			var employees = _employeesRepository.GetAll();
			return View(employees);
		}

		[HttpGet]
		public IActionResult Search(string searchString)
		{
			var employees = _employeesRepository.GetAll();

			if (!string.IsNullOrEmpty(searchString))
			{
				employees = employees.Where(e => e.Name.Contains(searchString));
			}

			return PartialView("_EmployeeListPartial", employees);
		}


		public IActionResult Details(int? id , string ViewName = "Details")
		{
			if (id == null)
				return BadRequest();

			var employee = _employeesRepository.GetById(id.Value);
			if (employee == null)
				return NotFound();
			return View(ViewName,employee);
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Departments = _departmentRepository.GetAll();
			return View();
		}
		[HttpPost]
		public IActionResult Create(Employee employee)
		{
			if(ModelState.IsValid)
			{
				int res = _employeesRepository.Add(employee);
				if(res > 0)
				{
					TempData["Message"] = "Employee Added Successfully";
				}
				return RedirectToAction("Index");
			}
			return View(employee);
		}
		[HttpGet]
		public IActionResult Edit(int? id)
		{
			ViewBag.Departments = _departmentRepository.GetAll();	// to solve the error of null reference exception
			return Details(id, nameof(Edit));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult Edit(Employee employee, [FromRoute] int id)
		{
			if(id != employee.Id)
				return BadRequest();
			if(ModelState.IsValid)
			{
				try
				{
					_employeesRepository.Update(employee);
					return RedirectToAction("Index");
				}
				catch(System.Exception ex)
				{
					ModelState.AddModelError(string.Empty,ex.Message);
				}
			}
			return View(employee);
		}

		public IActionResult Delete(int? id)
		{
			return Details(id, nameof(Delete));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(Employee employee, [FromRoute] int id)
		{
			if(id != employee.Id)
				return BadRequest();
			try
			{
				_employeesRepository.Delete(employee);
				return RedirectToAction("Index");
			}
			catch(System.Exception ex)
			{
				ModelState.AddModelError(string.Empty,ex.Message);
			}
			return View(employee);
		}

	}
}
