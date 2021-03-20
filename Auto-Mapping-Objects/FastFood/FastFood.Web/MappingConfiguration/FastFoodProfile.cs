﻿using System;
using System.Globalization;
using System.Linq;
using FastFood.Web.ViewModels.Categories;
using FastFood.Web.ViewModels.Employees;
using FastFood.Web.ViewModels.Items;
using FastFood.Web.ViewModels.Orders;

namespace FastFood.Web.MappingConfiguration
{
    using AutoMapper;
    using Models;

    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            //Employees
            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(x => x.PositionName, y => y.MapFrom(p => p.Name));

            this.CreateMap<RegisterEmployeeInputModel, Employee>();
          

            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(x => x.Position, y => y.MapFrom(s => s.Position.Name));

            //Categories
            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(x => x.Name, y => y.MapFrom(c => c.CategoryName));

            this.CreateMap<Category, CategoryAllViewModel>();

            //Items
            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(x => x.CategoryId, y => y.MapFrom(c => c.Id))
                .ForMember(x => x.CategoryName, y => y.MapFrom(n => n.Name));

            this.CreateMap<CreateItemInputModel, Item>()
                .ForMember(x => x.CategoryId, y => y.MapFrom(
                    c => int.Parse(c.CategoryIdName.Split(" ",StringSplitOptions.RemoveEmptyEntries).First())));

            this.CreateMap<Item, ItemsAllViewModels>()
                .ForMember(x => x.Category, y => y.MapFrom(c => c.Category.Name));

            //Orders
            this.CreateMap<CreateItemInputModel, Order>();
                

            this.CreateMap<Order, OrderAllViewModel>()
                .ForMember(x => x.OrderId, y => y.MapFrom(c => c.Id))
                .ForMember(x => x.Employee, y => y.MapFrom(e => e.Employee.Name))
                .ForMember(x => x.DateTime, y => y.MapFrom(e => e.DateTime.ToString("dd-MM-yyyy hh-mm-ss")));


        }
    }
}
