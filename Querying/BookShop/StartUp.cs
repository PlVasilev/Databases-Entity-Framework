using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookShop.Models;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                DbInitializer.ResetDatabase(db);

                //prob 1  string command = Console.ReadLine(); Console.WriteLine(GetBooksByAgeRestriction(db, command));
                //prob 2  Console.WriteLine(GetGoldenBooks(db
                //prob 2  Console.WriteLine(GetBooksByPrice(db));
                //prob 4  int command = int.Parse(Console.ReadLine()); Console.WriteLine(GetBooksNotReleasedIn(db, command));
                //prob 5  string command = Console.ReadLine(); Console.WriteLine(GetBooksByCategory(db, command));
                //prob 6  string command = Console.ReadLine(); Console.WriteLine(GetBooksReleasedBefore(db, command));
                //prob 7
                string command = Console.ReadLine(); Console.WriteLine(GetAuthorNamesEndingIn(db, command));
                //prob 8  string command = Console.ReadLine(); Console.WriteLine(GetBookTitlesContaining(db, command));
                //prob 9  string command = Console.ReadLine(); Console.WriteLine(GetBooksByAuthor(db, command));
                //prob 10 int command = int.Parse(Console.ReadLine()); Console.WriteLine(CountBooks(db, command));
                //prop 11 Console.WriteLine(CountCopiesByAuthor(db));
                //prob 12 Console.WriteLine(GetTotalProfitByCategory(db));
                //prob 13 Console.WriteLine(GetMostRecentBooks(db));
                //prob 14 IncreasePrices(db);
                //prob 15 Console.WriteLine(RemoveBooks(db));
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            context.Books
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .OrderBy(b => b.Title)
                .ToList()
                .ForEach(b => sb.AppendLine(b.Title));

            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(b => b.Title)
                .ToList()
                .ForEach(x => sb.AppendLine(x));

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList()
                .ForEach(x => sb.AppendLine($"{x.Title} - ${x.Price:f2}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();

            context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList()
                .ForEach(x => sb.AppendLine(x));

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();
            string[] categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            List<string> titles = new List<string>();
            foreach (var category in categories)
            {
                titles.AddRange(context.BooksCategories
                    .Where(bc => bc.Category.Name.ToLower() == category.ToLower())
                    .Select(b => b.Book.Title)
                    .ToList());
            }
            foreach (var title in titles.OrderBy(t => t))
            {
                sb.AppendLine(title);
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            context.Books.Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", null))
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                }).ToList()
                .ForEach(b => sb.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:f2}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            context.Books
                .Where(b => b.Author.FirstName.EndsWith(input))
                .Select(a => a.Author.FirstName + " " + a.Author.LastName)
                .GroupBy(x => x)
                .Select(g => g.First())
                .OrderBy(a => a).ToList()
                .ForEach(a => sb.AppendLine(a));

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(t => t.Title)
                .OrderBy(t => t)
                .ToList()
                .ForEach(t => sb.AppendLine(t));

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(t => t.BookId)
                .Select(t => $"{t.Title} ({t.Author.FirstName} {t.Author.LastName})")
                .ToList()
                .ForEach(t => sb.AppendLine(t));

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Count(b => b.Title.Length > lengthCheck);
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Authors.
                Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    Copies = a.Books.Select(b => b.Copies).Sum()
                }).OrderByDescending(a => a.Copies)
                .ToList()
                .ForEach(a => sb.AppendLine($"{a.FirstName} {a.LastName} - {a.Copies}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            context.Categories
                .Select(c => new
                {
                    c.Name,
                    Profit = c.CategoryBooks.Select(b => b.Book.Copies * b.Book.Price).Sum()
                }).OrderByDescending(c => c.Profit)
                .ToList()
                .ForEach(c => sb.AppendLine($"{c.Name} ${c.Profit:f2}"));

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var category in context.Categories.OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    TopThreeBooks = c.CategoryBooks.Select(b => b.Book).OrderByDescending(b => b.ReleaseDate).Take(3)
                }).ToList())
            {
                sb.AppendLine($"--{category.Name}");
                foreach (var book in category.TopThreeBooks)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            foreach (var book in context.Books.Where(b => b.ReleaseDate.Value.Year < 2010))
            {
                book.Price += 5;
            }
            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksToRemove = context.Books.Where(b => b.Copies < 4200).ToList();
            context.Books.RemoveRange(booksToRemove);
            context.SaveChanges();
            return booksToRemove.Count;
        }
    }
}
