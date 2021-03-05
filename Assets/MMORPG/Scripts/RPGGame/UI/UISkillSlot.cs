﻿using UnityEngine;
using UnityEngine.UI;
namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    public class UISkillSlot : MonoBehaviour
    {
        public UIShowToolTip tooltip;
        public UIDragAndDropable dragAndDropable;
        public Image image;
        public Button button;
        public GameObject cooldownOverlay;
        public Text cooldownText;
        public Image cooldownCircle;
        public Text descriptionText;
        public Button upgradeButton;
        public Button descripButton;
    }
}
