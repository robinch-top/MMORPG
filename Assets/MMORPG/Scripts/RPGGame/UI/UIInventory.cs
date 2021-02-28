using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    public partial class UIInventory : MonoBehaviour
    {

        public static UIInventory singleton;
        public KeyCode hotKey = KeyCode.I;
        public GameObject panel;
        public UIInventorySlot slotPrefab;
        public Transform content;
        public Text goldText;

        public UIInventory()
        {
            if (singleton == null) singleton = this;
        }
          
    }
}