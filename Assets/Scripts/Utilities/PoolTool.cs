using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Utilities
{
    public class PoolTool : MonoBehaviour
    {
        public GameObject objectPrefab;
        private ObjectPool<GameObject> pool;

        private void Awake()
        {
            pool = new ObjectPool<GameObject>(
                createFunc:()=>Instantiate(objectPrefab, transform),
                actionOnGet:(obj)=>obj.SetActive(true),
                actionOnRelease:(obj)=> obj.SetActive(false),
                actionOnDestroy:(obj)=>Destroy(obj),
                false, // 不自动扩展池
                10, // 初始大小
                20 // 最大大小
            );
            
            PreFillPool(7);
        }
        
        private void PreFillPool(int count)
        {
            var preFillArray = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                preFillArray[i] = pool.Get();

            }

            foreach (var o in preFillArray)
            {
                pool.Release(o);
            }
        } 
        
        public GameObject GetObject()
        {
            return pool.Get();
        }
        
        public void ReleaseObject(GameObject obj)
        {
            if (obj != null)
            {
                pool.Release(obj);
            }
            else
            {
                Debug.LogWarning("尝试释放一个不在池中的对象或对象为null");
            }
        }
    }
}