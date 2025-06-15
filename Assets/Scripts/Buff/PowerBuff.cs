using UnityEngine;

[Buff("power")]
public class PowerBuff: BuffBase
{
    public PowerBuff(BuffData data, CharacterBase from, CharacterBase target) : base(data, from, target)
    {
    }

    public override void OnStart()
    {
        target.GetComponent<CharacterBase>().damageMultiplier += value * curLayer; // 增加伤害倍率
        Debug.Log($"PowerBuff started on {target.name} from {from.name}. Initial value: {value}, Layer: {curLayer}");
    }

    public override void OnEffect()
    {
        curTurn--; // 每次效果触发后减少回合数
    }

    public override void OnEnd()
    {
        target.GetComponent<CharacterBase>().damageMultiplier -= value * curLayer; // 减少伤害倍率
        Debug.Log($"PowerBuff ended on {target.name}. Final value: {value}, Layer: {curLayer}");
    }

    public override void Overlap()
    {
        if (buffData.overlap == BuffOverlap.None)
        {
            curTurn = buffData.turn; // 重叠时重置回合数
        }
    }
}
