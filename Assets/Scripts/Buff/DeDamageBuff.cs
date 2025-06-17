
using UnityEngine;

[Buff("dedamage")]
public class DeDamageBuff : BuffBase
{
    public DeDamageBuff(BuffData data, CharacterBase from, CharacterBase target) : base(data, from, target)
    {
    }

    public override void OnStart()
    {
        target.GetComponent<CharacterBase>().vulnerabilityMultiplier += value; // 增加伤害倍率
        Debug.Log($"DeDamageBuff started on {target.name} from {from.name}. Initial value: {value}, Layer: {curLayer}");
    }

    public override void OnEffect()
    {
        curLayer--;
    }

    public override void OnEnd()
    {
        target.GetComponent<CharacterBase>().vulnerabilityMultiplier -= value; // 减少伤害倍率
    }

    public override void Overlap()
    {
        if (buffData.overlap == BuffOverlap.None)
        {
            curTurn = buffData.turn; // 重叠时重置回合数
        }
    }
}
