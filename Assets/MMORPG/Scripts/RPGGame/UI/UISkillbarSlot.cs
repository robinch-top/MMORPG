﻿using UnityEngine;
using UnityEngine.UI;
namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    public class UISkillbarSlot : MonoBehaviour
    {
        public UIShowToolTip tooltip;
        public UIDragAndDropable dragAndDropable;
        public Image image;
        public Button button;
        public GameObject cooldownOverlay;
        public Text cooldownText;
        public Image cooldownCircle;
        public GameObject amountOverlay;
        public Text amountText;
        public Text hotkeyText;
    }
}
