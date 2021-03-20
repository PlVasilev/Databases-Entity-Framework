using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string suppliersJason = File.ReadAllText(@"..\..\..\Datasets\suppliers.json");
            string partsJason = File.ReadAllText(@"..\..\..\Datasets\parts.json");
            string carsJason = File.ReadAllText(@"..\..\..\Datasets\cars.json");
            string customersJason = File.ReadAllText(@"..\..\..\Datasets\customers.json");
            string salesJason = File.ReadAllText(@"..\..\..\Datasets\sales.json");

            using (var context = new CarDealerContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                //Console.WriteLine(ImportSuppliers(context, suppliersJason));
                //Console.WriteLine(ImportParts(context, partsJason));
                Console.WriteLine(ImportCars(context, carsJason));
                //Console.WriteLine(ImportCustomers(context, customersJason));
                //Console.WriteLine(ImportSales(context, salesJason));
                //Console.WriteLine(GetOrderedCustomers(context));
                //Console.WriteLine(GetCarsFromMakeToyota(context));
                //Console.WriteLine(GetLocalSuppliers(context));
                //Console.WriteLine(GetCarsWithTheirListOfParts(context));
                //Console.WriteLine(GetTotalSalesByCustomer(context));
                //Console.WriteLine(GetSalesWithAppliedDiscount(context));
           }
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson);
            List<Part> partsToInsert = new List<Part>();
            var supliyersIds = context.Suppliers.Select(x => x.Id).ToArray();

            foreach (var part in parts)
            {
                if (!supliyersIds.Contains(part.SupplierId))
                {
                    continue;
                }
                partsToInsert.Add(part);
            }
            context.Parts.AddRange(partsToInsert);
            context.SaveChanges();

            return $"Successfully imported {partsToInsert.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            //Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            var avaibleParts = context.Parts.Select(p => p.Id).ToArray();
            var carDtos = JsonConvert.DeserializeObject<CarInsertDto[]>(inputJson);
            List<Car> cars = new List<Car>();

            foreach (var carDto in carDtos)
            {
                Car car = Mapper.Map<CarInsertDto, Car>(carDto);
                cars.Add(car);

                var partIds = carDto.PartsId.Distinct().ToList();
            
                foreach (var pid in partIds)
                {
                    if (!avaibleParts.Contains(pid))
                    {
                        continue;
                    }
                    var currentPair = new PartCar()
                    {
                        Car = car,
                        PartId = pid
                    };
                    car.PartCars.Add(currentPair);
                }          
            }
            
            context.Cars.AddRange(cars);
            context.SaveChanges();
            
            return $"Successfully imported {cars.Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var orderedCustomers = context.Customers.OrderBy(x => x.BirthDate).ThenBy(x => x.IsYoungDriver)
                .Select(cus => new
                {
                    cus.Name,
                    BirthDate = cus.BirthDate.ToString("dd/MM/yyyy"),
                    cus.IsYoungDriver
                }).ToArray();

            var json = JsonConvert.SerializeObject(orderedCustomers, Formatting.Indented);
            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var carsFromMakeToyota = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .Select(car => new
                    {
                        car.Id,car.Make,car.Model,car.TravelledDistance
                    })
                .ToArray();

            var json = JsonConvert.SerializeObject(carsFromMakeToyota, Formatting.Indented);
            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers.Where(x => x.IsImporter == false)
                .Select(sup => new
                {
                    sup.Id,
                    sup.Name,
                    PartsCount = sup.Parts.Count
                }).ToArray();

            var json = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);
            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithTheirListOfParts = context.Cars.Select(cp => new
            {
                car = new
                {
                    cp.Make,
                    cp.Model,
                    cp.TravelledDistance
                },
                parts = cp.PartCars.Select(p => new
                {
                    p.Part.Name,
                    Price = p.Part.Price.ToString("0.00")
                }).ToArray()
            }).ToArray();
         

            var json = JsonConvert.SerializeObject(carsWithTheirListOfParts, Formatting.Indented);
            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var totalSalesByCustomer = context.Customers
                .Where(c => c.Sales.Any(b => b.Customer != null))
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.Select(x => x.Car.PartCars.Sum(p => p.Part.Price)).Sum()
                }).OrderByDescending(x => x.spentMoney)
                .ThenByDescending(x => x.boughtCars)
                .ToArray();

            var json = JsonConvert.SerializeObject(totalSalesByCustomer, Formatting.Indented);
            return json;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var salesWithAppliedDiscount = context.Sales.Select(x => new
            {
                car = new
                {
                    x.Car.Make,
                    x.Car.Model,
                    x.Car.TravelledDistance
                },
                customerName = x.Customer.Name,
                Discount = x.Discount.ToString("0.00"),
                price = x.Car.PartCars.Sum(p => p.Part.Price).ToString("0.00"),
                priceWithDiscount = (x.Car.PartCars.Sum(p => p.Part.Price) * (1 - x.Discount/100)).ToString("0.00")
            }).Take(10).ToArray();


            var json = JsonConvert.SerializeObject(salesWithAppliedDiscount, Formatting.Indented);

            return json;


          
        }
    }
}