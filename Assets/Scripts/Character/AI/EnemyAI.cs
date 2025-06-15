using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyAI : MonoBehaviour
{
    public EnemyActionDataSO actionDataSO;
    private EnemyAction curAction;
    private List<EnemyAction> curActions;

    private void Awake()
    {
        curActions = new List<EnemyAction>();
    }

    public EnemyAction OnPlayerTurnBegin()
    {
        if (actionDataSO == null)
        {
            Debug.LogError("EnemyActionDataSO is not assigned.");
        }

        // 选择一个随机的行动
        int randomIndex = Random.Range(0, actionDataSO.actions.Count);
        curAction = actionDataSO.actions[randomIndex];
        return curAction;
    }
}
