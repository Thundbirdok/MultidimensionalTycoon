using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameResources.Control.Economy.Resources.Scripts;
using GameResources.Control.Economy.Resources.Stone;
using GameResources.Control.Economy.Resources.Wood;
using GameResources.Save.Scripts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Zenject;

namespace GameResources.Control.Economy.ResourcesHandler.Scripts
{
    [CreateAssetMenu(fileName = "MetaResourcesHandler", menuName = "Economy/MetaResourcesHandler")]
    public sealed class MetaEconomyResourcesHandler : ScriptableObjectInstaller, ISaveProgress
    {
        public event Action OnChangedValue;

        public IReadOnlyList<Resource> Resources => _handlers.Select(handler => handler.Resource).ToList();
        
        [NonSerialized]
        private JObject _jObject;

        [NonSerialized]
        private readonly List<IResourceHandler> _handlers = new List<IResourceHandler>();

        private const string FILE_NAME = "EconomyResources.json";

        private static string JsonPath
            => Application.persistentDataPath + FILE_NAME;
        
        public override void InstallBindings()
        {
            if (_jObject == null)
            {
                GetJObject();
            }
            
            Container
                .BindInterfacesAndSelfTo<EconomyResourcesHandler>()
                .FromInstance(this)
                .AsSingle();
            
            BindHandler(typeof(WoodResourceHandler));
            BindHandler(typeof(StoneResourceHandler));
        }

        public void Save()
        {
            var saveJObject = CollectResourcesToJObject();

            using var file = File.CreateText(JsonPath);
            using var writer = new JsonTextWriter(file);
            saveJObject.WriteTo(writer);
        }
        
        public bool TryGetHandler(IResourceType resourceType, out IResourceHandler handler)
        {
            handler = _handlers.FirstOrDefault(handler => handler.ResourceType.Equals(resourceType));

            return handler != null;
        }

        public bool TryGetKeyType(string key, out IResourceType type)
        {
            var handler = _handlers.FirstOrDefault
            (
                handler =>
                    handler.ResourceType.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)
            ); 
            
            if (handler != null)
            {
                type = handler.ResourceType;

                return true;
            }

            type = null;

            return false;
        }
        
        public bool Spend(IEnumerable<Resource> prices)
        {
            var isFail =
            (
                from price in prices 
                from handler in _handlers 
                where handler.ResourceType.Equals(price.Type)
                select handler.Spend(price.Value) == false
            ).Any();

            return isFail == false;
        }
        
        public bool IsEnoughResources(IEnumerable<Resource> prices)
        {
            var isFail =
            (
                from price in prices 
                from handler in _handlers 
                where handler.ResourceType.Equals(price.Type)
                select handler.Value < price.Value
            ).Any();

            return isFail == false;
        }
        
        private void BindHandler(Type handlerType)
        {
            var handler = GetHandlerByHandlerType(handlerType);
            
            Container
                .BindInterfacesAndSelfTo(handlerType)
                .FromInstance(handler)
                .AsSingle();
        }

        private IResourceHandler GetHandlerByHandlerType(Type handlerType)
        {
            var handler = _handlers.FirstOrDefault(x => x.GetType() == handlerType);

            if (handler != null)
            {
                return handler;
            }
            
            handler = (IResourceHandler)Activator.CreateInstance(handlerType);
            handler.ChangeWithoutNotify(GetValue(handler.ResourceType.Key));

            handler.OnChangedValue += InvokeOnChangeChangedValue;
            
            _handlers.Add(handler);

            return handler;
        }
        
        private void InvokeOnChangeChangedValue() => OnChangedValue?.Invoke();
        private void GetJObject()
        {
            try
            {
                using var file = File.OpenText(JsonPath);
                using var reader = new JsonTextReader(file);

                _jObject = (JObject)JToken.ReadFrom(reader);
            }
            catch (FileNotFoundException)
            {
                _jObject = new JObject();
            }
        }

        private int GetValue(string key)
        {
            var token = _jObject.SelectToken(key);

            return token?.Value<int>() ?? 0;
        }

        private JObject CollectResourcesToJObject()
        {
            var collection = Container.ResolveAll<IResourceHandler>();

            var saveJObject = new JObject();

            foreach (var handler in collection)
            {
                saveJObject.Add(handler.ResourceType.Key, handler.Value);
            }

            return saveJObject;
        }
    }
}