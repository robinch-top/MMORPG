using System.Text;
using UnityEngine;

namespace Assets.MMORPG.Scripts.RPGGame.ScriptableItems
{
    public class UsableItem : ScriptableItem
    {

        [Header("Usage")]
        public int minLevel; // 道具的最小等级要求

        [Header("Cooldown")]
        public float cooldown;

        // 基于name或类别的道具冷却时间，如果没有类别，就只按此种道具name
        // 比如蓝，红，活力药水是同一类型，共cd
        [SerializeField] string _cooldownCategory = "";
        public string cooldownCategory =>
            string.IsNullOrWhiteSpace(_cooldownCategory) ? name : _cooldownCategory;

        // usage ///////////////////////////////////////////////////////////////////
        public virtual bool CanUse(Entity.Player player, int Index, Items.Item item)
        {
            // 检查冷确时间与装备类道具的耐久
            return player.GetItemCooldown(cooldownCategory) == 0 &&
                   item.CheckDurability();
        }
        public virtual void Use(Entity.Player player, int inventoryIndex)
        {
            if (cooldown > 0)
                player.SetItemCooldown(cooldownCategory, cooldown);
        }
        // 道具对效果、声音等使用Rpc回调。
        public virtual void OnUsed(Entity.Player player) { }

        // UsableItem重写药剂类道具的tooltip
        public override string ToolTip()
        {
            StringBuilder tip = new StringBuilder(base.ToolTip());
            tip.Replace("{MINLEVEL}", minLevel.ToString());
            return tip.ToString();
        }
    }
}