using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameResources.Economy.Resources.Scripts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Zenject;

namespace GameResources.Location.Builder.Scripts
{
    public class BuildingsPricesInitializer : MonoBehaviour
    {
        public event Action OnInited;
        
        public bool IsInited { get; private set; }

        private const string FILE_NAME = "/GameResources/Location/Building/Settings/Prices.json";
        
        private static string JsonPath
            => Application.dataPath + FILE_NAME;
        
        private BuildingsViewDataCollector _buildingsViewDataCollector;

        private JObject _jObject;

        private List<IResource> _resources;

        [Inject]
        private void Construct(BuildingsViewDataCollector buildingsViewDataCollector)
        {
            _buildingsViewDataCollector = buildingsViewDataCollector;
        }
        
        private void Start()
        {
            GetJObject();

            GetResourcesTypes();

            if (_buildingsViewDataCollector == null)
            {
                _buildingsViewDataCollector.OnInited += Init;

                return;
            }
            
            Init();
        }

        private void OnDestroy()
        {
            _buildingsViewDataCollector.OnInited -= Init;
        }
        
        private void Init()
        {
            _buildingsViewDataCollector.OnInited -= Init;
            
            foreach (var building in _buildingsViewDataCollector.Buildings)
            {
                building.SetPrice(GetPrices(building.Key));
            }

            IsInited = true;
            OnInited?.Invoke();
        }
        
        private void GetJObject()
        {
            try
            {
                using var file = File.OpenText(JsonPath);
                using JsonTextReader reader = new(file);

                _jObject = (JObject)JToken.ReadFrom(reader);
            }
            catch (FileNotFoundException)
            {
                _jObject = new JObject();
            }
        }
        
        private void GetResourcesTypes()
        {
            _resources = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IResource).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(type => Activator.CreateInstance(type) as IResource)
                .ToList();
        }
        
        private List<ResourceValue> GetPrices(string key)
        {
            var buildingToken = _jObject.SelectToken(key);

            if (buildingToken == null)
            {
                return null;
            }
            
            var priceToken = buildingToken.SelectToken("Price");

            if (priceToken == null)
            {
                return null;
            }

            var price = new List<ResourceValue>();
            
            foreach (var resource in _resources)
            {
                var resourceValueToken = priceToken.SelectToken(resource.Key);

                if (resourceValueToken == null)
                {
                    continue;
                }
                
                var resourceValue = new ResourceValue(resource, resourceValueToken.Value<int>());
                price.Add(resourceValue);
            }

            return price;
        }
    }
}
