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
    public class BuildingsInteractionsInitializer : MonoBehaviour
    {
        public event Action OnInited;
        
        public bool IsInited { get; private set; }
        
        private BuildingsDataCollector _buildingsDataCollector;
        private EconomyResourcesHandler _economyResourcesHandler;
        
        private const string FILE_PATH = "Settings/BuildingsInteractions";
        
        private JObject _jObject;
        
        [Inject]
        private void Construct
        (
            BuildingsDataCollector buildingsDataCollector,
            EconomyResourcesHandler economyResourcesHandler
        )
        {
            _buildingsDataCollector = buildingsDataCollector;
            _economyResourcesHandler = economyResourcesHandler;
            
            GetJObject();

            if (_jObject == null)
            {
                return;
            }

            if (_buildingsDataCollector == null)
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
                foreach (var buildingToken in _jObject)
                {
                    building.SetInteractions(GetInteractions(buildingToken.Value));
                }
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

        private List<BuildingsInteractionValue> GetInteractions(JToken packToken)
        {
            if (packToken == null)
            {
                return null;
            }
            
            var interactions = new List<BuildingsInteractionValue>();
            
            foreach (var interactionToken in packToken)
            {
                try
                {
                    if (interactionToken == null)
                    {
                        continue;
                    }

                    if (int.TryParse(interactionToken.ToString(), out var value) == false)
                    {
                        continue;
                    }

                    var resourceObjectData = FindResourceObjectData;

                    if (_economyResourcesHandler.TryGetKeyType(interactionToken, out var type) == false)
                    {
                        continue;
                    }
                    
                    var resource = new Resource(type, interactionToken);
                    
                    var interaction = new BuildingsInteractionValue(resourceObjectData, resource);
                    
                    interactions.Add(interaction);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }

            return interactions;
        }
    }
}
