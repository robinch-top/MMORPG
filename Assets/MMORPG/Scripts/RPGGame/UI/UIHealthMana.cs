using UnityEngine;
using UnityEngine.UI;

//角色生命与蓝条UI组件
public partial class UIHealthMana : MonoBehaviour
{
    public GameObject panel;
    public Slider healthSlider;
    public Text healthStatus;
    public Slider manaSlider;
    public Text manaStatus;

    void Update()
    {
        Player player = Player.localPlayer;
        if (player !=null)
        {
            panel.SetActive(true);

            // 调用角色生命属性,当前生命与最大生命比值
            healthSlider.value = player.health.Percent();
            healthStatus.text = player.health.current + " / " + player.health.max;

            // 调用角色蓝量属性,当前蓝量与最大蓝量比值
            manaSlider.value = player.mana.Percent();
            manaStatus.text = player.mana.current + " / " + player.mana.max;
        }
        else panel.SetActive(false);
    }
}
