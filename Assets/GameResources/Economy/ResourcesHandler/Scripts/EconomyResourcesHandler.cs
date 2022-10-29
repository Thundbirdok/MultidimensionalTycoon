using System;
using System.IO;
using GameResources.Economy.Stone;
using GameResources.Economy.Wood;
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

        private JObject _jObject;
        
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
            using JsonTextWriter writer = new(file);
            saveJObject.WriteTo(writer);
        }
        
        private void BindHandler(Type type)
        {
            var handler = (IResourceHandler)Activator.CreateInstance(type);
            handler.ChangeWithoutNotify(GetValue(handler.Key));

            Container
                .BindInterfacesAndSelfTo(type)
                .FromInstance(handler)
                .AsSingle();
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