// 背包格子预制体
// 绑定到预置体，以便通过UI脚本更容易地访问。
// 否则我们需要slot.GetChild（0）.GetComponentInChildren<Text>等。
using UnityEngine;
using UnityEngine.UI;

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
