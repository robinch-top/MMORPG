using UnityEngine;
using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.Player
{
    public class PlayerIndicator : MonoBehaviour
    {
        [Header("Indicator")]
        public GameObject indicatorPrefab;
        [HideInInspector] public GameObject indicator;

        public void SetViaParent(Transform parent)
        {
            if (!indicator) indicator = Instantiate(indicatorPrefab);
            indicator.transform.SetParent(parent, true);
            indicator.transform.position = parent.position;
        }

        public void SetViaPosition(Vector3 position)
        {
            if (!indicator) indicator = Instantiate(indicatorPrefab);
            indicator.transform.parent = null;
            indicator.transform.position = position;
        }

        // 清除指示器,当指示器所在目标死亡或消失
        public void ClearIfNoParent()
        {
            if (indicator != null && indicator.transform.parent == null)
                Destroy(indicator);
        }

        // 清除指示器
        public void Clear()
        {
            if (indicator != null)
                Destroy(indicator);
        }

        void OnDestroy()
        {
            if (indicator != null)
                Destroy(indicator);
        }
    }
}
