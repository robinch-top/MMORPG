using UnityEngine;

public class SimComponet : SComponet {
    public SimObject simObject;
    int y = 0;

    // 重写Start方法，SComponet会帮我们执行它
    // 只是给SimObject取了一个名字
    public override void Start(){
        simObject = this.gameObject;
        simObject.name = "guaiguai";
    }
    
    // 重写Update方法，SComponet会帮我们执行它
    // 只是不断在改变SimObject Y轴上的位置
    public override void Update(){
        simObject.position = new Vector3(0,y++,0);
        Debug.Log($"{simObject.name}的位置{simObject.position}");
    }
}