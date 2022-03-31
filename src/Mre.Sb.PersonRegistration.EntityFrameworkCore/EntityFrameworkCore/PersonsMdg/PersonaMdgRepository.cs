using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mre.Sb.RegistroPersona.Persona;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Mre.Sb.RegistroPersona.EntityFrameworkCore
{
    public class PersonaMdgRepository : EfCoreRepository<RegistroPersonaDbContext, PersonaMdg.PersonaMdg, Guid>, IPersonaMdgRepository
    {
        public PersonaMdgRepository(
            IDbContextProvider<RegistroPersonaDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
        public async Task<PersonaMdg.PersonaMdg> FindByRegisterNumberAsync(string registerNumber)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(person => person.NumeroRegistro == registerNumber);
        }
    }
}
