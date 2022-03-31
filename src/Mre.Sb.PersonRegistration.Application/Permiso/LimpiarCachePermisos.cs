using Microsoft.Extensions.Logging;
using Mre.Sb.Base.Permiso;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;

namespace Mre.Sb.PersonRegistration.Permiso
{
    public class LimpiarCachePermisos : IDistributedEventHandler<EntityCreatedEto<PermissionGrantEto>>
          , IDistributedEventHandler<EntityDeletedEto<PermissionGrantEto>>
          , IDistributedEventHandler<EntityUpdatedEto<PermissionGrantEto>>
          , ITransientDependency
    {

        protected ICurrentTenant CurrentTenant { get; }
        protected ILogger<LimpiarCachePermisos> Logger { get; }

        protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

        public LimpiarCachePermisos(
            IDistributedCache<PermissionGrantCacheItem> cache,
            ICurrentTenant currentTenant,
            ILogger<LimpiarCachePermisos> logger)
        {
            Cache = cache;
            CurrentTenant = currentTenant;
            Logger = logger;
        }


        public async Task HandleEventAsync(EntityCreatedEto<PermissionGrantEto> eventData)
        {
            Logger.LogDebug("Evento EntityCreatedEto recibido. Entidad: {0}. Id: {1}",
                nameof(PermissionGrantEto),
                    $"{eventData.Entity.Name}+{eventData.Entity.ProviderKey}");

            await LimpiarAsync(eventData.Entity);

        }



        public async Task HandleEventAsync(EntityDeletedEto<PermissionGrantEto> eventData)
        {
            Logger.LogDebug("Evento EntityDeletedEto recibido. Entidad: {0}. Id: {1}",
               nameof(PermissionGrantEto),
                   $"{eventData.Entity.Name}+{eventData.Entity.ProviderKey}");

            await LimpiarAsync(eventData.Entity);
        }

        public async Task HandleEventAsync(EntityUpdatedEto<PermissionGrantEto> eventData)
        {
            Logger.LogDebug("Evento EntityUpdatedEto recibido. Entidad: {0}. Id: {1}",
              nameof(PermissionGrantEto),
                  $"{eventData.Entity.Name}+{eventData.Entity.ProviderKey}");

            await LimpiarAsync(eventData.Entity);
        }


        protected virtual async Task LimpiarAsync(PermissionGrantEto eventData)
        {

            var cacheKey = CalculateCacheKey(
                  eventData.Name,
                  eventData.ProviderName,
                  eventData.ProviderKey
                );


            using (CurrentTenant.Change(eventData.TenantId))
            {
                await Cache.RemoveAsync(cacheKey);
            }
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
