using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PetClinic.DataProcessor.Import;
using PetClinic.Models;

namespace PetClinic.DataProcessor
{
    using System;

    using PetClinic.Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Error: Invalid data.";

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            var dtos = JsonConvert.DeserializeObject<AnimalAidsDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<AnimalAid> animalAids = new List<AnimalAid>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var animalAid = new AnimalAid
                {
                    Name = dto.Name,
                    Price = dto.Price
                };
                var animalAidCheck = animalAids.FirstOrDefault(x => x.Name == dto.Name);

                if (animalAidCheck != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                animalAids.Add(animalAid);
                sb.AppendLine($"Record {animalAid.Name} successfully imported.");
            }
            context.AnimalAids.AddRange(animalAids);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            var dtos = JsonConvert.DeserializeObject<AnimalsDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Animal> animals = new List<Animal>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto) || !IsValid(dto.Passport))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var animal = new Animal
                {
                    Name = dto.Name,
                    Type = dto.Type,
                    Age = dto.Age,
                    Passport = new Passport
                    {
                        SerialNumber = dto.Passport.SerialNumber,
                        OwnerPhoneNumber = dto.Passport.OwnerPhoneNumber,
                        OwnerName = dto.Passport.OwnerName,
                        RegistrationDate = DateTime.ParseExact(dto.Passport.RegistrationDate, "dd-MM-yyyy",
                            CultureInfo.InvariantCulture)
                    }
                };
                var validatedAnimal =
                    animals.FirstOrDefault(x => x.Passport.SerialNumber == dto.Passport.SerialNumber);
                if (validatedAnimal != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                animals.Add(animal);
                sb.AppendLine($"Record {animal.Name} Passport №: {animal.Passport.SerialNumber} successfully imported.");
            }
            context.Animals.AddRange(animals);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(VetsDto[]), new XmlRootAttribute("Vets"));
            var deserialized = (VetsDto[])serializer.Deserialize(new StringReader(xmlString));
            var vets = new List<Vet>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in deserialized)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var vet = new Vet
                {
                    Name = dto.Name,
                    Profession = dto.Profession,
                    Age = dto.Age,
                    PhoneNumber = dto.PhoneNumber
                };
                var validatedVet = vets.FirstOrDefault(x => x.PhoneNumber == dto.PhoneNumber);
                if (validatedVet != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                sb.AppendLine($"Record {vet.Name} successfully imported.");
                vets.Add(vet);
            }

            context.Vets.AddRange(vets);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            //var serialize = new XmlSerializer(typeof(ProceduresDto[]),
            //    new XmlRootAttribute("Procedures"));
            //
            //var procedures1 = (ProceduresDto[])serialize.Deserialize(
            //    new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));
            //
            //var validProcedures = new List<Procedure>();
            //
            //var sb1 = new StringBuilder();
            //
            //foreach (var procedureDto in procedures1)
            //{
            //    if (!IsValid(procedureDto))
            //    {
            //        sb1.AppendLine("Error: Invalid data.");
            //        continue;
            //    }
            //
            //    var vet = context.Vets
            //        .SingleOrDefault(v => v.Name == procedureDto.Vet);
            //
            //    var animal = context.Animals
            //        .SingleOrDefault(a => a.PassportSerialNumber == procedureDto.Animal);
            //
            //    var dateTime = DateTime.ParseExact(procedureDto.DateTime,
            //        "dd-MM-yyyy", CultureInfo.InvariantCulture);
            //
            //    bool animalAidsInvalid = false;
            //
            //    var procedureAnimalAids = new List<ProcedureAnimalAid>();
            //
            //    foreach (var animalAidDto in procedureDto.AnimalAids)
            //    {
            //        var animalAid = context.AnimalAids
            //            .SingleOrDefault(a => a.Name == animalAidDto.Name);
            //
            //        if (animalAid == null
            //            || procedureAnimalAids.Any(p => p.AnimalAid == animalAid))
            //        {
            //            animalAidsInvalid = true;
            //            break;
            //        }
            //
            //        var procedureAnimalAid = new ProcedureAnimalAid
            //        {
            //            AnimalAid = animalAid
            //        };
            //
            //        procedureAnimalAids.Add(procedureAnimalAid);
            //    }
            //
            //    if (vet == null || animal == null || animalAidsInvalid)
            //    {
            //        sb1.AppendLine("Error: Invalid data.");
            //        continue;
            //    }
            //
            //    var procedure = new Procedure
            //    {
            //        Animal = animal,
            //        DateTime = dateTime,
            //        Vet = vet,
            //        ProcedureAnimalAids = procedureAnimalAids
            //    };
            //
            //    validProcedures.Add(procedure);
            //    sb1.AppendLine("Record successfully imported.");
            //}
            //
            //context.Procedures.AddRange(validProcedures);
            //context.SaveChanges();
            //
            //return sb1.ToString();

            var serializer = new XmlSerializer(typeof(ProceduresDto[]), new XmlRootAttribute("Procedures"));
            var deserialized = (ProceduresDto[])serializer.Deserialize(new StringReader(xmlString));
            var procedures = new List<Procedure>();
            StringBuilder sb = new StringBuilder();
            
            foreach (var dto in deserialized)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            
                bool isValidated = true;
            
                var vet = context.Vets.FirstOrDefault(x => x.Name == dto.Vet);
                var animal = context.Animals.FirstOrDefault(x => x.PassportSerialNumber == dto.Animal);
                
                var procedure = new Procedure
                {
                    Animal = animal,
                    Vet = vet,
                    DateTime = DateTime.ParseExact(dto.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                };

                foreach (var animalAidDto in dto.AnimalAids)
                {
                    var animalAid = context.AnimalAids.FirstOrDefault(x => x.Name == animalAidDto.Name); 
                    
                    if (animalAid == null || procedure.ProcedureAnimalAids.Any(p => p.AnimalAid == animalAid))
                    { // check if contains with any
                        isValidated = false;
                        break;
                    }

                    var procedureAnimalAid = new ProcedureAnimalAid
                    {
                        //Procedure = procedure, works without it
                        AnimalAid = animalAid
                    };

                    procedure.ProcedureAnimalAids.Add(procedureAnimalAid);
                }
                if (isValidated == false || vet == null || animal == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

           

                procedures.Add(procedure);
                sb.AppendLine("Record successfully imported.");                
            }
            context.Procedures.AddRange(procedures);
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
