using UnityEngine;
using UnityEngine.UI;

namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    public class UIShortcuts : MonoBehaviour
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


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
           Entity.Player player = Entity.Player.localPlayer;
            if (player)
            {

                panel.SetActive(true);
                Base.Extensions.SetListener(inventoryButton.onClick, () => {
                    print("inventoryPanel");
                    inventoryPanel.SetActive(!inventoryPanel.activeSelf);
                });
                //inventoryButton.onClick.AddListener(() => {
                //    print("inventoryPanel");
                //    inventoryPanel.SetActive(!inventoryPanel.activeSelf);
                //});

                Base.Extensions.SetListener(equipmentButton.onClick,() => {
                    equipmentPanel.SetActive(!equipmentPanel.activeSelf);
                });

                Base.Extensions.SetListener(skillsButton.onClick,() => {
                    skillsPanel.SetActive(!skillsPanel.activeSelf);
                });

                Base.Extensions.SetListener(characterInfoButton.onClick,() => {
                    characterInfoPanel.SetActive(!characterInfoPanel.activeSelf);
                });

                Base.Extensions.SetListener(questsButton.onClick,() => {
                    questsPanel.SetActive(!questsPanel.activeSelf);
                });

                Base.Extensions.SetListener(partyButton.onClick,() => {
                    partyPanel.SetActive(!partyPanel.activeSelf);
                });

                Base.Extensions.SetListener(guildButton.onClick,() => {
                    guildPanel.SetActive(!guildPanel.activeSelf);
                });
                Base.Extensions.SetListener(itemMallButton.onClick,() => {
                    itemMallPanel.SetActive(!itemMallPanel.activeSelf);
                });

                quitButton.GetComponent<UIShowToolTip>().text = "Quit";
                quitButton.interactable = true;
                quitButton.onClick.AddListener(() => {
                    //NetworkManagerMMO.Quit();
                });
            }
            else panel.SetActive(false);
        }
    }
}