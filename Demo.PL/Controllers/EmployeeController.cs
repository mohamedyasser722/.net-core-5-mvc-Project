using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IEmployeesRepository _employeesRepository;
		private readonly IDepartmentRepository _departmentRepository;
		private readonly IMapper _mapper;
		public EmployeeController(IEmployeesRepository employeesRepository, IDepartmentRepository departmentRepository ,
			IMapper mapper) 
		{
			_employeesRepository = employeesRepository;
			_departmentRepository = departmentRepository;
			_mapper = mapper;

		}
		public IActionResult Index()
		{
			var employees = _employeesRepository.GetAll();
			var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
			return View(MappedEmployees);
		}

		[HttpGet]
		public IActionResult Search(string searchString)
		{
			var employees = _employeesRepository.GetAll();
			
			if (!string.IsNullOrEmpty(searchString))
			{
				employees = employees.Where(e => e.Name.ToLower().Contains(searchString.ToLower()));
				

			}
			var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
			return PartialView("PartialViews/_EmployeeListPartial", MappedEmployees);
		}


		public IActionResult Details(int? id, string ViewName = "Details")
		{
			if (id == null)
				return BadRequest();

			var employee = _employeesRepository.GetById(id.Value);

			if (employee == null)
				return NotFound();

			var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
			return View(ViewName, MappedEmployee);
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Departments = _departmentRepository.GetAll();
			return View();
		}
		[HttpPost] 
		public IActionResult Create(EmployeeViewModel employeeVM)
		{
			if (ModelState.IsValid)
			{
				// map the employeeVM to employee using automapper
				var MappedEmployee = _mapper.Map<EmployeeViewModel , Employee>(employeeVM);
				int res = _employeesRepository.Add(MappedEmployee);
				if (res > 0)
				{
					TempData["Message"] = "Employee Added Successfully";
				}
				return RedirectToAction("Index");
			}
			return View(employeeVM);
		}
		[HttpGet]
		public IActionResult Edit(int? id)
		{
			ViewBag.Departments = _departmentRepository.GetAll();   // to solve the error of null reference exception
			return Details(id, nameof(Edit));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
		{
			if (id != employeeVM.Id)
				return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
					var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
					_employeesRepository.Update(MappedEmployee);
					return RedirectToAction("Index");
				}
				catch (System.Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			return View(employeeVM);
		}

		public IActionResult Delete(int? id)
		{
			return Details(id, nameof(Delete));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
		{
			if (id != employeeVM.Id)
				return BadRequest();
			try
			{
				var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
				_employeesRepository.Delete(MappedEmployee);
				return RedirectToAction("Index");
			}
			catch (System.Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View(employeeVM);
		}

	}
}
