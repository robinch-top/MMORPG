using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// 道具的资源数据对象基类，可以在系统create菜单中创建ScriptableItem
// 技能，装备，各种道具都继承ScriptableItem

[CreateAssetMenu(menuName="uMMORPG Item/General", order=999)]
public partial class ScriptableItem : ScriptableNonAlloc
{
    /// <summary>
    /// 道具最大堆叠数
    /// </summary>
    public int maxStack;
    /// <summary>
    /// 道具名
    /// </summary>
    public string itemName;
    /// <summary>
    /// 道具编号，不可重复
    /// </summary>
    public int itemID;
    
    [Tooltip("耐久性仅适用于不可堆叠的项目（如MaxStack为1）")]
    public int maxDurability = 0; // disabled by default
    public long buyPrice;
    public long sellPrice;
    public long itemMallPrice;
    public bool sellable;
    public bool tradable;
    public bool destroyable;

    [SerializeField, TextArea(1, 30)] protected string toolTip;
    public Sprite image;

    public virtual string ToolTip()
    {
        StringBuilder tip = new StringBuilder(toolTip);
        tip.Replace("{NAME}", name);
        tip.Replace("{ItemName}", itemName);
        tip.Replace("{DESTROYABLE}", (destroyable ? "Yes" : "No"));
        tip.Replace("{SELLABLE}", (sellable ? "Yes" : "No"));
        tip.Replace("{TRADABLE}", (tradable ? "Yes" : "No"));
        tip.Replace("{BUYPRICE}", buyPrice.ToString());
        tip.Replace("{SELLPRICE}", sellPrice.ToString());
        return tip.ToString();
    }

    // dict //////////////////////////////////////////////////////////////
    // -> 获取与缓存道具的ScriptableItem实例
    // 所有的道具，装备，技能等的ScriptableItem对象都定义在\Assets\MMORPG\Resources目录下
    static Dictionary<int, ScriptableItem> cache;
    public static Dictionary<int, ScriptableItem> dict
    {
        get
        {
            // not loaded yet?
            if (cache == null)
            {
                // 从 Resources/Items 下加载所有的 ScriptableItem
                ScriptableItem[] items = Resources.LoadAll<ScriptableItem>("Items");

                // 检查重复项，然后添加到缓存
                List<string> duplicates = items.ToList().FindDuplicates(item => item.name);
                if (duplicates.Count == 0)
                {
                    cache = items.ToDictionary(item => item.itemID, item => item);
                }
                else
                {
                    foreach (string duplicate in duplicates)
                        Debug.LogError("资源文件夹包含多个名为" + duplicate + "的ScriptableItem");
                }
            }
            return cache;
        }
    }

    // validation //////////////////////////////////////////////////////////////
    void OnValidate()
    {
        //max stack 最大弹药堆叠数
        //max durability 最大耐久
    }
}