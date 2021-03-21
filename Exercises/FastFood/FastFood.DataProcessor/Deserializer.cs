using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
	public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

		public static string ImportEmployees(FastFoodDbContext context, string jsonString)
		{
		    var dtos = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);
		    StringBuilder sb = new StringBuilder();
		    List<Employee> employees = new List<Employee>();
		    List<Position> positions = new List<Position>();

            foreach (var dto in dtos)
		    {
		        if (!IsValid(dto))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        var position = positions.FirstOrDefault(x => x.Name == dto.Position);
		        if (position == null) 
		        {
		            position = new Position
		            {
                        Name = dto.Position
		            };
		            positions.Add(position);
                }

                var employee = new Employee
                {
                    Name = dto.Name,
                    Age = dto.Age,
                    Position = position                   
                };
                employees.Add(employee);

		        sb.AppendLine(string.Format(SuccessMessage,employee.Name));
		    }
		    context.Employees.AddRange(employees);
		    context.SaveChanges();
		    return sb.ToString().TrimEnd();
        }

		public static string ImportItems(FastFoodDbContext context, string jsonString)
		{
		    var dtos = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);
		    StringBuilder sb = new StringBuilder();
		    List<Item> items = new List<Item>();
		    List<Category> categories = new List<Category>();

		    foreach (var dto in dtos)
		    {
		        if (!IsValid(dto) || items.Any(x => x.Name == dto.Name))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        var category = categories.FirstOrDefault(x => x.Name == dto.Category);
		        if (category == null)
		        {
		            category = new Category()
		            {
		                Name = dto.Category
		            };
		            categories.Add(category);
		        }

		        var item = new Item()
		        {
		            Name = dto.Name,
		            Price = dto.Price,
		            Category = category
		        };
		        items.Add(item);

		        sb.AppendLine(string.Format(SuccessMessage, item.Name));
		    }
		    context.Items.AddRange(items);
		    context.SaveChanges();
		    return sb.ToString().TrimEnd();
        }

		public static string ImportOrders(FastFoodDbContext context, string xmlString)
		{
            var serializer = new XmlSerializer(typeof(OrdersDto[]), new XmlRootAttribute("Orders"));
            var deserialized = (OrdersDto[])serializer.Deserialize(new StringReader(xmlString));
            var orders = new List<Order>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in deserialized)
            {
                if (!IsValid(dto) || !dto.Items.All(IsValid))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }
                var employee = context.Employees.FirstOrDefault(x => x.Name == dto.EmployeeName);
                if (employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                OrderType orderType;
                if (!Enum.TryParse(dto.Type, true, out orderType))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Order order = new Order()
                {
                    Customer = dto.CustomerName,
                    DateTime = DateTime.ParseExact(dto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Type = orderType,
                    Employee = employee
                };

                var isValidated = true;
                //List<OrderItem> orderItems = new List<OrderItem>();
                foreach (var itemDto in dto.Items)
                {
                    var item = context.Items.FirstOrDefault(x => x.Name == itemDto.ItemName);
                    if (item == null)
                    {
                        isValidated = false;
                        break;                      
                    }     
                    OrderItem orderItem = new OrderItem
                    {
                        Order = order,
                        Item = item,
                        Quantity = itemDto.Quantity
                    };
                    order.OrderItems.Add(orderItem);
                }
                if (isValidated == false)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }               
                orders.Add(order);
                sb.AppendLine($"Order for {dto.CustomerName} on {dto.DateTime} added");
            }
            context.Orders.AddRange(orders);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

	    private static bool IsValid(object entity)
	    {
	        var validationContext = new ValidationContext(entity);
	        var validationResult = new List<ValidationResult>();

	        bool isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

	        return isValid;
	    }
    }
}