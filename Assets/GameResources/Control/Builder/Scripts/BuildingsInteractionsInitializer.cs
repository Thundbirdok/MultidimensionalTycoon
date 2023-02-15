using System;
using System.Collections.Generic;
using System.IO;
using GameResources.Control.Building.Scripts;
using GameResources.Control.Economy.Resources.Scripts;
using GameResources.Control.Economy.ResourcesHandler.Scripts;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Zenject;

namespace GameResources.Control.Builder.Scripts
{
    using System.Linq;
    using GameResources.Control.ResourceObjects.Scripts;
    using Newtonsoft.Json;

    public class BuildingsInteractionsInitializer : MonoBehaviour
    {
        public event Action OnInited;
        
        public bool IsInited { get; private set; }

        private ResourceObjectsDataCollector _resourceObjectsDataCollector;
        private BuildingsDataCollector _buildingsDataCollector;
        private EconomyResourcesHandler _economyResourcesHandler;
        
        private const string FILE_PATH = "Settings/BuildingsInteractions";
        
        private JObject _jObject;
        
        [Inject]
        private void Construct
        (
            ResourceObjectsDataCollector resourceObjectsDataCollector,
            BuildingsDataCollector buildingsDataCollector,
            EconomyResourcesHandler economyResourcesHandler
        )
        {
            _resourceObjectsDataCollector = resourceObjectsDataCollector;
            _buildingsDataCollector = buildingsDataCollector;
            _economyResourcesHandler = economyResourcesHandler;
            
            GetJObject();

            if (_jObject == null)
            {
                return;
            }

            if (_buildingsDataCollector.IsInited == false)
            {
                _buildingsDataCollector.OnInited += Init;

                return;
            }
            
            Init();
        }
        
        ~BuildingsInteractionsInitializer()
        {
            _buildingsDataCollector.OnInited -= Init;
        }
        
        private void Init()
        {
            _buildingsDataCollector.OnInited -= Init;

            foreach (var building in _buildingsDataCollector.Buildings)
            {
                if (building.Key.Equals(""))
                {
                    Debug.LogError("Building key is empty!", building);
                    
                    continue;
                }
                
                var buildingToken = _jObject[building.Key];

                if (buildingToken == null)
                {
                    Debug.LogError("Building key in token not found!", building);
                    
                    continue;
                }
                
                building.SetInteractions(GetInteractions(buildingToken));
            }

            IsInited = true;
            OnInited?.Invoke();
        }
        
        private void GetJObject()
        {
            try
            { 
                var file = Resources.Load<TextAsset>(FILE_PATH);

                if (file != null)
                {
                    _jObject = (JObject)JToken.Parse(file.ToString());
                }
            }
            catch (FileNotFoundException)
            {
                _jObject = new JObject();
            }
        }

        private List<BuildingsInteractionValue> GetInteractions(JToken buildingToken)
        {
            if (buildingToken == null)
            {
                return null;
            }
            
            var interactions = new List<BuildingsInteractionValue>();
            
            foreach (var interactionToken in buildingToken)
            {
                GetInteraction(interactionToken, ref interactions);
            }

            return interactions;
        }

        private void GetInteraction
            (
            JToken interactionToken,
            ref List<BuildingsInteractionValue> interactions
            )
        {
            var isSerializationGet = TryGetInteractionSerialization
            (
                interactionToken,
                out var interactionSerialization
            );

            if (isSerializationGet == false)
            {
                return;
            }

            var isResourceObjectDataGet = TryGetResourceObjectData
            (
                interactionSerialization.ResourceObjectKey,
                out var resourceObjectData
            );

            if (isResourceObjectDataGet == false)
            {
                return;
            }

            var isTypeFound = _economyResourcesHandler.TryGetKeyType
            (
                interactionSerialization.ResourceTypeKey,
                out var type
            );

            if (isTypeFound == false)
            {
                return;
            }

            var resource = new Resource(type, interactionSerialization.ResourceValue);

            var interaction = new BuildingsInteractionValue(resourceObjectData, resource);

            interactions.Add(interaction);
        }

        private static bool TryGetInteractionSerialization
        (
            JToken interactionToken, 
            out InteractionSerialization interactionSerialization
        )
        {
            if (interactionToken == null)
            {
                interactionSerialization = null;
                
                return false;
            }

            try
            {
                interactionSerialization = interactionToken
                        .ToObject(typeof(InteractionSerialization))
                    as InteractionSerialization;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                
                interactionSerialization = null;
                
                return false;
            }
            
            return interactionSerialization is { IsKeysFound: true };
        }

        private bool TryGetResourceObjectData(string resourceObjectKey, out ResourceObjectData data)
        {
            var buildingData = _buildingsDataCollector.Buildings
                .FirstOrDefault
                (
                    building => building.Key.Equals
                    (
                        resourceObjectKey,
                        StringComparison.InvariantCultureIgnoreCase
                    )
                );

            if (buildingData != null)
            {
                data = buildingData;

                return true;
            }

            var resourceObjectData = _resourceObjectsDataCollector.ResourceObjects
                .FirstOrDefault
                (
                    building => building.Key.Equals
                    (
                        resourceObjectKey,
                        StringComparison.InvariantCultureIgnoreCase
                    )
                );

            if (resourceObjectData != null)
            {
                data = resourceObjectData;

                return true;
            }
            
            data = null;
                
            return false;
        }

        [Serializable]
        [JsonObject(MemberSerialization.Fields)]
        public class InteractionSerialization
        {
            [JsonProperty("ResourceObjectKey")]
            private string _resourceObjectKey;
            public string ResourceObjectKey => _resourceObjectKey;
            
            [JsonProperty("ResourceTypeKey")]
            private string _resourceTypeKey;
            public string ResourceTypeKey => _resourceTypeKey;
            
            [JsonProperty("ResourceValue")]
            private int _resourceValue;
            public int ResourceValue => _resourceValue;

            public bool IsKeysFound => _resourceObjectKey.Equals("") == false
                                       && _resourceTypeKey.Equals("") == false;
        }
    }
}
