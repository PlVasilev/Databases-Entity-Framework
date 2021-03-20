using System.Linq;
using AutoMapper.QueryableExtensions;
using FastFood.Models;
using FastFood.Web.ViewModels.Positions;

namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System;

    using Data;
    using ViewModels.Employees;

    public class EmployeesController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public EmployeesController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Register()
        {
            var positIons = this.context.Positions.ProjectTo<RegisterEmployeeViewModel>(mapper.ConfigurationProvider).ToList();
            return this.View(positIons);
        }

        [HttpPost]
        public IActionResult Register(RegisterEmployeeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            Position position = this.context.Positions.FirstOrDefault(x => x.Name == model.PositionName);
            var employee = mapper.Map<Employee>(model);
            employee.PositionId = position.Id;
            context.Employees.Add(employee);
            context.SaveChanges();

            return RedirectToAction("All", "Employees");
        }

        public IActionResult All()
        {
            var empluyees = context.Employees.ProjectTo<EmployeesAllViewModel>(mapper.ConfigurationProvider).ToList();

            return View(empluyees);
        }
    }
}
