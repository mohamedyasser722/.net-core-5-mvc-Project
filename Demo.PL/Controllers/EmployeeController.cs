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
using System.Threading.Tasks;


namespace Demo.PL.Controllers
{
	public class EmployeeController : Controller
	{

		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper) // ASK CLR to create an instance of IUnitOfWork
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}
		public async Task<IActionResult> Index()
		{
			var employees = await _unitOfWork.EmployeesRepository.GetAllAsync();
			var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
			return View(MappedEmployees);
		}

		[HttpPost]
		public async Task<IActionResult> Search(string searchString)
		{
			var employees = await _unitOfWork.EmployeesRepository.GetAllAsync();

			if (!string.IsNullOrEmpty(searchString))
			{
				employees = employees.Where(e => e.Name.ToLower().Contains(searchString.ToLower()));


			}
			var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
			return PartialView("PartialViews/_EmployeeListPartial", MappedEmployees);
		}


		public async Task<IActionResult> Details(int? id, string ViewName = "Details")
		{
			if (id == null)
				return BadRequest();

			var employee = await _unitOfWork.EmployeesRepository.GetByIdAsync(id.Value);

			if (employee == null)
				return NotFound();

			var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
			return View(ViewName, MappedEmployee);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
		{
			if (ModelState.IsValid)
			{


				employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");


				// map the employeeVM to employee using automapper
				var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
				await _unitOfWork.EmployeesRepository.AddAsync(MappedEmployee);
				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
				{
					TempData["Message"] = "Employee Added Successfully";
				}
				return RedirectToAction("Index");
			}
			return View(employeeVM);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAllAsync();   // to solve the error of null reference exception
			return await Details(id, nameof(Edit));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
		{
			if (id != employeeVM.Id)
				return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
					employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
					var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
					_unitOfWork.EmployeesRepository.Update(MappedEmployee);
					await _unitOfWork.CompleteAsync();
					return RedirectToAction("Index");
				}
				catch (System.Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			return View(employeeVM);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			return await Details(id, nameof(Delete));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
		{
			if (id != employeeVM.Id)
				return BadRequest();
			try
			{
				var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
				_unitOfWork.EmployeesRepository.Delete(MappedEmployee);
				int result = await _unitOfWork.CompleteAsync();
				if (result > 0 && employeeVM.ImageName is not null)
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
