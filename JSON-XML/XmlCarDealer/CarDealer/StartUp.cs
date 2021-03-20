using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());
            string suppliersXml = File.ReadAllText(@"..\..\..\Datasets\suppliers.xml");
            string partXml = File.ReadAllText(@"..\..\..\Datasets\parts.xml");
            string carsXml = File.ReadAllText(@"..\..\..\Datasets\cars.xml");
            string customersXml = File.ReadAllText(@"..\..\..\Datasets\customers.xml");
            string salesXml = File.ReadAllText(@"..\..\..\Datasets\sales.xml");

            using (CarDealerContext context = new CarDealerContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                //
                //Console.WriteLine(ImportSuppliers(context, suppliersXml));
                //Console.WriteLine(ImportParts(context, partXml));
                //Console.WriteLine(ImportCars(context, carsXml));
                //Console.WriteLine(ImportCustomers(context, customersXml));
                //Console.WriteLine(ImportSales(context, salesXml));
                //Console.WriteLine(GetCarsWithDistance(context));
                //Console.WriteLine(GetCarsFromMakeBmw(context));
                //Console.WriteLine(GetLocalSuppliers(context));
                //Console.WriteLine(GetCarsWithTheirListOfParts(context));
                //Console.WriteLine(GetTotalSalesByCustomer(context));
                Console.WriteLine(GetSalesWithAppliedDiscount(context));

            }

        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity); //context of the validation object
            List<ValidationResult> validationResults = new List<ValidationResult>(); // list of attributes to validate

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true); // true to validate all props

            return isValid;
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SuplierDto[]), new XmlRootAttribute("Suppliers"));  //<Users>
            var suplierDtos = (SuplierDto[])serializer.Deserialize(new StringReader(inputXml));
            List<Supplier> suppliers = new List<Supplier>();

            foreach (var suplierDto in suplierDtos)
            {
                if (!IsValid(suplierDto))
                {
                    continue;
                }
                var supplier = Mapper.Map<Supplier>(suplierDto);
                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PartDto[]), new XmlRootAttribute("Parts"));  //<Users>
            var partDtos = (PartDto[])serializer.Deserialize(new StringReader(inputXml));
            List<Part> parts = new List<Part>();

            foreach (var partDto in partDtos)
            {
                if (!IsValid(partDto) || context.Suppliers.Find(partDto.SupplierId) == null)
                {
                    continue;
                }
                var part = Mapper.Map<Part>(partDto);
                parts.Add(part);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CarDto[]), new XmlRootAttribute("Cars"));  //<Users>
            var carDtos = (CarDto[])serializer.Deserialize(new StringReader(inputXml));
            List<Car> cars = new List<Car>();
            

            foreach (var carDto in carDtos)
            {
                if (!IsValid(carDto))
                {
                    continue;
                }
                var car = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance
                };

                var uniquePartsIds = carDto.partIds.Select(x => int.Parse(x.Id)).Distinct().ToArray();

                foreach (var carDtoPartId in uniquePartsIds)
                {
                    Part part = context.Parts.Find(carDtoPartId);

                    if (part == null)
                    {
                        continue;
                    }
   
                    var patrtCar = new PartCar
                    {
                        Car = car,
                        Part = part
                    };

                    car.PartCars.Add(patrtCar);
                }
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CustomerDto[]), new XmlRootAttribute("Customers"));  
            var customerDtos = (CustomerDto[])serializer.Deserialize(new StringReader(inputXml));
            List<Customer> customers = new List<Customer>();

            foreach (var customerDto in customerDtos)
            {
                if (!IsValid(customerDto))
                {
                    continue;
                }
                var customer = Mapper.Map<Customer>(customerDto);
                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SaleDto[]), new XmlRootAttribute("Sales"));
            var saleDtos = (SaleDto[])serializer.Deserialize(new StringReader(inputXml));
            List<Sale> sales = new List<Sale>();

            foreach (var saleDto in saleDtos)
            {

                if (!IsValid(saleDto) || context.Cars.Find(saleDto.carId) == null)
                {
                    continue;
                }
                var sale = Mapper.Map<Sale>(saleDto);
                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carsWithDistance = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .Select(x => new CarsWithDistanceDTO
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                }).OrderBy(c => c.Make).ThenBy(c => c.Model).Take(10).ToArray();


            XmlSerializer serializer = new XmlSerializer(typeof(CarsWithDistanceDTO[]), new XmlRootAttribute("cars"));
            var sb = new StringBuilder();
            var nameSpaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            serializer.Serialize(new StringWriter(sb), carsWithDistance, nameSpaces);
            return sb.ToString().TrimEnd();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var carsFromMakeBmw = context.Cars.Where(c => c.Make == "BMW")
                .Select(c => new CarFromMakeBmwDbo
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                }).OrderBy(c => c.Model).ThenByDescending(c => c.TravelledDistance).ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(CarFromMakeBmwDbo[]), new XmlRootAttribute("cars"));
            var sb = new StringBuilder();
            var nameSpaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            serializer.Serialize(new StringWriter(sb), carsFromMakeBmw, nameSpaces);
            return sb.ToString().TrimEnd();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers.Where(s => s.IsImporter == false)
                .Select(s => new LocalSupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                }).ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(LocalSupplierDto[]), new XmlRootAttribute("suppliers"));
            var sb = new StringBuilder();
            var nameSpaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            serializer.Serialize(new StringWriter(sb), localSuppliers, nameSpaces);
            return sb.ToString().TrimEnd();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithTheirListOfParts = context.Cars
                .Select(c => new CarWithTheirListOfPartsDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars.Select(p => new ExportPartDto
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    }).OrderByDescending(p => p.Price).ToArray()
                }).OrderByDescending(c => c.TravelledDistance).ThenBy(c => c.Model).Take(5).ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(CarWithTheirListOfPartsDto[]), new XmlRootAttribute("cars"));
            var sb = new StringBuilder();
            var nameSpaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            serializer.Serialize(new StringWriter(sb), carsWithTheirListOfParts, nameSpaces);
            return sb.ToString().TrimEnd();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var totalSalesByCustomer = context.Customers.Where(c => c.Sales.Count > 0)
                .Select(c => new TotalSalesByCustomerDto
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Select(s => s.Car.PartCars.Sum(p => p.Part.Price)).Sum()
                }).OrderByDescending(s => s.SpentMoney).ToArray();
                

            XmlSerializer serializer = new XmlSerializer(typeof(TotalSalesByCustomerDto[]), new XmlRootAttribute("customers"));
            var sb = new StringBuilder();
            var nameSpaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            serializer.Serialize(new StringWriter(sb), totalSalesByCustomer, nameSpaces);
            return sb.ToString().TrimEnd();
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            //var salesWithAppliedDiscount = context.Sales
            //    .Select(s => new saleWithAppliedDiscountDto
            //    {
            //        CarsWithDistanceDto = new CarsWithAttribDTO()
            //        {
            //            Make = s.Car.Make,
            //            Model = s.Car.Model,                       
            //            TravelledDistance = s.Car.TravelledDistance
            //        },
            //        SaleDiscount = s.Discount,
            //        CustomerName = s.Customer.Name,
            //        CarPrice = s.Car.PartCars.Sum(p => p.Part.Price).ToString("F2"),
            //        CarPriceWithDiscount = (s.Car.PartCars.Sum(p => p.Part.Price) * (1 - s.Discount / 100)).ToString("F2")
            //    }).ToArray();

            var salesWithAppliedDiscount = context.Sales
                .Select(x => new saleWithAppliedDiscountDto()
                {
                    CarsWithDistanceDto = new CarsWithAttribDTO()
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    SaleDiscount = x.Discount,
                    CustomerName = x.Customer.Name,
                    CarPrice = x.Car.PartCars.Sum(y => y.Part.Price),
                    CarPriceWithDiscount = $"{ (x.Car.PartCars.Sum(y => y.Part.Price) - (x.Car.PartCars.Sum(y => y.Part.Price) * x.Discount / 100))}"
                })
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(saleWithAppliedDiscountDto[]), new XmlRootAttribute("sales"));
            var sb = new StringBuilder();
            var nameSpaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            serializer.Serialize(new StringWriter(sb), salesWithAppliedDiscount, nameSpaces);
            //Console.WriteLine(sb.ToString().TrimEnd().Substring(195));
            return sb.ToString().TrimEnd();

        }
    }
}
