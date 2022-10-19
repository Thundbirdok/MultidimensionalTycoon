using System;
using System.IO;
using GameResources.Economy.Gems;
using GameResources.Economy.Money;
using GameResources.Save.Scripts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Zenject;

namespace GameResources.Economy.ResourcesHandler.Scripts
{
    [CreateAssetMenu(fileName = "ResourcesHandler", menuName = "Economy/ResourcesHandler")]
    public class EconomyResourcesHandler : ScriptableObjectInstaller, ISaveProgress
    {
        private const string FILE_NAME = "EconomyResources.json";

        private bool _isLoaded;

        private JObject _jObject;

        private static string JsonPath
            => Path.Combine(Application.persistentDataPath, FILE_NAME);

        public void Save()
        {
            var saveJObject = CollectResourcesToJObject();

            using var file = File.CreateText(JsonPath);
            using JsonTextWriter writer = new(file);
            saveJObject.WriteTo(writer);
        }

        public override void InstallBindings()
        {
            if (_isLoaded == false) Load();

            BindHandler(typeof(MoneyResourceHandler));
            BindHandler(typeof(GemsResourceHandler));
        }

        private void Load()
        {
            GetJObject();
            
            _isLoaded = true;
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