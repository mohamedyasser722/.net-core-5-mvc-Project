using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)// ask CLR to create an instance of IUnitOfWork
		{
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
			return View();
		}
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) // server side validation
            {   
				await _unitOfWork.DepartmentRepository.AddAsync(department);
                int res = await _unitOfWork.CompleteAsync();
                if(res > 0)
                {
                    TempData["Message"] = "Department Added Successfully";
                }
				return RedirectToAction("Index");
			}
			return View(department);
		}


        public async Task<IActionResult> Details(int? id, string ViewName="Details")
        {
            if(id == null)
                return BadRequest();

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if(department == null)
                return NotFound();
            return View(ViewName, department);
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
			//if(id == null)
			//	return BadRequest();

			//var department = _departmentRepository.GetById(id.Value);
			//if(department == null)
			//	return NotFound();
			//return View(department);
            return await Details(id, nameof(Edit));
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Department department , [FromRoute] int id) 
        {
            if (id != department.Id)   // to prevent hackers from changing the id in the form , using overposting attack
                return BadRequest();
      
			if (ModelState.IsValid) // server side validation
            {
                try
                {
					_unitOfWork.DepartmentRepository.Update(department);
                    await _unitOfWork.CompleteAsync();
					return RedirectToAction("Index");
				}
				catch (System.Exception ex)
                {
                    // 1. Log the error
                    // 2. Display a friendly error message to the user

					ModelState.AddModelError(string.Empty, ex.Message);
				}
            }
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
			
            return await Details(id, nameof(Delete));
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department , [FromRoute] int id)
        {
			if (id != department.Id)   // to prevent hackers from changing the id in the form , using overposting attack
				return BadRequest();
            
            try
            {
				_unitOfWork.DepartmentRepository.Delete(department);
                await _unitOfWork.CompleteAsync();
				return RedirectToAction("Index");
			}
			catch (System.Exception ex)
            {
				// 1. Log the error
				// 2. Display a friendly error message to the user

				ModelState.AddModelError(string.Empty, ex.Message);
            }
            
            return View(department);
        }

         
    }
}
