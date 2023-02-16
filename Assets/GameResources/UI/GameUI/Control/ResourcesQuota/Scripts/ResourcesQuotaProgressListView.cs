namespace GameResources.UI.GameUI.Control.ResourcesQuota.Scripts
{
    using System.Collections.Generic;
    using GameResources.Control.Economy.ResourcesHandler.Scripts;
    using GameResources.Control.Economy.ResourcesQuotas.Scripts;
    using UnityEngine;
    using UnityEngine.Pool;
    using Zenject;

    public class ResourcesQuotaProgressListView : MonoBehaviour
    {
        [SerializeField]
        private ResourceQuotaProgressView viewPrefab;

        [SerializeField]
        private Transform container;

        private const int DEFAULT_CAPACITY = 2;

        private const int MAX_SIZE = 10;

        private EconomyResourcesHandler _economyResourcesHandler;

        private ResourcesQuotas _quotas;

        private readonly List<ResourceQuotaProgressView> _list = new List<ResourceQuotaProgressView>();

        private ObjectPool<ResourceQuotaProgressView> _pool;

        private bool _isInited;
        
        [Inject]
        private void Construct(EconomyResourcesHandler economyResourcesHandler, ResourcesQuotas quotas)
        {
            _economyResourcesHandler = economyResourcesHandler;
            _quotas = quotas;

            _pool = new ObjectPool<ResourceQuotaProgressView>
            (
                CreateFunc, 
                ActionOnGet,
                ActionOnRelease, 
                ActionOnDestroy,
                false,
                DEFAULT_CAPACITY, 
                MAX_SIZE
            );

            _isInited = true;
        }

        private static void ActionOnDestroy(ResourceQuotaProgressView obj)
        {
            Destroy(obj.gameObject);
        }

        private void ActionOnRelease(ResourceQuotaProgressView obj)
        {
            obj.gameObject.SetActive(false);

            _list.Remove(obj);
        }

        private void ActionOnGet(ResourceQuotaProgressView obj)
        {
            obj.gameObject.SetActive(true);
            
            _list.Add(obj);
        }

        private void OnEnable()
        {
            _quotas.OnCompletedQuota += Populate;
            
            Populate();
        }

        private void OnDisable()
        {
            _quotas.OnCompletedQuota -= Populate;
        }

        private ResourceQuotaProgressView CreateFunc()
        {
            return Instantiate(viewPrefab, container);
        }
        
        private void Populate()
        {
            if (_isInited == false)
            {
                return;
            }
            
            ReleaseViews();

            SetQuotaViews();
        }

        private void SetQuotaViews()
        {
            foreach (var quota in _quotas.CurrentQuota.Resources)
            {
                var view = _pool.Get();

                if (_economyResourcesHandler.TryGetHandler(quota.Key, out var handler) == false)
                {
                    Debug.LogError("Can not find handler for quota: " + quota.Key.Key);

                    continue;
                }

                view.Set(handler, quota.Value.Value);
            }
        }

        private void ReleaseViews()
        {
            for (var i = 0; i < _list.Count; )
            {
                _pool.Release(_list[i]);
            }
        }
    }
}
