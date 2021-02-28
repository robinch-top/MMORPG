using UnityEngine;
using System.Collections;

namespace Assets.MMORPG.Scripts.RPGGame.Base
{
    public class NetBehaviourNonAlloc : MonoBehaviour
    {

        // 获取和设置MonoBehaviour对象的base name
        string cachedName;
        /// <summary>
        /// 角色中文名称  
        /// </summary>
        public string CNName;
        public new string name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(cachedName))
                    cachedName = base.name;
                return cachedName;
            }
            set
            {
                cachedName = base.name = value;
            }
        }
    }
}