using UnityEngine;
using UnityEngine.UI;
namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    // 装备格子预制体
    // 绑定到预置体，以便通过UI脚本更容易地访问。
    // 否则我们需要slot.GetChild（0）.GetComponentInChildren<Text>等。
    /// <summary>
    /// 装备格子预制体
    /// </summary>
    public class UIEquipmentSlot : MonoBehaviour
    {
        public UIShowToolTip tooltip;
        public Button button;
        public Image image;
        public Image cooldownCircle;
        public UIDragAndDropable dragAndDropable;
        public GameObject amountOverlay;
        public Text amountText;
        public GameObject categoryOverlay;
        public Text categoryText;
    }
}