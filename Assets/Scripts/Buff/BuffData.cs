using System;
using UnityEngine;


[CreateAssetMenu(fileName = "BuffData", menuName = "BuffData", order = 0)]
public class BuffData : ScriptableObject
{
    public string buffName; // Buff 的名称 
    public int turn; // Buff持续的回合数 -1表示永久
    public int maxlayer; // Buff的最大层数，-1表示无限层数
    public int oneLayer; // Buff一次叠加的层数，通常为1
    public float value; // 比如力量值或每秒伤害
    public BuffType type; // Buff的类型，决定了 Buff 的触发方式
    public BuffOverlap overlap; // Buff 的叠加类型，决定了新同名 Buff 进来的效果
    public bool isDebuff; // 是否是 Debuff，影响 Buff 的图标和描述
    [TextArea]
    public string description; // Buff 的描述
}

[Serializable]
public enum BuffType
{
    Turn, // 依赖回合变化
    Action, // 依赖行动变化 (玩家出牌或者敌人行动)
    Hit, // 依赖受击变化 
}


[Serializable]
public enum BuffOverlap // 叠加类型，新同名buff进来的效果
{
    None, // 不叠加，直接覆盖
    AddLayer, // 叠加，增加层数
}


