using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utilities
{
    public class Loader : MonoBehaviour
    {
        public AssetReference persistentScene;

        private void Awake()
        {
            Addressables.LoadSceneAsync(persistentScene); 
        }
    }
}