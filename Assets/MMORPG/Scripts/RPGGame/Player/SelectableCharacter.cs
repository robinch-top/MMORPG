using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerIndicator))]
[DisallowMultipleComponent]
public class SelectableCharacter : MonoBehaviour
{
    // 角色被选择时将被设置
    public int index = -1;

    void OnMouseDown()
    {
        // set selection index
        RPGManager.singleton.selection = index;
        RPGManager.singleton.CharaId = GetComponent<Player>().CharaId;

        // 显示角色模型脚下的选中标识
        GetComponent<PlayerIndicator>().SetViaParent(transform);
    }

    void Update()
    {
        // 移除选中标识
        if ( RPGManager.singleton.selection != index)
        {
            GetComponent<PlayerIndicator>().Clear();
        }
    }
}
