using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mre.Sb.RegistroPersona.EntityFrameworkCore
{
    public class PersonRegistrationModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public PersonRegistrationModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}