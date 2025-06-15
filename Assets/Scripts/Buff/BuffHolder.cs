using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuffHolder : MonoBehaviour
{
    [SerializeReference] private List<BuffBase> buffs;

    private void Awake()
    {
        buffs = new List<BuffBase>();
    }
    
    
    // 添加buff
    public void AddBuff(BuffData data, CharacterBase from, CharacterBase target)
    {
        // 检查buffs中是否有同id的buff
        if(buffs.FirstOrDefault(b => b.buffData.buffName == data.buffName) is BuffBase existingBuff)
        {
            // 如果有，触发buffBase的Overlap方法
            existingBuff.Overlap();
        }
        else
        {
            var buff = BuffFactory.Create(data.buffName, data, from, target);
            buffs.Add(buff);
            buff.OnStart(); // 调用OnStart方法
        }
    }

    
    // 移除buff
    public void RemoveBuff(BuffBase buff)
    {
        if (buffs.Contains(buff))
        {
            buff.OnEnd(); // 调用OnEnd方法
            buffs.Remove(buff);
        }
        else
        {
            Debug.LogWarning("尝试移除不存在的Buff: " + buff.buffData.buffName);
        }
    }

    // 执行buff效果
    public void ExecuteAllBuff()
    {
        foreach (var buff in buffs.ToList()) // 使用ToList()防止在遍历时修改集合
        {
            buff.OnEffect();
            if (buff.buffData.turn != -1 && buff.curTurn <= 0) // 如果是永久buff则不移除
            {
                RemoveBuff(buff);
            }
        }
    }


    public void ExecuteAllTurnBeginBuff()
    {
        var turnBeginBuffs = buffs.Where(b => b.buffData.type == BuffType.Turn).ToList();
        
        foreach (var buff in turnBeginBuffs)
        {
            buff.OnEffect(); // 执行回合开始的效果
            if (buff.buffData.turn != -1 && buff.curTurn <= 0) // 如果是永久buff则不移除
            {
                RemoveBuff(buff);
            }
        }
    }
}
