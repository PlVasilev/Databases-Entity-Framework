using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetClinic.DataProcessor.Import
{
   public class AnimalsDto
    {
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }

        [MinLength(3), MaxLength(20)]
        public string Type { get; set; }

        [Range(1, int.MaxValue)]
        public int Age { get; set; }

        [Required]
        public PassportDto Passport { get; set; }
    }

    public class PassportDto
    {
        [RegularExpression("^[A-Za-z]{7}[0-9]{3}$")]
        public string SerialNumber { get; set; }

        [MinLength(3), MaxLength(30)]
        public string OwnerName { get; set; }

        [RegularExpression(@"^\+359[0-9]{9}$|^0[0-9]{9}$")]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        public string RegistrationDate { get; set; }
    }
    //"Name":"Bella",
    //"Type":"cat",
    //"Age": 2,
    //"Passport": {
    //"SerialNumber": "etyhGgH678",
    //"OwnerName": "Sheldon Cooper",
    //"OwnerPhoneNumber": "0897556446",
    //"RegistrationDate": "12-03-2014"

}
