using System;

/// 持有ScriptableItem的结构体，类似的还有 :
// Item            道具属性方法结构体
// ItemSlot        格子属性方法结构体
// ItemSC          持有ScriptableItem的结构体
// Skill,Buff      技能,buff属性方法结构体
namespace Assets.MMORPG.Scripts.RPGGame.Items
{

    [Serializable]
    public struct ItemSC
    {
        public ScriptableItems.ScriptableItem item;
        public int amount;
        public string type;
    }
}