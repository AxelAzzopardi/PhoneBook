using AutoMapper;
using PhoneBookTask.Dtos;
using PhoneBookTask.Models;

namespace PhoneBookTask.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Company, DisplayCompanyDto>()
                .ForMember(dest => dest.NumberOfPeople, opt => opt.MapFrom(src => src.Persons.Count));

            CreateMap<Person, DisplayPersonDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.CompanyName));
        }
    }
}