using UnityEngine;
using System;

////////////////////////////////////////////////////////////////////////////
// ** 抽象类与接口的使用和比较：
// 只是一个类元件，并不希望他被实例化，使用抽象类，如ScriptableNonAlloc，NetBehaviourNonAlloc
// 包含抽象方法，必须使用抽象类
// 不完整的类，但又有属性也有方法，有抽象方法，也有非抽象方法，使用抽象类
// 接口多定义对象的行为；抽象类多定义对象的属性；

// 如果设计小而简练的功能块，使用接口
    // -> 尽量将接口设计成功能单一的定义

// 如果要设计大的功能系统，使用抽象类
   // -> 比如 Ability能力系统，Enitity实体更是整体ECS系统结构的角色实体基类
   // -> 主要定义子类需要的核心属性，用虚方法定义的可供子类调用的基类方法
   // -> 主要定义的方法都是抽象方法，或空的虚方法，具体实现由子类重写 

// 抽象类应主要用于关系更密切的对象定义，而接口主要用于关系较远的不同系统间通用方法的约定
   // -> 比如 UsableItem 只用于 道具类对象继承，Ability只用于角色能力相关对象继承
   // -> 比如 ICombat 可用于在装备道具系统，技能系统约定通用的方法，供能力系统调用

public struct BattleSts
{
    public bool battle;
}
//角色能力基本类，派生类有Mana,Health,Combat等
public abstract class Ability : NetBehaviourNonAlloc
{
    int _current = 0;
    /// <summary>
    /// 能力值的当前数值
    /// </summary>
    public int current
    {
        get { return Mathf.Min(_current, max); }
        set
        {
            _current = Mathf.Clamp(value, 0, max);
        }
    }
    /// <summary>
    /// 角色重生后,能力值是否立即为满值
    /// </summary>
    public bool spawnFull = true;
    /// <summary>
    /// 能力值的最大值,基类定义为抽象属性
    /// </summary>
    public abstract int max { get; }

    /// <summary>
    /// 能力组件中获取所属角色Entity组件
    /// </summary>
    Entity entity;
    /// <summary>
    /// 获取所属角色的Health组件,public但不序列化
    /// </summary>
    [NonSerialized]
    public Health health;
    /// <summary>
    /// 获取所属角色的Level组件,public但不序列化
    /// </summary>
    [NonSerialized]
    public Level level;

    ////////////////////////////////////////////////////////////////////////////
    // 能力值自动恢复相关的属性
    // -> 通过这些属性可以决定此项能力值是否可自动恢复，恢复间隔，单位时间恢复量
    /// <summary>
    /// 可否自动恢复
    /// </summary>
    public bool canRecover = false;
    /// <summary>
    /// 单位时间恢复数量，基类定义为抽象属性
    /// </summary>
    public abstract int recovery { get; }
    /// <summary>
    /// 脱战恢复间隔
    /// </summary>
    public int timeRate = 2;
    /// <summary>
    /// 战斗恢复间隔
    /// </summary>
    public int battleTimeRate = 5;
    /// <summary>
    /// 用于比较角色战斗状态
    /// </summary>
    bool sts = true;

    void Awake(){
        entity = GetComponent<Entity>();
        health = GetComponent<Health>();
        level = GetComponent<Level>();

        // 重生时满数值
        if (spawnFull) current = max;
    }

    // Percent /////////////////////////////////////////////////////////////
    /// -> <summary>获取当前值与最大值的百分比。</summary>
    public float Percent() =>
        (current != 0 && max != 0) ? (float)current / (float)max : 0;


    void Update()
    {
        // 如果能力值可恢复，并且当前能力值小于最大能力值
        if(canRecover && current < max){
            // 如果战斗状态发生改变，调用DoRepeat方法
            if(sts != entity.inbattle){
                sts = entity.inbattle;
                DoRepeat("Recover");
            } 
        }
    }

    // DoRepeat /////////////////////////////////////////////////////////////
    /// -> <summary>根据战斗状态切换恢复间隔频率，执行InvokeRepeating。</summary>
    private void DoRepeat(string recover){
        // 根据战斗状态切换恢复间隔频率
        int rate;
        if(sts) rate = battleTimeRate;
        else rate = timeRate;
        
        // 取消已有的Invoke方法，并用新的间隔频率执行InvokeRepeating
        if(IsInvoking(recover))
            CancelInvoke(recover);
        InvokeRepeating(recover, 0, rate);     
    }

    // Recover /////////////////////////////////////////////////////////////
    /// -> <summary>自动恢复，间隔调用的方法。</summary>
    public virtual void Recover()
    {
        if (enabled && health.current > 0)
            current += recovery;
    }

}
