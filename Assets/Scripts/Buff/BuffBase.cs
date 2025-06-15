using System;

[Serializable]
public abstract class BuffBase
{
    public BuffData buffData;
    
    public int curLayer; // Buff的层数
    public int curTurn; // Buff的当前回合数
    public float value;
    public CharacterBase from; // Buff的来源
    public CharacterBase target; // Buff现在附加的对象
    
    public BuffBase(BuffData data, CharacterBase from, CharacterBase target)
    {
        buffData = data;
        this.from = from;
        this.target = target;
        curLayer = data.oneLayer; // 默认层数为1
        curTurn = data.turn; // 初始回合数
        value = data.value; // 初始值为BuffData中的值
    }

    public abstract void OnStart();

    public abstract void OnEffect();

    public abstract void OnEnd();

    public abstract void Overlap();
}
