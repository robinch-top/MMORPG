using UnityEngine;
using UnityEngine.UI;

// 快捷工具条UI组件
// 快捷工具条上的按钮是不能随意拖放位置的,这不同于Toolsbar(道具技能工具条)
public partial class UIShortcuts : MonoBehaviour
{
    public GameObject panel;

    public Button inventoryButton;
    public GameObject inventoryPanel;

    public Button equipmentButton;
    public GameObject equipmentPanel;

    public Button skillsButton;
    public GameObject skillsPanel;

    public Button characterInfoButton;
    public GameObject characterInfoPanel;

    public Button questsButton;
    public GameObject questsPanel;

    public Button guildButton;
    public GameObject guildPanel;

    public Button partyButton;
    public GameObject partyPanel;

    public Button itemMallButton;
    public GameObject itemMallPanel;

    public Button gameMasterButton;
    public GameObject gameMasterPanel;

    public Button quitButton;

    void Update()
    {
        Player player = Player.localPlayer;
        if (player)
        {
            
            panel.SetActive(true);

            inventoryButton.onClick.SetListener(() => {
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            });

            equipmentButton.onClick.SetListener(() => {
                equipmentPanel.SetActive(!equipmentPanel.activeSelf);
            });

            skillsButton.onClick.SetListener(() => {
                skillsPanel.SetActive(!skillsPanel.activeSelf);
            });

            characterInfoButton.onClick.SetListener(() => {
                characterInfoPanel.SetActive(!characterInfoPanel.activeSelf);
            });

            questsButton.onClick.SetListener(() => {
                questsPanel.SetActive(!questsPanel.activeSelf);
            });

            partyButton.onClick.SetListener(() => {
                partyPanel.SetActive(!partyPanel.activeSelf);
            });

            guildButton.onClick.SetListener(() => {
                guildPanel.SetActive(!guildPanel.activeSelf);
            });

            itemMallButton.onClick.SetListener(() => {
                itemMallPanel.SetActive(!itemMallPanel.activeSelf);
            });

            quitButton.GetComponent<UIShowToolTip>().text = "Quit";
            quitButton.interactable = true;
            
        }
        else panel.SetActive(false);
    }
}