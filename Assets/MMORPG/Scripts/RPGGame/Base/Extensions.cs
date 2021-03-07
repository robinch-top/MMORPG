using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;

public static class Extensions
{
    // string to int 
    public static int ToInt(this string value, int errVal=0)
    {
        Int32.TryParse(value, out errVal);
        return errVal;
    }

    // string to long
    public static long ToLong(this string value, long errVal=0)
    {
        Int64.TryParse(value, out errVal);
        return errVal;
    }

    // UI SetListener扩展，删除以前的侦听器，然后添加新的侦听器
    //（此版本用于onClick等）
    public static void SetListener(this UnityEvent uEvent, UnityAction call)
    {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call);
    }

    // UI SetListener扩展，删除以前的侦听器，然后添加新的侦听器
    //（此版本适用于onededit、onValueChanged等）
    public static void SetListener<T>(this UnityEvent<T> uEvent, UnityAction<T> call)
    {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call);
    }

    //NavMeshAgent帮助函数，返回指定目的地。这对click&wsad移动非常有用
    //因为玩家可能会点击进入各种无法行走的路径：
    //       ________
    //      |xxxxxxx|
    //      |x|   |x|
    // P   A|B| C |x|
    //      |x|___|x|
    //      |xxxxxxx|
    //
    // 如果player在位置P并试图去：
    // A:这条路可以走，一切都很好
    // C:C在导航网上，但我们不能直接到达那里。CalulatePath将返回A作为最后一个可行走点
    // B:B不在NavMesh上，CalulatePath在这里不起作用。我们需要查找NavMesh上最近的点（可能是a或C），
    // 然后计算最接近的有效值（A）
    public static Vector3 NearestValidDestination(this NavMeshAgent agent, Vector3 destination)
    {
        // 能计算出那里的路径吗？然后返回最近的有效点
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(destination, path))
            return path.corners[path.corners.Length - 1];

        // 否则，请先找到最近的navmesh位置。我们用速度*2为半径
        // 效果很好。然后找到最近的有效点。
        if (NavMesh.SamplePosition(destination, out NavMeshHit hit, agent.speed * 2, NavMesh.AllAreas))
            if (agent.CalculatePath(hit.position, path))
                return path.corners[path.corners.Length - 1];

        // 什么都没用，哪儿也别去。
        return agent.transform.position;
    }

    // 需要一个真正停止所有运动的方法。
    public static void ResetMovement(this NavMeshAgent agent)
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }

    // 检查列表是否有重复项
    public static bool HasDuplicates<T>(this List<T> list)
    {
        return list.Count != list.Distinct().Count();
    }

    // 查找列表中的所有重复项
    // 注意：这只在开始时调用一次，所以Linq在这里很好！
    public static List<U> FindDuplicates<T, U>(this List<T> list, Func<T, U> keySelector)
    {
        return list.GroupBy(keySelector)
                   .Where(group => group.Count() > 1)
                   .Select(group => group.Key).ToList();
    }

    // string.GetHashCode 不能保证在所有机器上都是相同的，但是
    // 我们需要一个在所有机器上都一样的。这是一个简单的方法：
    public static int GetStableHashCode(this string text)
    {
        unchecked
        {
            int hash = 23;
            foreach (char c in text)
                hash = hash * 31 + c;
            return hash;
        }
    }
}