using Assets.MMORPG.Scripts.RPGGame.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    public partial class UISkillbar : MonoBehaviour
    {
        public GameObject panel;
        public UISkillbarSlot slotPrefab;
        public Transform content;

        [Header("Durability Colors")]
        public Color brokenDurabilityColor = Color.red;
        public Color lowDurabilityColor = Color.magenta;
        [Range(0.01f, 0.99f)] public float lowDurabilityThreshold = 0.1f;
        void Update()
        {
            Entity.Player player = Entity.Player.localPlayer;
            if (!(player is null))
            {

                panel.SetActive(true);

                // 删除和创建工具条格子，维持工具条足够的格子实例
                UIUtils.BalancePrefabs(slotPrefab.gameObject, player.skillbar.slots.Length, content);

                // refresh all
                for (int i = 0; i < player.skillbar.slots.Length; ++i)
                {
                    Player.SkillbarEntry entry = player.skillbar.slots[i];

                    UISkillbarSlot slot = content.GetChild(i).GetComponent<UISkillbarSlot>();
                    slot.dragAndDropable.name = i.ToString(); // drag and drop index

                    // 键位ui标识
                    string pretty = entry.hotKey.ToString().Replace("Alpha", "");
                    slot.hotkeyText.text = pretty;

                    // 技能，道具或装备道具
                    int skillIndex = player.skills.GetSkillIndexByName(entry.reference);
                    int inventoryIndex = player.inventory.GetItemIndexByName(entry.reference);
                    int equipmentIndex = player.equipment.GetItemIndexByName(entry.reference);

                    // 技能
                    if (skillIndex != -1)
                    {
                        Skill.Skill skill = player.skills.skills[skillIndex];
                        bool canCast = player.skills.CastCheckSelf(skill);
                        // 热键使用技能
                        if (Input.GetKeyDown(entry.hotKey) &&
                            !UIUtils.AnyInputActive()
                            && canCast)
                        {
                            //print(skillIndex);
                            ((Player.PlayerSkills)player.skills).TryUse(skillIndex);
                        }

                        // 点击使用技能
                        slot.button.interactable = canCast;
                        slot.button.onClick.SetListener(() =>
                        {
                            ((Player.PlayerSkills)player.skills).TryUse(skillIndex);
                        });

                        slot.tooltip.enabled = true;
                        if (slot.tooltip.IsVisible())
                            slot.tooltip.text = skill.ToolTip();
                        slot.dragAndDropable.dragable = true;
                        slot.image.color = Color.white;
                        slot.image.sprite = skill.image;

                        // 技能释放冷却
                        float cooldown = skill.CooldownRemaining();
                        slot.cooldownOverlay.SetActive(cooldown > 0);
                        slot.cooldownText.text = cooldown.ToString("F0");
                        slot.cooldownCircle.fillAmount = skill.cooldown > 0 ? cooldown / skill.cooldown : 0;

                        slot.amountOverlay.SetActive(false);
                    }
                    // 背包道具(非装备道具)
                    else if (inventoryIndex != -1)
                    {
                        Items.ItemSlot itemSlot = player.inventory.slots[inventoryIndex];
                        // 热键使用道具
                        if (Input.GetKeyDown(entry.hotKey) && !UIUtils.AnyInputActive())
                            player.inventory.CmdUseItem(inventoryIndex);

                        // 点击使用道具
                        slot.button.onClick.SetListener(() =>
                        {
                            player.inventory.CmdUseItem(inventoryIndex);
                        });

                        slot.tooltip.enabled = true;
                        if (slot.tooltip.IsVisible())
                            slot.tooltip.text = itemSlot.ToolTip();
                        slot.dragAndDropable.dragable = true;

                        // 耐久度颜色
                        if (itemSlot.item.maxDurability > 0)
                        {
                            if (itemSlot.item.durability == 0)
                                slot.image.color = brokenDurabilityColor;
                            else if (itemSlot.item.DurabilityPercent() < lowDurabilityThreshold)
                                slot.image.color = lowDurabilityColor;
                            else
                                slot.image.color = Color.white;
                        }
                        else slot.image.color = Color.white;
                        slot.image.sprite = itemSlot.item.image;

                        slot.cooldownOverlay.SetActive(false);
                        // 如果是可使用道具的CD
                        if (itemSlot.item.data is ScriptableItems.UsableItem usable)
                        {
                            float cooldown = player.GetItemCooldown(usable.cooldownCategory);
                            slot.cooldownCircle.fillAmount = usable.cooldown > 0 ? cooldown / usable.cooldown : 0;
                        }
                        else slot.cooldownCircle.fillAmount = 0;
                        slot.amountOverlay.SetActive(itemSlot.amount > 1);
                        slot.amountText.text = itemSlot.amount.ToString();
                    }
                    // 装备道具
                    else if (equipmentIndex != -1)
                    {
                        Items.ItemSlot itemSlot = player.equipment.slots[equipmentIndex];

                        // 这里移除了点击事件
                        slot.button.onClick.RemoveAllListeners();
                        // 可使用装备在工具条上能不能使用？
                        // ...

                        slot.tooltip.enabled = true;
                        if (slot.tooltip.IsVisible())
                            slot.tooltip.text = itemSlot.ToolTip();
                        slot.dragAndDropable.dragable = true;

                        // 耐久度颜色
                        if (itemSlot.item.maxDurability > 0)
                        {
                            if (itemSlot.item.durability == 0)
                                slot.image.color = brokenDurabilityColor;
                            else if (itemSlot.item.DurabilityPercent() < lowDurabilityThreshold)
                                slot.image.color = lowDurabilityColor;
                            else
                                slot.image.color = Color.white;
                        }
                        else slot.image.color = Color.white;
                        slot.image.sprite = itemSlot.item.image;
                        slot.cooldownOverlay.SetActive(false);
                        // 如果是可使用道具的CD
                        if (itemSlot.item.data is ScriptableItems.UsableItem usable)
                        {
                            float cooldown = player.GetItemCooldown(usable.cooldownCategory);
                            slot.cooldownCircle.fillAmount = usable.cooldown > 0 ? cooldown / usable.cooldown : 0;
                        }
                        else slot.cooldownCircle.fillAmount = 0;
                        slot.amountOverlay.SetActive(itemSlot.amount > 1);
                        slot.amountText.text = itemSlot.amount.ToString();
                    }
                    else
                    {
                        //清除失效的引用
                        //SkillbarEntry[] slots，（需要直接赋值，因为SkillbarEntry是一个结构体）
                        player.skillbar.slots[i].reference = "";

                        // refresh empty slot
                        slot.button.onClick.RemoveAllListeners();
                        slot.tooltip.enabled = false;
                        slot.dragAndDropable.dragable = false;
                        slot.image.color = Color.clear;
                        slot.image.sprite = null;
                        slot.cooldownOverlay.SetActive(false);
                        slot.cooldownCircle.fillAmount = 0;
                        slot.amountOverlay.SetActive(false);
                    }
                }
            }
            else panel.SetActive(false);
        }
    }
}
