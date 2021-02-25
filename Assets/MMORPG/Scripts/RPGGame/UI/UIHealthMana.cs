using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    public class UIHealthMana : MonoBehaviour
    {
        public GameObject panel;
        public Slider healthSlider;
        public Text healthStatus;
        public Slider manaSlider;
        public Text manaStatus;

        void Update()
        {
            Entity.Player player = Entity.Player.localPlayer;
            if (player != null)
            {
                panel.SetActive(true);

                // 暂写死满血,满蓝都为200 来测试UI效果
                healthSlider.value = (float)player.health / 200f;
                healthStatus.text = player.health + " / " + 200;

                manaSlider.value = (float)player.mana / 200f;
                manaStatus.text = player.mana + " / " + 200;
            }
            else panel.SetActive(false);
        }
    }
}
