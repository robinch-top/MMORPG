using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// 网络管理组件基类

namespace Mirror
{
    public class NetworkManager : MonoBehaviour
    {
        public int serverTickRate = 30;

        [FormerlySerializedAs("m_SpawnPrefabs"), HideInInspector]
        public List<GameObject> spawnPrefabs = new List<GameObject>();

        // 不同职业角色的出生点位置，暂时用不上，用了一个SpawnLocation的位置
        public static int startPositionIndex;
        public static List<Transform> startPositions = new List<Transform>();

        public static void RegisterStartPosition(Transform start)
        {
            startPositions.Add(start);
            startPositions = startPositions.OrderBy(transform => transform.GetSiblingIndex()).ToList();
        }

        public static void UnRegisterStartPosition(Transform start)
        {
            startPositions.Remove(start);
        }
    }
}

