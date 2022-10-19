using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameResources.Economy.Money
{
    public class MoneyCheat : MonoBehaviour
    {
        [SerializeField]
        private Button add;

        [SerializeField]
        private Button spend;

        private MoneyResourceHandler handler;

        [Inject]
        private void Construct(MoneyResourceHandler resourceHandler)
        {
            handler = resourceHandler;

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
