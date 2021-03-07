using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName="uMMORPG Item/Weapon", order=999)]
public class WeaponItem : EquipmentItem
{
    [Header("Weapon")]

    /// <summary>
    /// 武器对弹药道具的要求,比如枪械、弓驽武器就对弹药道具有要求
    /// </summary>
    public AmmoItem requiredAmmo;

    // tooltip
    public override string ToolTip()
    {
        StringBuilder tip = new StringBuilder(base.ToolTip());
        if (requiredAmmo != null)
            tip.Replace("{REQUIREDAMMO}", requiredAmmo.name);
        return tip.ToString();
    }
}
