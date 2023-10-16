using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDBContext mvcDemoDBContext;

        public EmployeesController(MVCDemoDBContext mvcDemoDBContext)
        {

            this.mvcDemoDBContext = mvcDemoDBContext;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employess = await mvcDemoDBContext.Employees.ToListAsync();
            return View(employess);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

            await mvcDemoDBContext.Employees.AddAsync(employee);
            await mvcDemoDBContext.SaveChangesAsync();

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = mvcDemoDBContext.Employees.FirstOrDefault(x => x.Id == id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewmodel()
                {

                    Id = Guid.NewGuid(),
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };

                return await Task.Run(()=>View("View",viewModel));
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewmodel updateEmployeeViewmodel)
        {
            var employee = await mvcDemoDBContext.Employees.FindAsync(updateEmployeeViewmodel.Id);

            if(employee != null)
            {
                employee.Id=updateEmployeeViewmodel.Id;
                employee.Name = updateEmployeeViewmodel.Name;
                employee.Salary = updateEmployeeViewmodel.Salary;
                employee.DateOfBirth = updateEmployeeViewmodel.DateOfBirth;
                employee.Department = updateEmployeeViewmodel.Department;

                await mvcDemoDBContext.SaveChangesAsync();

                return RedirectToAction("Index");

            }
            return RedirectToAction("Add");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewmodel model)
        {
            var employee = await mvcDemoDBContext.Employees.FindAsync(model.Id);

            if(employee != null)
            {
                mvcDemoDBContext.Employees.Remove(employee);

                await mvcDemoDBContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

    }
}
