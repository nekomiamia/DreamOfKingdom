
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "DrawEffect", menuName = "Card Effect/DrawEffect", order = 0)]
public class DrawEffect : Effect
{
    public IntEventSO drawCardEvent;
    
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        drawCardEvent?.RaiseEvent(value, this);
    }
}
