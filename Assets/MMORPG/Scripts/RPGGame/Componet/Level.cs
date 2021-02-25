using UnityEngine;
using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.Componet
{
    [DisallowMultipleComponent]
    public class Level : MonoBehaviour
    {
        /// <summary>
        /// 角色当前等级
        /// </summary>
        public int current = 1;
        /// <summary>
        /// 角色最高等级
        /// </summary>
        public int max = 1;

        //编辑器模式下OnValidate 仅在下面两种情况下被调用：
        //脚本被加载时
        //Inspector 中的任何值被修改时
        void OnValidate()
        {
            current = Mathf.Clamp(current, 1, max);
        }
    }
}
