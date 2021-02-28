using UnityEngine;
using System.Collections;

namespace Assets.MMORPG.Scripts.RPGGame.Base
{
    public class ScriptableNonAlloc : ScriptableObject
    {

        // 获取ScriptableObject的base name
        string cachedName;
        public new string name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(cachedName))
                    cachedName = base.name;
                return cachedName;
            }
        }
    }
}