using System;

namespace Utilities
{
    [Flags]
    public enum RoomType
    {
        MinorEnemy = 1, // 普通敌人
        EliteEnemy = 2, // 精英敌人
        Shop = 4, // 商店
        Treasure = 8, // 宝箱
        RestRoom = 16, // 休息室
        Boss = 32 // Boss房间
    }
    
    public enum RoomState
    {
        Locked, // 锁定状态
        Visited, // 已访问状态
        Attainable, // 可访问状态
    }
    
    public enum CardType
    {
        Attack, // 攻击卡
        Defense, // 防御卡
        Ability, // 技能卡
    }

    public enum EffectTargetType
    {
        Self,
        Target,
        All
    }
}