
using UnityEngine;
using UnityEngine.UI;
namespace Assets.MMORPG.Scripts.RPGGame.UI
{

    // 装备界面UI组件
    public partial class UIEquipment : MonoBehaviour
    {
        public KeyCode hotKey = KeyCode.U;
        public GameObject panel;
        public UIEquipmentSlot slotPrefab;
        public Transform content;

        void Update()
        {
            Entity.Player player = Entity.Player.localPlayer;
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
                        uislot.dragAndDropable.name = i.ToString();
                        Items.ItemSlot slot = player.equipment.slots[i];

                        // 显示装备部位名称
                        Data.EquipSlotInfo slotInfo = player.equipment.slotInfo[i];
                        uislot.categoryOverlay.SetActive(slotInfo.requiredCategory != "");
                        string overlay = slotInfo.catName;
                        uislot.categoryText.text = overlay != "" ? overlay : "?";

                        /********************************************************************/
                        /* 刷新装备格子                                                       *                        
                        /*==================================================================*/
                        if (slot.amount > 0)
                        {
                            // 装备鼠标经过提示
                            uislot.tooltip.enabled = true;
                            if (uislot.tooltip.IsVisible())
                                uislot.tooltip.text = slot.ToolTip();
                            // 装备可拖拽
                            uislot.dragAndDropable.dragable = true;
                            // 装备类道具，耐久度颜色
                            // ...

                            uislot.image.color = Color.white;
                            uislot.image.sprite = slot.item.image;

                            // 有冷确时间的道具可使用装备
                            // ...

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
                            uislot.dragAndDropable.dragable = false;
                            uislot.tooltip.enabled = false;
                            uislot.image.color = Color.clear;
                            uislot.image.sprite = null;
                            uislot.amountOverlay.SetActive(false);
                        }
                    }
                }
            }
            else panel.SetActive(false);
        }
    }
}