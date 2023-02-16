using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameResources.Control.Economy.Resources.Scripts;
using GameResources.Control.Economy.Resources.Stone;
using GameResources.Control.Economy.Resources.Wood;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Zenject;

namespace GameResources.Control.Economy.ResourcesHandler.Scripts
{
    [CreateAssetMenu(fileName = "ResourcesHandler", menuName = "Economy/ResourcesHandler")]
    public sealed class EconomyResourcesHandler : ScriptableObjectInstaller
    {
        public event Action OnChangedValue;

        public IReadOnlyList<Resource> Resources => _handlers.Select(handler => handler.Resource).ToList();
        
        [NonSerialized]
        private JObject _jObject;

        [NonSerialized]
        private readonly List<IResourceHandler> _handlers = new List<IResourceHandler>();

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<EconomyResourcesHandler>()
                .FromInstance(this)
                .AsSingle();
            
            BindHandler(typeof(WoodResourceHandler));
            BindHandler(typeof(StoneResourceHandler));
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

            handler.OnChangedValue += InvokeOnChangeChangedValue;
            
            _handlers.Add(handler);

            return handler;
        }
        
        private void InvokeOnChangeChangedValue() => OnChangedValue?.Invoke();
    }
}