using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameResources.Economy.Resources.Scripts.Stone
{
    public sealed class StoneCheat : MonoBehaviour
    {
        [SerializeField]
        private Button add;

        [SerializeField]
        private Button spend;

        private StoneResourceHandler _handler;

        [Inject]
        private void Construct(StoneResourceHandler stoneHandler)
        {
            _handler = stoneHandler;

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
