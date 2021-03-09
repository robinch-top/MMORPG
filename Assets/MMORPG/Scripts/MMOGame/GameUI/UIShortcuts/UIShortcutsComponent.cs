using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
namespace MMOGame
{
    [ObjectSystem]
    public class UIShortcutsComponentSystem : AwakeSystem<UIShortcutsComponent>
    {
        public override void Awake(UIShortcutsComponent self)
        {
            self.Awake();
        }
    }
    public class UIShortcutsComponent : ETModel.Component
    {
        GameObject inventoryPanel;
        GameObject equipmentPanel;
        GameObject skillsPanel;
        GameObject characterInfoPanel;

        GameObject questsPanel;
        GameObject guildPanel;
        GameObject partyPanel;
        GameObject itemMallPanel;
        GameObject gameMasterPanel;

        Button CharaInfoButton;
        Button inventoryButton;
        Button equipmentButton;
        Button skillsButton;

        Button questsButton;
        Button guildButton;
        Button partyButton;
        Button itemMallButton;
        Button gameMasterButton;
        Button quitButton;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            // 初始化数据
            CharaInfoButton = rc.Get<GameObject>("ButtonCharacterInfo").GetComponent<Button>();
            inventoryButton = rc.Get<GameObject>("ButtonInventory").GetComponent<Button>();
            equipmentButton = rc.Get<GameObject>("ButtonEquipment").GetComponent<Button>();
            skillsButton = rc.Get<GameObject>("ButtonSkills").GetComponent<Button>();

            // 获取界面
            ReferenceCollector panelrc = GameObject.Find("Canvas").GetComponent<ReferenceCollector>();
            inventoryPanel = panelrc.Get<GameObject>("InventoryPanel");
            equipmentPanel = panelrc.Get<GameObject>("EquipmentPanel");
            skillsPanel = panelrc.Get<GameObject>("SkillsPanel");
            characterInfoPanel = panelrc.Get<GameObject>("RoleInfoPanel");

            // 添加事件
            inventoryButton.onClick.Add(() => InventoryBtnOnClick());
            equipmentButton.onClick.Add(() => EquipmentBtnOnClick());
            skillsButton.onClick.Add(() => SkillsBtnOnClick());
            CharaInfoButton.onClick.Add(() => CharaInfoBtnOnClick());
            rc.Get<GameObject>("ButtonQuit").GetComponent<Button>().onClick.Add(() => QuitBtnOnClick());
        }

        public void QuitBtnOnClick()
        {
            Game.EventSystem.Run(EventIdType.BackSelection);
        }

        public void InventoryBtnOnClick()
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
        public void EquipmentBtnOnClick()
        {
            equipmentPanel.SetActive(!equipmentPanel.activeSelf);
        }
        public void SkillsBtnOnClick()
        {
            skillsPanel.SetActive(!skillsPanel.activeSelf);
        }
        public void CharaInfoBtnOnClick()
        {
            characterInfoPanel.SetActive(!characterInfoPanel.activeSelf);
        }

    }
}
