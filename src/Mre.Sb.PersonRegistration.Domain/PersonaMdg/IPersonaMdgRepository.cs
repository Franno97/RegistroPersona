using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Mre.Sb.RegistroPersona.Persona
{
    public interface IPersonaMdgRepository : IRepository<PersonaMdg.PersonaMdg, Guid>
    {
        Task<PersonaMdg.PersonaMdg> FindByRegisterNumberAsync(string registerNumber);
    }
}
