﻿using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyActionDataSO", menuName = "Enemy/EnemyActionDataSO", order = 0)]
public class EnemyActionDataSO : ScriptableObject
{
    public List<EnemyAction> actions;
}


[Serializable]
public struct EnemyAction
{
    public Sprite intentSprite;
    public Effect effect;
}