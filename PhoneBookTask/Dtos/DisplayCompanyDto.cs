using System;

namespace PhoneBookTask.Dtos
{
    public class DisplayCompanyDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }

        public DateTime RegistrationDate { get; set; }
        public int NumberOfPeople { get; set; }
    }
}