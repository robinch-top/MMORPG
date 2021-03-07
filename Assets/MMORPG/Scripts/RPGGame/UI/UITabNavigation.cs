using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//用tab键盘切换界面中的输入框子与按钮焦点
//支持界面中的InputField,Button
//将脚本挂在离界面中所有按钮与输入框最近的父级物体上（权衡即能查到按钮又能查到输入框标准的最近），减少查询他们的深度
public class UITabNavigation : MonoBehaviour
{

    private EventSystem es;

    private GameObject[] IfArray;

    private int index = 0;

    void Start() {
        es = EventSystem.current;

        InputField[] inputarr = this.GetComponentsInChildren<InputField>();
        Button[] butarr = this.GetComponentsInChildren<Button>();

        int count = inputarr.Length+butarr.Length;
        IfArray = new GameObject[count];

        //将按钮与输入框的gameobject添加到IfArray中
        for(int i=0;i<inputarr.Length;++i){     
            IfArray.SetValue(inputarr[i].gameObject,i);
        }
        
        for(int k=0;k<butarr.Length;++k){
            IfArray.SetValue(butarr[k].gameObject,inputarr.Length+k);
        }

        es.SetSelectedGameObject(IfArray[index].gameObject,new BaseEventData(es));
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            index++;
            if (index >= IfArray.Length) {
                index = 0;
            }
            es.SetSelectedGameObject(IfArray[index].gameObject, new BaseEventData(es));           
        }
    }
}
