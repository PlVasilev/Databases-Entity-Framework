using System;
using System.Collections.Generic;
using AutoMapper;
using CustomAutoMapper;
using Newtonsoft.Json;
using Mapper = AutoMapper.Mapper;

namespace AutomapperLab
{
    class StartUp
    {
        static void Main(string[] args)
        {
            //By Convention THE mapped prop names must be the SAME
            Person person = new Person()
            {
                FirstName = "Peter",
                LastName = "Petrov",
                Age = 20,
                StringCourses = new List<string>()
                {
                    "DBstring", "Asp.NETstring"                        
                },
                ClassCourses = new List<PersonCourse>()
                {
                    new PersonCourse(){Name = "DB"},
                    new PersonCourse(){Name = "Asp.NET"}
                }
            };

            //Without mapper 
            //Student student = new Student()
            //{
            //    FirstName = person.FirstName,
            //    LastName = person.LastName
            //};

            //With Mapper
            Mapper.Initialize(cfg => //works as Fluent API
            {
                cfg.CreateMap<Person, Student>() 
                    .ForMember(dest => dest.FullName,
                        opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")) //Custom Map added with Options and Sores
                    .ForMember(dest => dest.FirstName, opt => opt.Ignore()) //ignoring FirstName prop (=null) (we can ignore salary)
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Age.ToString())); // in name we put age as string
                cfg.CreateMap<PersonCourse, StudentCourse>(); //we can add more couples of more mapped objects
                //ForMember(dest => dest.FirstName [GET THIS], opt => opt.MapFrom(src => src.Age.ToString()[FROM THIS])); !!
            });

            var student = Mapper.Map<Student>(person); //Create new object of type Student with persons props

            Console.WriteLine(JsonConvert.SerializeObject(student));
            Console.WriteLine();
            // Automapper in LINQ with Database using conext and its DBSets
            //      var persons = Contex.Students  //using automapper.extensions
            //          .ProjectTo<Person>() 
            //          .ToList();

            var customMapper = new CustomAutoMapper.Mapper();

            var studentCust = customMapper.Map<Student>(person);
            Console.WriteLine(JsonConvert.SerializeObject(studentCust));
        }
    }
}
