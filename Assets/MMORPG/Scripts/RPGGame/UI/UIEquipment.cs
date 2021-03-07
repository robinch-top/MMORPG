using UnityEngine;
using UnityEngine.UI;

// 装备界面UI组件
public partial class UIEquipment : MonoBehaviour
{
    public KeyCode hotKey = KeyCode.U; 
    public GameObject panel;
    public UIEquipmentSlot slotPrefab;
    public Transform content;

    /// <summary>
    /// 0耐久度颜色
    /// </summary>
    public Color brokenDurabilityColor = Color.red;

    /// <summary>
    /// 低耐久度颜色
    /// </summary>
    public Color lowDurabilityColor = Color.magenta;

    /// <summary>
    /// 低装备耐久度阀值
    /// </summary>
    [Range(0.01f, 0.99f)] public float lowDurabilityThreshold = 0.2f;

    void Update()
    {
        Player player = Player.localPlayer;
        if (player != null)
        {
            // 装备界面开关快捷按键
            if (Input.GetKeyDown(hotKey) && !UIUtils.AnyInputActive())
                panel.SetActive(!panel.activeSelf);

            // 装备界面显示时才更新其中的装备道具
            if (panel.activeSelf)
            {
                // 维持装备界面Content下面足够的装备格子实例
                UIUtils.BalancePrefabs(slotPrefab.gameObject, player.equipment.slots.Count, content);

                // 刷新装备界面的的装备道具
                for (int i = 0; i < player.equipment.slots.Count; ++i)
                {
                    UIEquipmentSlot uislot = content.GetChild(i).GetComponent<UIEquipmentSlot>();
                    uislot.dragAndDropable.name = i.ToString(); // drag and drop slot
                    ItemSlot slot = player.equipment.slots[i];

                    // 显示装备部位名称
                    EquipSlotInfo slotInfo = player.equipment.slotInfo[i];
                    uislot.categoryOverlay.SetActive(slotInfo.requiredCategory != "");
                    string overlay = slotInfo.catName;
                    uislot.categoryText.text = overlay != "" ? overlay : "?";

                    /********************************************************************/
                    /* 刷新装备格子                                                       *                        
                    /*==================================================================*/
                    if (slot.amount > 0)
                    {
                        // 在背包中使用道具的点击事件
                        int icopy = i; 
                        uislot.button.onClick.SetListener(() => {
                            if (slot.item.data is UsableItem usable && usable.CanUse(player, icopy,slot.item)){
                                print(slot.item.data.GetType());
                                // 调用使用道具的方法 
                                player.equipment.CmdUseItem(icopy);
                            }  
                        });

                        // 装备鼠标经过提示
                        uislot.tooltip.enabled = true;
                        if (uislot.tooltip.IsVisible())
                            uislot.tooltip.text = slot.ToolTip();
                        // 装备可拖拽
                        uislot.dragAndDropable.dragable = true;

                        // 装备类道具，耐久度颜色
                        if (slot.item.maxDurability > 0)
                        {
                            if (slot.item.durability == 0)
                                uislot.image.color = brokenDurabilityColor;
                            else if (slot.item.DurabilityPercent() < lowDurabilityThreshold)
                                uislot.image.color = lowDurabilityColor;
                            else
                                uislot.image.color = Color.white;
                        }
                        else uislot.image.color = Color.white; 
                        uislot.image.sprite = slot.item.image;

                        // 有冷确时间的道具可使用装备
                        if (slot.item.data is UsableItem usable2)
                        {
                            float cooldown = player.GetItemCooldown(usable2.cooldownCategory);
                            uislot.cooldownCircle.fillAmount = usable2.cooldown > 0 ? cooldown / usable2.cooldown : 0;
                        }
                        else uislot.cooldownCircle.fillAmount = 0;

                        // 可堆叠装备数量，如果只是一件不显示堆叠数量
                        uislot.amountOverlay.SetActive(slot.amount > 1);
                        uislot.amountText.text = slot.amount.ToString();
                    }
                    /********************************************************************/
                    /* 刷新无道具装备格子                                                   *                        
                    /*==================================================================*/
                    else
                    {
                        // 刷新无效项
                        uislot.tooltip.enabled = false;
                        uislot.dragAndDropable.dragable = false;
                        uislot.image.color = Color.clear;
                        uislot.image.sprite = null;
                        uislot.cooldownCircle.fillAmount = 0;
                        uislot.amountOverlay.SetActive(false);
                    }
                }
            }
        }
        else panel.SetActive(false);
    }
}
