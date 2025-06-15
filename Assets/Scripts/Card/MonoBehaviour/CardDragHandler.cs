using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject arrowPrefab;
    private GameObject currentArrow;
    
    private Card curCard;
    private bool canMove;
    private bool canExecute;

    private CharacterBase targetCharacter;
    private void Awake()
    {
        curCard = GetComponent<Card>();
    }

    private void OnDisable()
    {
        canMove = false;
        canExecute = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!curCard.isAvailiable)
        {
            return;
        }
        switch (curCard.cardData.cardType)
        {
            case CardType.Attack:
                currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                break;
            case CardType.Defense:
            case CardType.Ability:
                canMove = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
    }

    
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!curCard.isAvailiable)
        {
            return;
        }
        if (canMove)
        {
            curCard.isAnimating = true;
            Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            curCard.transform.position = worldPos;
            canExecute = worldPos.y > 1f;
        }
        else
        {
            if (eventData.pointerEnter == null) return;

            if (eventData.pointerEnter.CompareTag("Enemy"))
            {
                canExecute = true;
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }

            canExecute = false;
            targetCharacter = null; 
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!curCard.isAvailiable)
        {
            return;
        }
        if(currentArrow!=null)Destroy(currentArrow);
        if (canExecute)
        {
            curCard.ExecuteCardEffects(curCard.player, targetCharacter);
        }
        else
        {
            curCard.ResetCardTransform();
            curCard.isAnimating = false;
        }
    }
}
