using UnityEngine;
using System;  // Activator 命名空间
using System.Collections; // ArrayList 命名空间
using System.Collections.Generic; // Dictionary 命名空间
using System.Linq; // 可使用 ToArray方法

public class SimObject {
    public Vector3 position;
    public string name;

    /// 用一个变量记录(也可以叫保存或持有)，添加的这些组件的引用
    public Dictionary<Type, SComponet> componentDict = new Dictionary<Type, SComponet>();

    public K AddComponet<K>() where K : SComponet, new (){
        // 创建组件的实例
        K component = Activator.CreateInstance(typeof (K)) as K;
        
        // 给一个对象添加组件的目的之一：
            // 把自己的引用给到组件，从而组件的方法可以作用于此对象
        component.DriveStart(this);

        // 给一个对象添加组件的目的之二：
            // 添加到对象的组件list，从而可以取得或移除组件
        componentDict.Add(typeof (K),component);
        return component;
    }
    
    public K GetComponent<K>() where K : SComponet
    {
        SComponet component;
        if (!this.componentDict.TryGetValue(typeof (K), out component))
        {
            return null;
        }
        return (K)component;
    }

    public K[] GetComponents<K>() where K : SComponet
    {
        ArrayList list = new ArrayList();
        for (int i = 0; i < componentDict.Count; i++)
        {
            K component = componentDict.ElementAt(i) as K;
            if (typeof(K) == component.GetType())
                list.Add(component);
        }
        return (K[])list.ToArray();
    }
}