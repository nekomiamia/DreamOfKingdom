using UnityEngine;
using UnityEngine.EventSystems;


public class TreasureButton : MonoBehaviour, IPointerDownHandler
{
    public ObjectEventSO gameWinEvent;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("TreasureButton clicked");
        gameWinEvent.RaiseEvent(null, this);
    }
}
