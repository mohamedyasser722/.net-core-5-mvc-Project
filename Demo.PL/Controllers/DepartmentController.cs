using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;


namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)// ask CLR to create an instance of IUnitOfWork
		{
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
			return View();
		}
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid) // server side validation
            {   
				_unitOfWork.DepartmentRepository.Add(department);
                int res = _unitOfWork.Complete();
                if(res > 0)
                {
                    TempData["Message"] = "Department Added Successfully";
                }
				return RedirectToAction("Index");
			}
			return View(department);
		}


        public IActionResult Details(int? id, string ViewName="Details")
        {
            if(id == null)
                return BadRequest();

            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            if(department == null)
                return NotFound();
            return View(ViewName,department);
            
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
			//if(id == null)
			//	return BadRequest();

			//var department = _departmentRepository.GetById(id.Value);
			//if(department == null)
			//	return NotFound();
			//return View(department);
            return Details(id, nameof(Edit));
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Edit(Department department , [FromRoute] int id) 
        {
            if (id != department.Id)   // to prevent hackers from changing the id in the form , using overposting attack
                return BadRequest();
      
			if (ModelState.IsValid) // server side validation
            {
                try
                {
					_unitOfWork.DepartmentRepository.Update(department);
                    _unitOfWork.Complete();
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
        public IActionResult Delete(int? id)
        {
			
            return Details(id, nameof(Delete));
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Department department , [FromRoute] int id)
        {
			if (id != department.Id)   // to prevent hackers from changing the id in the form , using overposting attack
				return BadRequest();
            
            try
            {
				_unitOfWork.DepartmentRepository.Delete(department);
                _unitOfWork.Complete();
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
