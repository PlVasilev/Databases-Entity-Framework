using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Cinema.Data.Models;
using Cinema.Data.Models.Enums;
using Cinema.DataProcessor.ImportDto;
using Newtonsoft.Json;

namespace Cinema.DataProcessor
{
    using System;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var deserialized = JsonConvert.DeserializeObject<ImportMoviesDto[]>(jsonString);
            var sb = new StringBuilder();
            List<Movie> movies = new List<Movie>();
            List<string> titles = new List<string>();

            foreach (var dto in deserialized)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                TimeSpan ts = TimeSpan.Parse(dto.Duration);

                var isValidEnum = Enum.TryParse<Genre>(dto.Genre, out Genre genre);

                if (!isValidEnum)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (titles.Contains(dto.Title))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = new Movie()
                {
                    Title = dto.Title,
                    Genre = genre,
                    Duration = ts,
                    Rating = dto.Rating,
                    Director = dto.Director
                };
                titles.Add(dto.Title);
                movies.Add(movie);
                sb.AppendLine(string.Format(SuccessfulImportMovie, dto.Title, dto.Genre, movie.Rating.ToString("0.00")));
            }

            context.Movies.AddRange(movies);
            context.SaveChanges();
            string result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var deserialized = JsonConvert.DeserializeObject<HallSeatsDto[]>(jsonString);
            var sb = new StringBuilder();
            List<Hall> halls = new List<Hall>();
            //List<string> hallNames = new List<string>();

            foreach (var dto in deserialized)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = new Hall()
                {
                    Name = dto.Name,
                    Is4Dx = dto.Is4Dx,
                    Is3D = dto.Is3D
                };

                for (int i = 0; i < dto.Seats; i++)
                {
                    var seat = new Seat
                    {
                        Hall = hall
                    };
                    hall.Seats.Add(seat);
                }

                string type = "";
                if (dto.Is4Dx == true && dto.Is3D == true)
                {
                    type = "4Dx/3D";
                }
                else if (dto.Is4Dx == true && dto.Is3D == false)
                {
                    type = "4Dx";
                }
                else if (dto.Is4Dx == false && dto.Is3D == true)
                {
                    type = "3D";
                }
                else
                {
                    type = "Normal";
                }
                halls.Add(hall);
                sb.AppendLine(string.Format(SuccessfulImportHallSeat, dto.Name, type, dto.Seats));
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();
            string result = sb.ToString().TrimEnd();
            return result;
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProjectionDto[]), new XmlRootAttribute("Projections"));
            var deserialized = (ProjectionDto[])serializer.Deserialize(new StringReader(xmlString));
            var projections = new List<Projection>();
            StringBuilder sb = new StringBuilder();
            
            foreach (var dto in deserialized)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = context.Movies.FirstOrDefault(x => x.Id == dto.MovieId);
                var hall = context.Halls.FirstOrDefault(x => x.Id == dto.HallId);

                if (movie == null || hall == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime dt = DateTime.ParseExact(dto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                var projection = new Projection()
                {
                    MovieId = dto.MovieId,
                    HallId = dto.HallId,
                    DateTime = dt
                };
                projections.Add(projection);
                sb.AppendLine(string.Format(SuccessfulImportProjection,movie.Title,projection.DateTime.ToString("MM/dd/yyyy")));
            }

            context.Projections.AddRange(projections);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(CustomerTicketDto[]), new XmlRootAttribute("Customers"));
            var deserialized = (CustomerTicketDto[]) serializer.Deserialize(new StringReader(xmlString));
            var customers = new List<Customer>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in deserialized)
            {
                if (!IsValid(dto) || !dto.Tickets.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var customer = new Customer()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age,
                    Balance = dto.Balance
                };

                bool isValidated = true;

                foreach (var dtoTicket in dto.Tickets)
                {
                    var projection = context.Projections.FirstOrDefault(x => x.Id == dtoTicket.ProjectionId);
                    if (projection == null)
                    {
                        isValidated = false;
                        break;
                    }
                    Ticket ticket = new Ticket()
                    {
                        Customer = customer,
                        Projection = projection,
                        Price = dtoTicket.Price
                    };
                    customer.Tickets.Add(ticket);
                }
                if (isValidated == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                customers.Add(customer);
                sb.AppendLine(string.Format(SuccessfulImportCustomerTicket,customer.FirstName,customer.LastName,customer.Tickets.Count));
            }

            context.Customers.AddRange(customers);
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