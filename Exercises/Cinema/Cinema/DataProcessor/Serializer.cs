using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Cinema.DataProcessor.ExportDto;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Cinema.DataProcessor
{
    using System;

    using Data;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies.Where(x => x.Rating >= rating && x.Projections.Any())
                .OrderByDescending(m => m.Rating)
                .ThenByDescending(ti => ti.Projections.Sum(p => p.Tickets.Sum(t => t.Price)))
                .Select(m => new
                {
                    MovieName = m.Title,
                    Rating = m.Rating.ToString("0.00"),
                    TotalIncomes = m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("0.00"),
                    Customers = m.Projections.SelectMany(p => p.Tickets).Distinct()
                        .Select(t => new
                        {
                            FirstName = t.Customer.FirstName,
                            LastName = t.Customer.LastName,
                            Balance = t.Customer.Balance.ToString("0.00")
                        }).OrderByDescending(x => x.Balance).ThenBy(x => x.FirstName).ThenBy(x => x.LastName)
                }).Take(10).ToArray();


            var json = JsonConvert.SerializeObject(movies, Formatting.Indented);
            return json;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var topCutomers = context.Customers.Where(c => c.Age >= age)
                .OrderByDescending(c => c.Tickets.Sum(x => x.Price))
                .Select(c => new TopCustomersDto
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    SpentMoney = c.Tickets.Sum(x => x.Price).ToString("0.00"),
                    SpentTime = string.Format("{0:00}:{1:00}:{2:00}",
                        TimeSpan.FromTicks(c.Tickets.Sum(p => p.Projection.Movie.Duration.Ticks)).Hours,
                        TimeSpan.FromTicks(c.Tickets.Sum(p => p.Projection.Movie.Duration.Ticks)).Minutes,
                            TimeSpan.FromTicks(c.Tickets.Sum(p => p.Projection.Movie.Duration.Ticks)).Seconds)
                })
                .Take(10)
                .ToArray();


            var xmlSerializer = new XmlSerializer(typeof(TopCustomersDto[]), new XmlRootAttribute("Customers"));
            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty,
            });

            var sb = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(sb), topCutomers, namespaces);

            var result = sb.ToString().TrimEnd();

            return result;
        }
    }
}