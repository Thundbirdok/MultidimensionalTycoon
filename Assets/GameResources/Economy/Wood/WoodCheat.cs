using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameResources.Economy.Wood
{
    public sealed class WoodCheat : MonoBehaviour
    {
        [SerializeField]
        private Button add;

        [SerializeField]
        private Button spend;

        private WoodResourceHandler _handler;

        [Inject]
        private void Construct(WoodResourceHandler resourceHandler)
        {
            _handler = resourceHandler;

            add.onClick.AddListener(Add);
            spend.onClick.AddListener(Spend);
        }

        private void OnDestroy()
        {
            add.onClick.RemoveListener(Add);
            spend.onClick.RemoveListener(Spend);
        }

        private void Add() => _handler.Add(5);

        private void Spend() => _handler.Spend(5);
    }
}
