using AutoMapper;
using ZnolBe.Shared.Models;
using Entities = ZnolBe.DataAccessLayer.Entities;

namespace ZnolBe.BusinessLayer.MapperProfile;
public class PersonMapperProfile: Profile
{
    public PersonMapperProfile()
    {
        CreateMap<Entities.Person, Person>();
    }
}
