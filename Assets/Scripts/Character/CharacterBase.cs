using System;
using UnityEngine;


public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
    public IntVariable defense;
    public GameObject buff;
    public GameObject debuff;
    
    public float damageMultiplier = 1f; // 伤害倍率
    public float vulnerabilityMultiplier = 0f; // 易伤倍率
    
    [Header("广播事件")]
    public ObjectEventSO characterDeathEvent; // 角色死亡事件
    public int CurHP
    {
        get => hp.curValue;
        set => hp.SetValue(value);
    }

    public int MaxHP
    {
        get => hp.maxValue;
    }

    protected Animator animator;
    public bool isDead;
    
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }


    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurHP = MaxHP;
        
        ResetDefense();
    }

    protected virtual void Update()
    {
        animator.SetBool("isDead", isDead);
    }

    public virtual void TakeDamage(int damage)
    {
        var vulnerDamage = (int)(damage + damage * vulnerabilityMultiplier);
        var curDamage = (vulnerDamage - defense.curValue) >= 0?(vulnerDamage - defense.curValue):0;
        var curDefense = (defense.curValue - vulnerDamage) >= 0 ? (defense.curValue - vulnerDamage) : 0;
        defense.SetValue(curDefense);
        if (CurHP > curDamage)
        {
            CurHP -= curDamage;
            Debug.Log($"{gameObject.name} 受到了 {curDamage} 点伤害，当前生命值：{CurHP}/{MaxHP}");
            animator.SetTrigger("hit");
        }
        else
        {
            CurHP = 0;
            
            // TODO Die
            characterDeathEvent?.RaiseEvent(this, this);
            isDead = true;
        }
    }

    public void UpdateDefense(int amount)
    {
        var value = defense.curValue + amount;
        defense.SetValue(value);
    }
    

    public void ResetDefense()
    {
        defense.SetValue(0);
    }

    public void HealHealth(int amount)
    {
        CurHP += amount;
        if (CurHP > MaxHP)
        {
            CurHP = MaxHP;
        }
        ActivateBuffView();
    }
    
    public void ActivateBuffView()
    {
        if (buff != null)
        {
            buff.SetActive(true);
        }
    }
    
    public void ActivateDeBuffView()
    {
        if (buff != null)
        {
            debuff.SetActive(true);
        }
    }
}
