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


                healthSlider.value = player.health.Percent();
                healthStatus.text = player.health.current + " / " + player.health.max;

                manaSlider.value = player.mana.Percent();
                manaStatus.text = player.mana.current + " / " + player.mana.max;
            }
            else panel.SetActive(false);
        }
    }
}
