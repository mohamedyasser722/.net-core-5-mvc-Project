using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;


namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository) // ask CLR to inject the departmentRepository into the DepartmentController , i need to go to the startup.cs to configure the dependency injection
        {
            _departmentRepository = departmentRepository;
        }

        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
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
				_departmentRepository.Add(department);
				return RedirectToAction("Index");
			}
			return View(department);
		}


        public IActionResult Details(int? id, string ViewName="Details")
        {
            if(id == null)
                return BadRequest();

            var department = _departmentRepository.GetById(id.Value);
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
					_departmentRepository.Update(department);
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
				_departmentRepository.Delete(department);
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
