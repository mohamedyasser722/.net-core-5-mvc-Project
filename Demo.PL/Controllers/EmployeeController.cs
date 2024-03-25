using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Demo.PL.Helpers;


namespace Demo.PL.Controllers
{
	public class EmployeeController : Controller
	{
		
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public EmployeeController(IUnitOfWork unitOfWork ,IMapper mapper) // ASK CLR to create an instance of IUnitOfWork
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}
		public IActionResult Index()
		{
			var employees = _unitOfWork.EmployeesRepository.GetAll();
			var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
			return View(MappedEmployees);
		}

		[HttpPost]
		public IActionResult Search(string searchString)
		{
			var employees = _unitOfWork.EmployeesRepository.GetAll();

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

			var employee = _unitOfWork.EmployeesRepository.GetById(id.Value);

			if (employee == null)
				return NotFound();

			var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
			return View(ViewName, MappedEmployee);
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
			return View();
		}
		[HttpPost]  
		public IActionResult Create(EmployeeViewModel employeeVM)
		{
			if (ModelState.IsValid)
			{


				employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
				

				// map the employeeVM to employee using automapper
				var MappedEmployee = _mapper.Map<EmployeeViewModel , Employee>(employeeVM);
				_unitOfWork.EmployeesRepository.Add(MappedEmployee);
				int res = _unitOfWork.Complete();
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
			ViewBag.Departments =_unitOfWork.DepartmentRepository.GetAll();   // to solve the error of null reference exception
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
					_unitOfWork.EmployeesRepository.Update(MappedEmployee);
					_unitOfWork.Complete();
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
				_unitOfWork.EmployeesRepository.Delete(MappedEmployee);
				int result = _unitOfWork.Complete();
				if(result > 0 && employeeVM.ImageName is not null)
				{
					DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
					TempData["Message"] = "Employee Deleted Successfully";
				}
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
