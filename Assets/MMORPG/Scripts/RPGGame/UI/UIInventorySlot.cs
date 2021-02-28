using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    public class UIInventorySlot : MonoBehaviour
    {

        public UIShowToolTip tooltip;
        public Button button;
        public UIDragAndDropable dragAndDropable;
        public Image image;
        public Image cooldownCircle;
        public GameObject amountOverlay;
        public Text amountText;
    }
}