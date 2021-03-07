using UnityEngine;
using System;

//玩家经验组件
public class PlayerExperience : Experience
{
    void Start(){
        // 测试升级所需经验每级增加13% 的算法
        long ex = Convert.ToInt64(100*Mathf.Pow(1.13f,2-1));
        Debug.Log($"2级需要的经验:{ex}");
        ex = Convert.ToInt64(100*Mathf.Pow(1.13f,3-1));
        Debug.Log($"3级需要的经验:{ex}");
        ex = Convert.ToInt64(100*Mathf.Pow(1.13f,4-1));
        Debug.Log($"4级需要的经验:{ex}");
        ex = Convert.ToInt64(100*Mathf.Pow(1.13f,5-1));
        Debug.Log($"5级需要的经验:{ex}");
        ex = Convert.ToInt64(100*Mathf.Pow(1.13f,60-1));
        Debug.Log($"59级需要的经验:{ex}");

        // 测试当前等级与经验，减50经验
        //current -= 50;

        // 测试当前等级与经验，加200经验
        //current += 200;
    }
    
    public override void OnDeath()
    {
        // 调用父类死亡方法，把所有有经验组件的角色，死亡后会引发的逻辑写在经验组件的父类中
        // 比如死亡扣经验的逻辑
        base.OnDeath();

        // 在聊天中输出一些信息
        // string message = "You died and lost experience.";
        // chat.TargetMsgInfo(message);
    }
}
