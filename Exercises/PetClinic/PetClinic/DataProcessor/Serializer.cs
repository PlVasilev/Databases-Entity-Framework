using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PetClinic.DataProcessor.Export;
using Formatting = Newtonsoft.Json.Formatting;

namespace PetClinic.DataProcessor
{
    using System;

    using PetClinic.Data;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var animalsByOwnerPhoneNumber = context.Animals.Where(x => x.Passport.OwnerPhoneNumber == phoneNumber)
                .Select(x => new
                {
                    OwnerName = x.Passport.OwnerName,
                    AnimalName = x.Name,
                    Age = x.Age,
                    SerialNumber = x.PassportSerialNumber,
                    RegisteredOn = x.Passport.RegistrationDate.ToString("dd-MM-yyyy")
                }).OrderBy(x => x.Age).ThenBy(x => x.SerialNumber).ToArray();


            var jsonResult = JsonConvert.SerializeObject(animalsByOwnerPhoneNumber, Formatting.Indented);

            return jsonResult;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            var serialized = context.Procedures.Select(x => new
                {
                    Passport = x.Animal.Passport.SerialNumber,
                    OwnerNumber = x.Animal.Passport.OwnerPhoneNumber,
                    DateTime = x.DateTime,
                    AnimalAids = x.ProcedureAnimalAids.Select(y => new AnimalAidDtoExport
                    {
                        Name = y.AnimalAid.Name,
                        Price = y.AnimalAid.Price
                    }).ToArray(),
                    TotalPrice = x.ProcedureAnimalAids.Sum(p => p.AnimalAid.Price)
                }).OrderBy(x => x.DateTime)
                .ThenBy(p => p.Passport).Select(m => new AllProcedures
                {
                    Passport = m.Passport,
                    OwnerNumber = m.OwnerNumber,
                    DateTime = m.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    AnimalAids = m.AnimalAids,
                    TotalPrice = m.TotalPrice
                }).ToArray();
          
            var xmlSerializer = new XmlSerializer(typeof(AllProcedures[]), new XmlRootAttribute("Procedures"));
            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty,
            });

            var sb = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(sb), serialized, namespaces);

            var result = sb.ToString().TrimEnd();

            return result;
        }
    }
}
