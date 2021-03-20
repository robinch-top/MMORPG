using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ETModel
{
    public enum StateType
    {
        InBattle,

        UnStoppable,//无法被打断的状态,比如霸体

        NotInControl,//玩家无法操作的状态

        Invincible, // 无敌

        CantDoAction, // 无法做任何行动

        Die // 死亡
        //后续什么沉默或者其他的状态,都可以在这里加. DEMO的话,这几个足够了
    }

    [ObjectSystem]
    public class UnitStateComponentAwakeSystem : AwakeSystem<UnitStateComponent>
    {
        public override void Awake(UnitStateComponent self)
        {
            self.Awake();
        }
    }
    public class UnitStateComponent: Component
    {
        //public Dictionary<int, UnitStateDelta> unitStatesDic; // 存储的是对应帧玩家状态的增量更新

        //public Dictionary<Type, IProperty> unitProperty; //存储玩家当前所有状态数据,如果要做延迟补偿,那么就利用这个和上面对应帧的增量更新来弄.

        public const int maxFrameCount_SaveStateDelta = 120;//服务器最多存储玩家120帧的增量更新数据,即每隔60帧清理一次

        public bool collectInput = false;// 是否接收到了玩家的新输入.

        public int currFrame;
        public int preSendMsgFrame;
        public Unit unit;

        public void Awake()
        {
            unit = GetParent<Unit>();
            currFrame = 0;
            preSendMsgFrame = -3;

        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
        }
    }
}
