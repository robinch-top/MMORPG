using UnityEngine;

public class SimMono: MonoBehaviour {
    public GameObject simObject;
    int y = 0;

    // 定义一个Start方法，Mono会帮我们执行它
    // 只是给GameObject取了一个名字
    void Start(){
        simObject = this.gameObject;
        simObject.name = "baobao";
    }
    
    // 定义一个Update方法，Mono会帮我们执行它
    // 只是不断在改变GameObject Y轴上的位置
    void Update(){
        simObject.transform.position = new Vector3(0,y++,0);
        Debug.Log($"{simObject.name}的位置{simObject.transform.position}");
    }
}