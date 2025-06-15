using UnityEngine;
using Utilities;


[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effect/DamageEffect", order = 0)]
public class DamageEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (target == null) return;
        switch (targetType)
        {
            case EffectTargetType.Target:
                var curValue =  value * from.damageMultiplier;
                target.TakeDamage((int)curValue);
                Debug.Log($"DamageEffect executed on {target.name} with value {curValue}");
                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;
            
        }
    }
}
