using UnityEngine;
public abstract class NetBehaviourNonAlloc : NetworkBehaviour
{
    // 获取和设置MonoBehaviour对象的base name
    string cachedName;
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
