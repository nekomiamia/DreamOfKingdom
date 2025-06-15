using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    private BaseEventSO<T> baseEventSO;

    private void OnEnable()
    {
        if (baseEventSO == null)
        {
            baseEventSO = target as BaseEventSO<T>;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("订阅数量:" + GetListener().Count.ToString());
        foreach (var listenter in GetListener())
        {
            EditorGUILayout.LabelField(listenter.ToString());
        }
    }

    private List<MonoBehaviour> GetListener()
    {
        List<MonoBehaviour> listeners = new List<MonoBehaviour>();
        
        if(baseEventSO == null || baseEventSO.OnEventRaised == null)
        {
            return listeners;
        }
        
        var subscribers = baseEventSO.OnEventRaised.GetInvocationList(); 
        foreach (var subscriber in subscribers)
        {
            if (subscriber.Target is MonoBehaviour monoBehaviour)
            {
                if(!listeners.Contains(monoBehaviour))
                    listeners.Add(monoBehaviour);
            }
        }

        return listeners;
    }
}