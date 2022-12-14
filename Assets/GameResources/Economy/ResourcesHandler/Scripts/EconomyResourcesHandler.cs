using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameResources.Economy.Resources.Scripts;
using GameResources.Economy.Resources.Scripts.Stone;
using GameResources.Economy.Resources.Scripts.Wood;
using GameResources.Save.Scripts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Zenject;

namespace GameResources.Economy.ResourcesHandler.Scripts
{
    [CreateAssetMenu(fileName = "ResourcesHandler", menuName = "Economy/ResourcesHandler")]
    public sealed class EconomyResourcesHandler : ScriptableObjectInstaller, ISaveProgress
    {
        private const string FILE_NAME = "EconomyResources.json";

        private static string JsonPath
            => Application.persistentDataPath + FILE_NAME;

        [NonSerialized]
        private JObject _jObject;

        [NonSerialized]
        private readonly List<IResourceHandler> _handlers = new List<IResourceHandler>();
        
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
        
        private void BindHandler(Type type)
        {
            var handler = GetHandler(type);
            
            Container
                .BindInterfacesAndSelfTo(type)
                .FromInstance(handler)
                .AsSingle();
        }

        private IResourceHandler GetHandler(Type type)
        {
            var handler = _handlers.FirstOrDefault(x => x.GetType() == type);

            if (handler != null)
            {
                return handler;
            }

            handler = (IResourceHandler)Activator.CreateInstance(type);
            handler.ChangeWithoutNotify(GetValue(handler.Key));
                
            _handlers.Add(handler);

            return handler;
        }
        
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

            foreach (var handler in collection) saveJObject.Add(handler.Key, handler.Value);

            return saveJObject;
        }
    }
}