using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            string usersXml = File.ReadAllText(@"..\..\..\Datasets\users.xml");
            string productsXml = File.ReadAllText(@"..\..\..\Datasets\products.xml");
            string categoriesXml = File.ReadAllText(@"..\..\..\Datasets\categories.xml");
            string categoriesProductsXml = File.ReadAllText(@"..\..\..\Datasets\categories-products.xml");

            using (ProductShopContext context = new ProductShopContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                //
                //Console.WriteLine(ImportUsers(context, usersXml));
                //Console.WriteLine(ImportProducts(context, productsXml));
                //Console.WriteLine(ImportCategories(context, categoriesXml));
                //Console.WriteLine(ImportCategoryProducts(context, categoriesProductsXml));
                //Console.WriteLine(GetProductsInRange(context));
                //Console.WriteLine(GetSoldProducts(context));
                Console.WriteLine(GetCategoriesByProductsCount(context));
                //Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity); //context of the validation object
            List<ValidationResult> validationResults = new List<ValidationResult>(); // list of attributes to validate

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true); // true to validate all props

            return isValid;
        }


        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportUserDto[]), new XmlRootAttribute("Users"));  //<Users>
            var usersDto = (ImportUserDto[])serializer.Deserialize(new StringReader(inputXml));
            List<User> users = new List<User>();

            foreach (var importUserDto in usersDto)
            {

                var user = Mapper.Map<User>(importUserDto);
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));
            var productsDto = (ImportProductDto[])serializer.Deserialize(new StringReader(inputXml));
            List<Product> products = new List<Product>();

            foreach (var importUserDto in productsDto)
            {
                var product = Mapper.Map<Product>(importUserDto);
                if (!IsValid(product))
                {
                    continue;
                }
                products.Add(product);
            }
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCategoryDto[]), new XmlRootAttribute("Categories"));
            var categoriesDto = (ImportCategoryDto[])serializer.Deserialize(new StringReader(inputXml));
            List<Category> categories = new List<Category>();

            foreach (var categoryDto in categoriesDto)
            {
                var category = Mapper.Map<Category>(categoryDto);
                if (!IsValid(category))
                {
                    continue;
                }
                categories.Add(category);
            }
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCategoryProductDto[]), new XmlRootAttribute("CategoryProducts"));
            var categoriesProductsDto = (ImportCategoryProductDto[])serializer.Deserialize(new StringReader(inputXml));
            List<CategoryProduct> categoriesProducts = new List<CategoryProduct>();


            foreach (var categoryProductDto in categoriesProductsDto)
            {
                var productId = context.Products.Find(categoryProductDto.ProductId);
                var categoryId = context.Categories.Find(categoryProductDto.CategoryId);

                if (productId == null || categoryId == null)
                {
                    continue;
                }
                var categoryProduct = Mapper.Map<CategoryProduct>(categoryProductDto);
                if (!IsValid(categoryProduct))
                {
                    continue;
                }
                categoriesProducts.Add(categoryProduct);
            }
            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new ExportProductDto
                {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName
                })
                .OrderBy(x => x.Price).Take(10).ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportProductDto[]), new XmlRootAttribute("Products"));
            var sb = new StringBuilder();
            var nameSpaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            serializer.Serialize(new StringWriter(sb), products, nameSpaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var userSales = context.Users
                .Include(i => i.ProductsSold)
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(u => new UserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = new SoldProducts
                    {
                        ProductDtos = u.ProductsSold.Where(ps => ps.Buyer != null)
                                .Select(ps => new SoldProductDto
                                {
                                    Name = ps.Name,
                                    Price = ps.Price,
                                }).ToList()
                    }
                })
                .Take(5).ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("Users"));
            var sb = new StringBuilder();
            var nameSpaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            serializer.Serialize(new StringWriter(sb), userSales, nameSpaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categoriesDto = context.Categories
                .Select(x => new CategoryDto
                {
                    Name = x.Name,
                    Count = x.CategoryProducts.Count,
                    AveragePrice = x.CategoryProducts.Average(y => y.Product.Price),
                    TotalRevenue = x.CategoryProducts.Sum(y => y.Product.Price)
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));
            var sb = new StringBuilder();
            var nameSpaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            serializer.Serialize(new StringWriter(sb), categoriesDto, nameSpaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersWithProducts = context.Users
                .Include(x => x.ProductsSold)
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count(p => p.Buyer != null))
                .ToList();

            var usersWithProducts2 = new ExportUserCountWithUsers
            {
                Count = usersWithProducts.Count,
                Users = new UsersWithUserDto {
                    Users = usersWithProducts.Select(up => new ExportUserWithAgeDto
                {
                    FirstName = up.FirstName,
                    LastName = up.LastName,
                    Age = up.Age,
                    SoldProducts = new ExportSoldProductsWIthCount
                    {
                        Count = up.ProductsSold.ToArray().Length,
                        ProductDtos = new SoldProductsList { 
                            ProductDtos = up.ProductsSold.
                            Where(b => b.Buyer != null)
                            .OrderByDescending(p => p.Price)
                            .Select(ps => new SoldProductDto
                            {
                                Name = ps.Name,
                                Price = ps.Price
                            }).ToList()
                            }
                    }
                }).Take(10).ToList()
                }
                                   
            };

            XmlSerializer serializer = new XmlSerializer(typeof(ExportUserCountWithUsers), new XmlRootAttribute("Users"));
            var sb = new StringBuilder();
            var nameSpaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });


            serializer.Serialize(new StringWriter(sb), usersWithProducts2, nameSpaces);

            //var stringaf = sb.ToString().TrimEnd().Substring(139);
            //Console.WriteLine(stringaf);
            return sb.ToString().TrimEnd();
        }
    }
}