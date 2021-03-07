using UnityEngine;
using UnityEngine.UI;

//角色头像UI组件
//这里使用的是一个静态图片
public partial class UIPortrait : MonoBehaviour
{
    public GameObject panel;
    public Image image;

    void Update()
    {
        Player player = Player.localPlayer;
        if (player !=null)
        {
            panel.SetActive(true);

            //设置Image组件的sprite属性,即可指定头像
            image.sprite = player.portraitIcon;
        }
        else panel.SetActive(false);
    }
}
