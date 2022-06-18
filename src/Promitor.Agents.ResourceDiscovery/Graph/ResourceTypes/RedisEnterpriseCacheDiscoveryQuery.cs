﻿using GuardNet;
using Newtonsoft.Json.Linq;
using Promitor.Core.Contracts;
using Promitor.Core.Contracts.ResourceTypes;

namespace Promitor.Agents.ResourceDiscovery.Graph.ResourceTypes
{
    public class RedisEnterpriseCacheDiscoveryQuery : ResourceDiscoveryQuery
    {
        public override string[] ResourceTypes => new[] { "microsoft.cache/redisenterprise" };
        public override string[] ProjectedFieldNames => new[] { "subscriptionId", "resourceGroup", "name" };

        public override AzureResourceDefinition ParseResults(JToken resultRowEntry)
        {
            Guard.NotNull(resultRowEntry, nameof(resultRowEntry));

            var cacheName = resultRowEntry[2]?.ToString();
            var resource = new RedisEnterpriseCacheResourceDefinition(resultRowEntry[0]?.ToString(), resultRowEntry[1]?.ToString(), cacheName);
            return resource;
        }
    }
}
