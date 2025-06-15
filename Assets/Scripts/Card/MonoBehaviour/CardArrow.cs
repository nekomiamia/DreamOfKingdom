using System;
using UnityEngine;


public class CardArrow : MonoBehaviour
{
    public int pointCount;
    public float arcModifier;
    
    private LineRenderer lineRenderer;
    private Vector3 mousePos;
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        SetArrowPosition();
    }

    public void SetArrowPosition()
    {
        Vector3 cardPosition = transform.position;
        Vector3 direction = (mousePos - cardPosition).normalized;
        
        Vector3 offset = new Vector3(-direction.y, direction.x, direction.z) * arcModifier;

        Vector3 controlPoint = (cardPosition + mousePos) / 2 + offset;
        lineRenderer.positionCount = pointCount;
        for (int i = 0; i < pointCount; i++)
        {
            float t = i / (float)(pointCount - 1);
            Vector3 bezierPoint = CalculateBezierPoint(t, cardPosition, controlPoint, mousePos);
            lineRenderer.SetPosition(i, bezierPoint);
        }
    }
    
    // 计算二次贝塞尔曲线的点
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }
}
