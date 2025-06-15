
using UnityEngine;

[Buff("poison")]
public class PoisonBuff : BuffBase
{
    public PoisonBuff(BuffData data, CharacterBase from, CharacterBase target) : base(data, from, target)
    {
    }

    public override void OnStart()
    {
        // 在Buff开始时可以添加一些初始化逻辑
        Debug.Log($"PoisonBuff started on {target.name} from {from.name}. Initial value: {value}");
    }

    public override void OnEffect()
    {
        int curDamage = (int) value * curLayer; // 计算当前层数的伤害
        curTurn--; // 每次效果触发后减少回合数
        curLayer--;
        target.TakeDamage(curDamage);
    }

    public override void OnEnd()
    {
        Debug.Log($"PoisonBuff ended on {target.name}. Final value: {value}");
    }

    public override void Overlap()
    {
        // 当Buff重叠时，可以增加层数或更新值
        curLayer += buffData.oneLayer;
        curTurn = curLayer;
        Debug.Log($"PoisonBuff overlapped on {target.name}. New layer: {curLayer}, New value: {value}");
    }
}
