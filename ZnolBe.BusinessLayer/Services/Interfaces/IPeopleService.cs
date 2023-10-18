using ZnolBe.Shared.Models;

namespace ZnolBe.BusinessLayer.Services.Interfaces;
public interface IPeopleService
{
    Task<IEnumerable<Person>> GetListAsync(string name);
}