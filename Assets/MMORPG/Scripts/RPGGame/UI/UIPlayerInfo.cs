using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    public class UIPlayerInfo : MonoBehaviour
    {
        public GameObject panel;
        public KeyCode hotKey = KeyCode.I;

        public Text level;
        public Text health;
        public Text mana;

        public Text endurance;
        public Text intellect;
        public Text strength;
        public Text spirit;
        public Text agility;

        public Text attack;
        public Text physical;
        public Text magic;
        public Text armor;
        public Text dePhysical;
        public Text deMagic;
        public Text dodge;

        public Text magicHit;
        public Text physicHit;
        public Text magicCrit;
        public Text physicCrit;

        public Text healthRecover;
        public Text manaRecover;


        // Update is called once per frame
        void Update()
        {
            Entity.Player player = Entity.Player.localPlayer;
            if (player != null)
            {

                if (Input.GetKeyDown(hotKey) && !UIUtils.AnyInputActive())
                    panel.SetActive(!panel.activeSelf);

                // only update the panel if it's active
                if (panel.activeSelf)
                {
                    level.text = player.level.current.ToString();
                    health.text = player.health.current.ToString() + " / " + player.health.max;
                    mana.text = player.mana.current.ToString() + " / " + player.mana.max;

                    endurance.text = player.health.endurance.ToString();
                    intellect.text = player.mana.intellect.ToString();
                    agility.text = player.combat.agility.ToString();
                    strength.text = player.combat.strength.ToString();
                    spirit.text = player.combat.spirit.ToString();

                    attack.text = player.combat.attackStrength.ToString();
                    physical.text = player.combat.physicalDamage.ToString();
                    magic.text = player.combat.magicDamage.ToString();

                    armor.text = player.combat.armorDefense.ToString();
                    dePhysical.text = player.combat.physicalDefense.ToString();
                    deMagic.text = player.combat.magicDefense.ToString();
                    dodge.text = player.combat.dodgeChance.ToString("F2");

                    magicHit.text = player.combat.magicHitrate.ToString("F2");
                    physicHit.text = player.combat.physicHitrate.ToString("F2");
                    magicCrit.text = player.combat.magicCritical.ToString("F2");
                    physicCrit.text = player.combat.physicCritical.ToString("F2");

                    healthRecover.text = player.health.recovery.ToString();
                    manaRecover.text = player.mana.recovery.ToString();
                }
            }
            else panel.SetActive(false);
        }
    }
}
