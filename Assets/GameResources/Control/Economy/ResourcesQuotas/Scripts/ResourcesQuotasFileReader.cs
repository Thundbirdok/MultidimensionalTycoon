namespace GameResources.Control.Economy.ResourcesQuotas.Scripts
{
    using System;
    using System.IO;
    using GameResources.Control.Economy.Resources.Scripts;
    using GameResources.Control.Economy.ResourcesHandler.Scripts;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using UnityEngine;
    
    public static class ResourcesQuotasFileReader
    {
        private const string FILE_PATH = "Settings/Quotas";

        public static StageQuota[] GetQuotas(EconomyResourcesHandler economyResourcesHandler) 
        {
            if (TryGetJArray(out var jArray) == false)
            {
                return null;
            }
            
            var stagesResources = new StageQuota[jArray.Count];
                
            for (var i = 0; i < jArray.Count; ++i)
            {
                var resourcesSerializationArray = JsonConvert.DeserializeObject<ResourceSerialization[]>
                (
                    jArray[i].ToString()
                );

                var resources = new Resource[resourcesSerializationArray.Length];

                for (var j = 0; j < resourcesSerializationArray.Length; j++)
                {
                    var resourceSerialization = resourcesSerializationArray[j];

                    var isTypeFound = economyResourcesHandler.TryGetKeyType
                    (
                        resourceSerialization.ResourceTypeKey,
                        out var type
                    );

                    if (isTypeFound == false)
                    {
                        continue;
                    }
                    
                    resources[j] = new Resource(type, resourceSerialization.ResourceValue);
                }

                stagesResources[i] = new StageQuota(resources);
            }
                
            return stagesResources;
        }
            
        private static bool TryGetJArray(out JArray jArray)
        {
            try
            { 
                var file = Resources.Load<TextAsset>(FILE_PATH);
                
                jArray = file == null ? null : (JArray)JToken.Parse(file.ToString());

                return file != null;
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("Can not get Quotas file");
                    
                jArray = null;

                return false;
            }
        }
        
        [Serializable]
        [JsonObject(MemberSerialization.Fields)]
        public class ResourceSerialization
        {
            [JsonProperty("TypeKey")]
            private string _resourceTypeKey;
            public string ResourceTypeKey => _resourceTypeKey;
            
            [JsonProperty("Value")]
            private int _resourceValue;
            public int ResourceValue => _resourceValue;
        }
    }
}
