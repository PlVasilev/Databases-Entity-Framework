using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;
using ProductShop.Views.Dtos;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new ProductShopContext())
            {
               //context.Database.EnsureDeleted();
               //context.Database.EnsureCreated();

                string usersJason = File.ReadAllText(@"..\..\..\Datasets\users.json");
                string productsJason = File.ReadAllText(@"..\..\..\Datasets\products.json");
                string categoriesJason = File.ReadAllText(@"..\..\..\Datasets\categories.json");
                string categorieProductsJason = File.ReadAllText(@"..\..\..\Datasets\categories-products.json");

                // Console.WriteLine(ImportUsers(context,usersJason));
                // Console.WriteLine(ImportProducts(context, productsJason));
                // Console.WriteLine(ImportCategories(context, categoriesJason));
                // Console.WriteLine(ImportCategoryProducts(context, categorieProductsJason));
                //prob 5 Console.WriteLine(GetProductsInRange(context));
                //prob 6 Console.WriteLine(GetSoldProducts(context));
                //prob 7 Console.WriteLine(GetCategoriesByProductsCount(context));
                Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity); //context of the validation object
            List<ValidationResult> validationResults = new List<ValidationResult>(); // list of attributes to validate

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true); // true to validate all props

            return isValid;
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);
            List<User> validatedUsers = new List<User>();

            foreach (var user in users)
            {
                if (!IsValid(user))
                {
                    continue;
                }
                validatedUsers.Add(user);
            }

            context.Users.AddRange(validatedUsers);
            context.SaveChanges();

            return $"Successfully imported {validatedUsers.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);
            List<Product> validatedProcucts = new List<Product>();
            foreach (var product in products)
            {
                if (!IsValid(product))
                {
                    continue;
                }
                validatedProcucts.Add(product);
            }
            context.Products.AddRange(validatedProcucts);
            context.SaveChanges();

            return $"Successfully imported {validatedProcucts.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson);
            List<Category> validatedCategories = new List<Category>();
            foreach (var category in categories)
            {
                if (!IsValid(category))
                {
                    continue;
                }
                validatedCategories.Add(category);
            }
            context.Categories.AddRange(validatedCategories);
            context.SaveChanges();

            return $"Successfully imported {validatedCategories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var CategoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
            List<CategoryProduct> validatedCategoryProducts = new List<CategoryProduct>();
            foreach (var categoryProduct in CategoryProducts)
            {
                if (!IsValid(categoryProduct))
                {
                    continue;
                }
                validatedCategoryProducts.Add(categoryProduct);
            }
            context.CategoryProducts.AddRange(validatedCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validatedCategoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products.Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                    //p.Seller.FirstName == null ? p.Seller.LastName : $"{p.Seller.FirstName} {p.Seller.LastName}" 
                }).OrderBy(p => p.Price)
                .ToList();

            var json = JsonConvert.SerializeObject(productsInRange, Formatting.Indented);
            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            //DTOs
            var userSalers = context.Users
                .Include(i => i.ProductsSold)
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToArray();

            List<UserSalesDto> userSalesDtos = new List<UserSalesDto>();
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            for (int i = 0; i < userSalers.Length; i++)
            {
                var userDto = Mapper.Map<UserSalesDto>(userSalers[i]);
                var products = userSalers[i].ProductsSold.Where(p => p.Buyer != null)
                    .AsQueryable()
                    .ProjectTo<SoldProductDto>().ToArray();

                userDto.SoldProduct.AddRange(products);
                userSalesDtos.Add(userDto);
            }

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var serialized = JsonConvert.SerializeObject(userSalesDtos, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return serialized;

            //JSON
            var userSalersAnunimustJSON = context.Users
                .Include(i => i.ProductsSold)
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                   .Select(u => new
                   {
                       FirstName = u.FirstName,
                       LastName = u.LastName,
                       SoldProducts = u.ProductsSold
                           .Where(ps => ps.Buyer != null)
                           .Select(ps => new
                           {
                               Name = ps.Name,
                               Price = ps.Price,
                               BuyerFirstName = ps.Buyer.FirstName,
                               BuyerLastName = ps.Buyer.LastName,
                           }).ToArray()

                   })
                .ToArray();

            DefaultContractResolver contractResolver1 = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var serialized2 = JsonConvert.SerializeObject(userSalersAnunimustJSON, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return serialized2;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categoryProducts = context.Categories
                .OrderByDescending(x => x.CategoryProducts.Count)
                .Select(x => new
                {
                    category = x.Name,
                    productsCount = x.CategoryProducts.Count,
                    productsPrice = x.CategoryProducts
                        .Select(y => new
                        {
                            productPrice = y.Product.Price
                        }),
                }).Select(z => new
                {
                    z.category,
                    z.productsCount,
                    averagePrice = (z.productsPrice.AsQueryable().Sum(x => x.productPrice) / z.productsCount).ToString("0.00"),
                    totalRevenue = z.productsPrice.AsQueryable().Sum(x => x.productPrice).ToString()
                }).ToArray();
            ;
            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var serialized2 = JsonConvert.SerializeObject(categoryProducts, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return serialized2;

            var categories = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .Select(x => new
                {
                    Category = x.Name,
                    ProductsCount = x.CategoryProducts.Count,
                    AveragePrice = $"{x.CategoryProducts.Average(c => c.Product.Price):F2}",
                    TotalRevenue = $"{x.CategoryProducts.Sum(c => c.Product.Price)}"
                })
                .ToList();

            string json = JsonConvert.SerializeObject(categories,
                new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy(),
                    },

                    Formatting = Formatting.Indented
                }
            );

            return json;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
                       
            //whit Mapper
            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count(p => p.Buyer != null))
                .ProjectTo<UserDto>()
                .ToList();

            var objectToSerialize = Mapper.Map<UsersAndProductsDto>(users);

            string json = JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },

                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            });

            return json;

            var usersWithProducts = context.Users
                 .Include(x => x.ProductsSold)
                 .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                 .OrderByDescending(u => u.ProductsSold.Count(p => p.Buyer != null))
                 .ToList();

            var usersWithProducts2 = new
            {
                usersCount = usersWithProducts.Count,
                users = usersWithProducts.Select(up => new
                {
                    firstName = up.FirstName,
                    lastName = up.LastName,
                    age = up.Age,
                    soldProducts = new
                    {
                        count = up.ProductsSold.Count,
                        products = up.ProductsSold.Select(ps => new
                        {
                            name = ps.Name,
                            price = ps.Price
                        })
                    }
                })
            };

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var serialized = JsonConvert.SerializeObject(usersWithProducts2, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return serialized;
        }
    }
}