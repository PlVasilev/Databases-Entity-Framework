using System.Collections.Generic;
using AutoMapper.QueryableExtensions;
using FastFood.Models;
using FastFood.Models.Enums;
using FastFood.Web.ViewModels.Employees;

namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    using Data;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var viewOrder = new CreateOrderViewModel
            {
                Items = this.context.Items.Select(x => x.Name).ToList(),
                Employees = this.context.Employees.Select(x => x.Name).ToList(),
            };

            return this.View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }
            var order = new Order
            {
                Customer = model.Customer,
                DateTime = DateTime.Now,
                Type = Enum.Parse<OrderType>(model.OrderType),
                TotalPrice = model.Quantity * context.Items.FirstOrDefault(x => x.Name == model.ItemName).Price,
                EmployeeId = context.Employees.FirstOrDefault(x => x.Name == model.EmployeeName).Id,// model.EmployeeId,
                Employee = context.Employees.FirstOrDefault(x => x.Name == model.EmployeeName),
                OrderItems = new List<OrderItem>()
            };
            order.OrderItems.Add(new OrderItem
            {
                ItemId = context.Items.FirstOrDefault(x => x.Name == model.ItemName).Id,// model.ItemId,
                OrderId = order.Id,
                Quantity = model.Quantity
            });
            ;
            context.Orders.Add(order);
            context.SaveChanges();

            return this.RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            var orders = context.Orders.ProjectTo<OrderAllViewModel>(mapper.ConfigurationProvider).ToList();

            return View(orders);
        }
    }
}
