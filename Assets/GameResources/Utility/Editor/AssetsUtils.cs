using System;
using System.Collections.Generic;
using UnityEditor;
using Object = UnityEngine.Object;

namespace GameResources.Utility.Editor
{
    public static class AssetsUtils
    {
        public static IEnumerable<Object> GetAssets(Type type, string name)
        {
            var guids = AssetDatabase.FindAssets("t:" + name);

            var assets = new List<Object>(guids.Length);
            
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                
                assets.Add(AssetDatabase.LoadAssetAtPath(path, type));
            }

            return assets;
        }
    }
}
