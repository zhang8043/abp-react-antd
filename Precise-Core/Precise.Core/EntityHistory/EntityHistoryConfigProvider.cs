using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;
using Abp.Configuration.Startup;

namespace Precise.EntityHistory
{
    public class EntityHistoryConfigProvider : ICustomConfigProvider
    {
        private readonly IAbpStartupConfiguration _abpStartupConfiguration;

        public EntityHistoryConfigProvider(IAbpStartupConfiguration abpStartupConfiguration)
        {
            _abpStartupConfiguration = abpStartupConfiguration;
        }

        public Dictionary<string, object> GetConfig(CustomConfigProviderContext customConfigProviderContext)
        {
            if (!_abpStartupConfiguration.EntityHistory.IsEnabled)
            {
                return new Dictionary<string, object>
                {
                    {
                        "EntityHistory",
                        new {
                            IsEnabled = false
                        }
                    }
                };
            }

            var entityHistoryEnabledEntities = new List<string>();

            foreach (var type in EntityHistoryHelper.TrackedTypes)
            {
                if (_abpStartupConfiguration.EntityHistory.Selectors.Any(s => s.Predicate(type)))
                {
                    entityHistoryEnabledEntities.Add(type.FullName);
                }
            }

            return new Dictionary<string, object>
            {
                {
                    "EntityHistory",
                    new {
                        IsEnabled = true,
                        EnabledEntities = entityHistoryEnabledEntities
                    }
                }
            };
        }
    }
}
