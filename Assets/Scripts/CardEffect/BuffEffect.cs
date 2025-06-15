using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "BuffEffect", menuName = "Card Effect/BuffEffect", order = 0)]
public class BuffEffect : Effect
{
    public BuffData buff;
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.GetComponent<BuffHolder>().AddBuff(buff, from, from);
                from.ActivateBuffView();
                break;
            case EffectTargetType.Target:
                target.GetComponent<BuffHolder>().AddBuff(buff, from, target);
                target.ActivateDeBuffView();
                break;
            case EffectTargetType.All:
                break;
        }
    }
}
