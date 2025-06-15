using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Card Effect/DefenseEffect", order = 0)]
public class DefenseEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == EffectTargetType.Self)
        {
            from.UpdateDefense(value);
        }
        
        if(targetType == EffectTargetType.Target)
        {
            if (target != null)
            {
                target.UpdateDefense(value);
            }
        }
    }
}
