using System.Data;
using ZnolBe.DataAccessLayer;
using Entities = ZnolBe.DataAccessLayer.Entities;
using Person = ZnolBe.Shared.Models.Person;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ZnolBe.BusinessLayer.Services.Interfaces;
using ZnolBe.Shared.Models.Requests;

namespace ZnolBe.BusinessLayer.Services;
public class PeopleService : IPeopleService
{
    private readonly IDataContext dataContext;
    private readonly IMapper mapper;

    public PeopleService(IDataContext datacontext, IMapper mapper)
    {
        this.dataContext = datacontext;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<Person>> GetListAsync(string name)
    {
        var query = dataContext.GetData<Entities.Person>();
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(p => p.Name.Contains(name));
        }

        //var people = mapper.Map<IEnumerable<Person>>(dbPeople);
        var people = await query.OrderBy(p => p.Name)
            .ProjectTo<Person>(mapper.ConfigurationProvider)
            .ToListAsync();

        return people;
    }
}
