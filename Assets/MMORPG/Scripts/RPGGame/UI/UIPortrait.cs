using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    public class UIPortrait : MonoBehaviour
    {
        public GameObject panel;
        public Image image;

        void Update()
        {
            Entity.Player player = Entity.Player.localPlayer;
            if (player != null)
            {
                panel.SetActive(true);

                //设置Image组件的sprite属性,即可指定头像
                image.sprite = player.portraitIcon;
            }
            else panel.SetActive(false);
        }
    }
}
