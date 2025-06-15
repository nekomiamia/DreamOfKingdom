using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal = true; // 是否水平布局
    public float maxWidth = 7f; // 最大宽度
    public float cardSpacing = 1f; // 卡牌间距
    public Vector3 centerPoint;
    [SerializeField] private List<Vector3> cardPositions = new List<Vector3>();
    private List<Quaternion> cardRotations = new List<Quaternion>();
    
    [Header("弧形参数")]
    public float angleBetweenCards = 5f; // 卡牌之间的角度

    public float maxAngle;
    public float radius = 17f;


    private void Awake()
    {
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }

    public CardTransform GetCardTransform(int index, int totalCards)
    {
        CalculatePosition(totalCards, isHorizontal);
        return new CardTransform(cardPositions[index], cardRotations[index]);
    }
    
    private void CalculatePosition(int numberOfCards, bool horizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();
        if (horizontal)
        {
            float curWidth = cardSpacing * (numberOfCards - 1);
            float totalWidth = Mathf.Min(curWidth, maxWidth);

            float curSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;

            for (int i = 0; i < numberOfCards; i++)
            {
                float xPos = 0 - (totalWidth / 2) + (i * curSpacing);
                var pos = new Vector3(xPos, centerPoint.y, 0);
                var quat = Quaternion.identity;
                cardPositions.Add(pos);
                cardRotations.Add(quat);
            }
        }
        else
        {
            float cardAngle = (numberOfCards - 1) * angleBetweenCards / 2;
            float totalAngle = Mathf.Min(cardAngle, maxAngle);
            
            float curAngleBetween = totalAngle > 0 ? totalAngle * 2 / (numberOfCards - 1) : 0;

            for (int i = 0; i < numberOfCards; i++)
            {
                var pos = FanCardPosition(totalAngle - i * curAngleBetween);
                
                var rotation = Quaternion.Euler(0, 0, totalAngle - i * curAngleBetween);
                cardPositions.Add(pos);
                cardRotations.Add(rotation);    
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0
        );
    }
    
}
