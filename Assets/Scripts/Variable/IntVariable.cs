using UnityEngine;


[CreateAssetMenu(fileName = "IntVariable", menuName = "Variable/IntVariable", order = 0)]
public class IntVariable : ScriptableObject
{
    public int maxValue;
    public int curValue;

    public IntEventSO valueChangedEvent;
    [TextArea]
    [SerializeField] private string description;
    
    
    public void SetValue(int value)
    {
        curValue = value;
        valueChangedEvent?.RaiseEvent(value, this);
    }
}
