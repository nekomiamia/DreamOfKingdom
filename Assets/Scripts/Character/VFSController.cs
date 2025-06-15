using System;
using UnityEngine;


public class VFSController : MonoBehaviour
{
    public GameObject buff, debuff;
    private float timeCounter;

    private void Update()
    {
        if (buff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.2f) // 假设buff持续时间为5秒
            {
                buff.SetActive(false);
                timeCounter = 0f; // 重置计时器
            }
        }
        if (debuff.activeInHierarchy)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= 1.2f) // 假设buff持续时间为5秒
            {
                debuff.SetActive(false);
                timeCounter = 0f; // 重置计时器
            }
        }
    }
}
