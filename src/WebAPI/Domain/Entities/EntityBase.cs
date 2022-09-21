using Newtonsoft.Json;
using System.Collections.Generic;

namespace Domain.Entities
{
    public abstract class EntityBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string Etag { get; set; }

        public string Type => GetType().Name;
    }
}
