using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json;
using SoftJail.Data;
using SoftJail.Data.Models;
using SoftJail.Data.Models.Enums;
using SoftJail.DataProcessor.ImportDto;

namespace SoftJail.DataProcessor
{
    
    using Data;
    using System;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {

            var departmentDto = JsonConvert.DeserializeObject<ImportDepartmentCellDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Department> departments = new List<Department>();
            bool isvalidated;

            foreach (var dto in departmentDto)
            {
                isvalidated = true;
                if (!IsValid(dto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var department = new Department
                {
                    Name = dto.Name
                };

                foreach (var cellDto in dto.Cells)
                {
                    if (!IsValid(cellDto))
                    {
                        isvalidated = false;
                        break;
                    }
                    else
                    {
                        var cell = new Cell
                        {
                            CellNumber = cellDto.CellNumber,
                            HasWindow = cellDto.HasWindow,
                            Department = department
                        };
                        department.Cells.Add(cell);
                    }
                }

                if (isvalidated == false)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
                departments.Add(department);

            }
            context.Departments.AddRange(departments);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var importPrisonersMailsDto = JsonConvert.DeserializeObject<ImportPrisonerMailDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Prisoner> prisoners = new List<Prisoner>();
            bool isValidated = true;

            foreach (var dto in importPrisonersMailsDto)
            {
                if (!IsValid(dto) || !dto.Mails.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                var prisoner = new Prisoner
                {
                    FullName = dto.FullName,
                    Nickname = dto.Nickname,
                    Age = dto.Age,
                    IncarcerationDate = DateTime.ParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = DateTime.ParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Bail = dto.Bail,
                    CellId = dto.CellId,
                };

                foreach (var dtoMail in dto.Mails)
                {
                    var mail = new Mail
                    {
                        Description = dtoMail.Description,
                        Sender = dtoMail.Sender,
                        Address = dtoMail.Address,
                        Prisoner = prisoner
                    };
                    prisoner.Mails.Add(mail);
                }

                prisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ImportOfficerPrisoners[]), new XmlRootAttribute("Officers"));
            var deserialized = (ImportOfficerPrisoners[])serializer.Deserialize(new StringReader(xmlString));

            var officersPrisoners = new List<Officer>();

            StringBuilder sb = new StringBuilder();

            foreach (var dto in deserialized)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Weapon weapon;
                if (!Enum.TryParse(dto.Weapon, true, out weapon))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Position position;
                if (!Enum.TryParse(dto.Position, true, out position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = dto.Name,
                    Salary = dto.Salary,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = dto.DepartmentId,
                };

                foreach (var dtoPrisoner in dto.PrisonerDtp)
                {
                    if (!IsValid(dtoPrisoner))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var prisoner = new OfficerPrisoner()
                    {
                        OfficerId = officer.Id,
                        PrisonerId = dtoPrisoner.Id
                    };

                    officer.OfficerPrisoners.Add(prisoner);
                }

                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
                officersPrisoners.Add(officer);
            }

            context.Officers.AddRange(officersPrisoners);
            context.SaveChanges();

            string result = sb.ToString();
            return result;

            //var importOfficerPrisonersDtos = new XmlSerializer(typeof(ImportOfficerPrisoners[]), new XmlRootAttribute("Officers"));
            //var officerPrisonersDtos = (ImportOfficerPrisoners[])importOfficerPrisonersDtos.Deserialize(new StringReader(xmlString));
            //var sb = new StringBuilder();
            //var officers = new List<Officer>();
            //
            //foreach (var dto in officerPrisonersDtos)
            //{
            //   if (!IsValid(dto))
            //    {
            //        sb.AppendLine("Invalid Data");
            //        continue;
            //    }
            //
            //    var isValidEnumPostion = Enum.TryParse<Position>(dto.Position, out Position position);
            //    var isValidEnumWeapon = Enum.TryParse<Weapon>(dto.Weapon, out Weapon weapon);
            //
            //    if (!isValidEnumPostion || !isValidEnumWeapon)
            //    {
            //        sb.AppendLine("Invalid Data");
            //        continue;
            //    }
            //
            //    var department = context.Departments.FirstOrDefault(x => x.Id == dto.DepartmentId);
            //    if (department == null)
            //    {
            //        sb.AppendLine("Invalid Data");
            //        continue;
            //    }
            //
            //    var officer = new Officer
            //    {
            //        FullName = dto.Name,
            //        Salary = dto.Salary,
            //        Position = position,
            //        Weapon = weapon,
            //        Department = department,
            //    };
            //
            //    bool isValidPrisoner = true;
            //
            //    var officerPrisoners = new List<OfficerPrisoner>();
            //    foreach (var prisonerId in dto.PrisonerDtp)
            //    {
            //        var prisoner = context.Prisoners.FirstOrDefault(x => x.Id == prisonerId.Id);
            //        if (prisoner == null)
            //        {
            //            isValidPrisoner = false;
            //            break;
            //        }
            //        var officerPrisoner = new OfficerPrisoner
            //        {
            //            Prisoner = prisoner,
            //            Officer = officer
            //
            //        };
            //        officerPrisoners.Add(officerPrisoner);
            //
            //        context.OfficersPrisoners.Add(officerPrisoner);
            //    }
            //
            //    if (isValidPrisoner == false)
            //    {
            //        sb.AppendLine("Invalid Data");
            //        continue;
            //    }
            //    officers.Add(officer);
            //    sb.AppendLine($"Imported {officer.FullName} ({officerPrisoners.Count} prisoners)");
            //}
            //
            //context.Officers.AddRange(officers);
            //context.SaveChanges();
            //
            //return sb.ToString().TrimEnd();

            //XmlSerializer serializer = new XmlSerializer(typeof(ImportOfficerPrisoners[]), new XmlRootAttribute("Officers"));
            //ImportOfficerPrisoners[] officerDtos = (ImportOfficerPrisoners[])serializer.Deserialize(new StringReader(xmlString));
            //StringBuilder sb = new StringBuilder();

            //foreach (var dto in officerDtos)
            //{
            //    if (!IsValid(dto))
            //    {
            //        sb.AppendLine(InvalidData);
            //        continue;
            //    }

            //    Position position;
            //    if (!Enum.TryParse<Position>(dto.Position, out position))
            //    {
            //        sb.AppendLine(InvalidData);
            //        continue;
            //    }

            //    Weapon weapon;
            //    if (!Enum.TryParse<Weapon>(dto.Weapon, out weapon))
            //    {
            //        sb.AppendLine(InvalidData);
            //        continue;
            //    }

            //    bool isValidPrisoners = true;
            //    foreach (var dtoPrisoner in dto.PrisonerDtp)
            //    {
            //        if (!context.Prisoners.Any(p => p.Id == dtoPrisoner.Id))
            //        {
            //            isValidPrisoners = false;
            //            break;
            //        }
            //    }

            //    if (!isValidPrisoners)
            //    {
            //        sb.AppendLine(InvalidData);
            //        continue;
            //    }

            //    Officer officer = new Officer()
            //    {
            //        FullName = dto.Name,
            //        Salary = dto.Salary,
            //        Position = position,
            //        Weapon = weapon,
            //        DepartmentId = dto.DepartmentId
            //    };

            //    context.Officers.Add(officer);
            //    context.SaveChanges();
            //    sb.AppendLine($"Imported {dto.Name} ({dto.PrisonerDtp.Length} prisoners)");

            //    List<OfficerPrisoner> officerPrisoners = new List<OfficerPrisoner>();
            //    foreach (var dtoPrisoner in dto.PrisonerDtp)
            //    {
            //        OfficerPrisoner officerPrisoner = new OfficerPrisoner()
            //        {
            //            OfficerId = officer.Id,
            //            PrisonerId = dtoPrisoner.Id
            //        };

            //        officerPrisoners.Add(officerPrisoner);
            //    }

            //    context.OfficersPrisoners.AddRange(officerPrisoners);
            //    context.SaveChanges();
            //}
            //return sb.ToString().Trim();
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