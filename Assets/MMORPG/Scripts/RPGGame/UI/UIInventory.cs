using UnityEngine;
using UnityEngine.UI;

// 背包UI组件
public partial class UIInventory : MonoBehaviour
{
    public static UIInventory singleton;
    public KeyCode hotKey = KeyCode.I;
    public GameObject panel;
    public UIInventorySlot slotPrefab;
    public Transform content;
    public Text goldText;

    [Header("Durability Colors")]
    public Color brokenDurabilityColor = Color.red;
    public Color lowDurabilityColor = Color.magenta;
    [Range(0.01f, 0.99f)] public float lowDurabilityThreshold = 0.1f;

    public UIInventory()
    {
        if (singleton == null) singleton = this;
    }

    void Update()
    {
        Player player = Player.localPlayer;
        if (player != null)
        {
            // 背包开关快捷按键
            if (Input.GetKeyDown(hotKey) && !UIUtils.AnyInputActive())
                panel.SetActive(!panel.activeSelf);

            // 背包界面显示时才更新背包中的道具
            if (panel.activeSelf)
            {
                // 维持背包界面Content下面足够的格子实例
                UIUtils.BalancePrefabs(slotPrefab.gameObject, player.inventory.slots.Count, content);

                // 刷新背包中的道具
                for (int i = 0; i < player.inventory.slots.Count; ++i)
                {
                    UIInventorySlot uislot = content.GetChild(i).GetComponent<UIInventorySlot>();
                    uislot.dragAndDropable.name = i.ToString(); // drag and drop index
                    ItemSlot slot = player.inventory.slots[i];

                    /********************************************************************/
                    /* 刷新有道具格子                                                      *                        
                    /*==================================================================*/
                    if (slot.amount > 0)
                    {
                        // 在背包中使用道具的点击事件
                        int icopy = i; 
                        uislot.button.onClick.SetListener(() => {
                            if (slot.item.data is UsableItem usable && usable.CanUse(player, icopy,slot.item)){
                                print(slot.item.data.GetType());
                                // 调用使用道具的方法 
                                player.inventory.CmdUseItem(icopy);
                            }  
                        });
                        
                        // 道具鼠标经过提示
                        uislot.tooltip.enabled = true;
                        if (uislot.tooltip.IsVisible())
                            uislot.tooltip.text = slot.ToolTip();
                        // 道具可拖拽
                        uislot.dragAndDropable.dragable = true;

                        // 装备类道具，根据不同的耐久度显示不同的颜色效果
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

                        // 有冷确时间的道具的冷确效果cooldown
                        if (slot.item.data is UsableItem usable2)
                        {
                            float cooldown = player.GetItemCooldown(usable2.cooldownCategory);
                            uislot.cooldownCircle.fillAmount = usable2.cooldown > 0 ? cooldown / usable2.cooldown : 0;
                        }
                        else uislot.cooldownCircle.fillAmount = 0;
                        
                        // 道具堆叠数量，如果只是一件不显示堆叠数量
                        uislot.amountOverlay.SetActive(slot.amount > 1);
                        uislot.amountText.text = slot.amount.ToString();
                    }
                    /********************************************************************/
                    /* 刷新无道具格子                                                      *                        
                    /*==================================================================*/
                    else
                    {
                        // 刷新无效项
                        uislot.button.onClick.RemoveAllListeners();
                        uislot.tooltip.enabled = false;
                        uislot.dragAndDropable.dragable = false;
                        uislot.image.color = Color.clear;
                        uislot.image.sprite = null;
                        uislot.cooldownCircle.fillAmount = 0;
                        uislot.amountOverlay.SetActive(false);
                    }
                }

                // gold
                goldText.text = player.gold.ToString();
            }
        }
        else panel.SetActive(false);
    }
}
