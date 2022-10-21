using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameResources.Economy.Gems
{
    public sealed class GemsCheat : MonoBehaviour
    {
        [SerializeField]
        private Button add;

        [SerializeField]
        private Button spend;

        private GemsResourceHandler handler;

        [Inject]
        private void Construct(GemsResourceHandler gemsHandler)
        {
            handler = gemsHandler;

            add.onClick.AddListener(Add);
            spend.onClick.AddListener(Spend);
        }

        private void OnDestroy()
        {
            add.onClick.RemoveListener(Add);
            spend.onClick.RemoveListener(Spend);
        }

        private void Add() => handler.Add(5);

        private void Spend() => handler.Spend(5);
    }
}
