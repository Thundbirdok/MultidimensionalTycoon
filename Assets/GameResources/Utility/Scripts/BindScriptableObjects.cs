using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameResources.Utility.Scripts
{
    public sealed class BindScriptableObjects : MonoInstaller
    {
        [SerializeField]
        private List<ScriptableObject> scriptableObjects = new();

        public override void InstallBindings()
        {
            foreach (var obj in scriptableObjects)
            {
                Container
                    .Bind(obj.GetType())
                    .FromInstance(obj);
            }
        }
    }
}