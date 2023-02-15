namespace GameResources.Control.BuildingPacks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using GameResources.Control.Builder.Scripts;
    using Newtonsoft.Json.Linq;
    using UnityEngine;

    public sealed class BuildingsPacksInitializer
    {
        public event Action OnInited;
        
        public bool IsInited { get; private set; }
        
        private List<BuildingsPack> _packs = new List<BuildingsPack>();
        public IReadOnlyList<BuildingsPack> Packs => _packs;
        
        private const string FILE_PATH = "Settings/Packs";

        private BuildingsDataCollector _buildingsDataCollector;

        private JObject _jObject;
        
        public BuildingsPacksInitializer(BuildingsDataCollector buildingsDataCollector)
        {
            _buildingsDataCollector = buildingsDataCollector;
            
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

        ~BuildingsPacksInitializer()
        {
            _buildingsDataCollector.OnInited -= Init;
        }
        
        private void Init()
        {
            _buildingsDataCollector.OnInited -= Init;
            
            foreach (var pack in _jObject)
            {
                _packs.Add(GetPack(pack.Value));
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

        private BuildingsPack GetPack(JToken packToken)
        {
            if (packToken == null)
            {
                return null;
            }
            
            var pack = new BuildingsPack();
            
            foreach (var building in _buildingsDataCollector.Buildings)
            {
                try
                {
                    var buildingToken = packToken.SelectToken(building.Key);

                    if (buildingToken == null)
                    {
                        continue;
                    }

                    if (int.TryParse(buildingToken.ToString(), out var value) == false)
                    {
                        continue;
                    }
                    
                    var pair = new BuildingSlot(building, value);
                    pack.Slots.Add(pair);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }

            return pack;
        }
    }
}
