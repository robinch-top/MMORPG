using UnityEngine;
using System.Collections;

namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    public class UIUtils : MonoBehaviour
    {
        public static void BalancePrefabs(GameObject prefab, int amount, Transform parent)
        {
            //在父项下实例化直到达到amount数量
            for (int i = parent.childCount; i < amount; ++i)
            {
                GameObject.Instantiate(prefab, parent, false);
            }

            //删除过多的内容
            //（向后循环，因为Destroy更改了childCount）
            for (int i = parent.childCount - 1; i >= amount; --i)
                GameObject.Destroy(parent.GetChild(i).gameObject);
        }
        public static bool AnyInputActive()
        {
            return true;
        }
    }
}