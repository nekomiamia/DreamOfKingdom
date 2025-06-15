using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;


public class Enemy : CharacterBase
{
    public BuffHolder buffHolder;
    public EnemyAction curAction;
    public Player player;
    
    protected override void Awake()
    {
        base.Awake();
        buffHolder = GetComponent<BuffHolder>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void OnPlayerTurnBegin()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        curAction = GetComponent<EnemyAI>().OnPlayerTurnBegin();
    }
    
    public void OnEnemyTurnBegin()
    {
        buffHolder.ExecuteAllTurnBeginBuff();
        switch (curAction.effect.targetType)
        {
            case EffectTargetType.Self:
                Skill();
                break;
            case EffectTargetType.Target:
                Attack();
                break;
            case EffectTargetType.All:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public virtual void Skill()
    {
        StartCoroutine(ProcessDelayAction("skill"));
    }
    
    public virtual void Attack()
    {
        StartCoroutine(ProcessDelayAction("attack"));
    }

    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        yield return new WaitUntil(()=>animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.6f &&
                                       ! animator.IsInTransition(0)&&
                                       animator.GetCurrentAnimatorStateInfo(0).IsName(actionName));
        if (actionName == "attack")
        {
            curAction.effect.Execute(this, player);
        }else if (actionName == "skill")
        {
            curAction.effect.Execute(this, this);
        }
    }
}
