using UnityEngine;
using Utilities;


[CreateAssetMenu(fileName = "HealEffect", menuName = "Card Effect/HealEffect", order = 0)]
public class HealEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == EffectTargetType.Self)
        {
            from.HealHealth(value);
        }
        
        if(targetType == EffectTargetType.Target)
        {
            if (target != null)
            {
                target.HealHealth(value);
            }
        }
    }
}
