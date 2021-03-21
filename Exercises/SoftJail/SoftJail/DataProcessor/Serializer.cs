using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace SoftJail.DataProcessor
{

    using Data;
    using System;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners.Where(p => ids.Contains(p.Id))
                .Include(op => op.PrisonerOfficers)
                .ThenInclude(o => o.Officer)
                .ThenInclude(d => d.Department)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.FullName,
                    CellNumber = x.Cell.CellNumber,
                    Officers = x.PrisonerOfficers
                        .Select(o => new
                        {
                            OfficerName = o.Officer.FullName,
                            Department = o.Officer.Department.Name
                        }).OrderBy(o => o.OfficerName).ToArray(),
                    TotalOfficerSalary = x.PrisonerOfficers.Sum(po => po.Officer.Salary)
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();

            var jsonResult = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return jsonResult;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisonersNamesArr = prisonersNames.Split(",", StringSplitOptions.RemoveEmptyEntries);

            var prisoners = context.Prisoners.Where(p => prisonersNamesArr.Contains(p.FullName))
                .Select(p => new PrisonersInboxDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd"),
                    EncryptedMessages = p.Mails.Select(m => new EncryptedMessages
                    {
                        Description = Reverce(m.Description)
                    }).ToArray()
                }).OrderBy(p => p.Name).ThenBy(p => p.Id).ToArray();

            var xmlSerializer = new XmlSerializer(typeof(PrisonersInboxDto[]), new XmlRootAttribute("Prisoners"));
            var namespaces = new XmlSerializerNamespaces(new[]
            {
                XmlQualifiedName.Empty,
            });

            var sb = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(sb), prisoners, namespaces);

            var result = sb.ToString().TrimEnd();

            return result;
        }

        private static string Reverce(string argDescription)
        {
            string result = string.Empty;
            for (int i = argDescription.Length - 1; i >= 0; i--)
            {
                result += argDescription[i];
            }

            return result;
        }
    }
}