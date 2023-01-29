using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneBookTask.Models
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; } 

        [DataType(DataType.Date)]
        [Required]
        public DateTime RegistrationDate { get; set; }

        public virtual List<Person> Persons { get; set; }
    }
}