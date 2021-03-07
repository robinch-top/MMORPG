//ThirdPerson camera control
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float distance = 12.5f;
    public float minDistance = 3;
    public float maxDistance = 20;

    public float XSensitivity = 2;
    public float YSensitivity = 2;
    public float MinimumX = -90;
    public float MaximumX = 90;

    public int mouseFreeLookButton = 0; // left button by default
    public int mouseRotateButton = 1; // right button by default

    public Vector2 thirdPersonOffset = Vector2.up;
    public Vector2 thirdPersonOffsetMultiplier = Vector2.zero;
    public LayerMask viewBlockingLayers;

    new Camera camera;
    Vector3 lastForward;
    Vector3 originalCameraPosition;

    public Transform firstPersonParent;
    public Vector3 headPosition => firstPersonParent.position;

    void Awake()
    {
        camera = Camera.main;
    }
    
    void Start()
    {
        // 记住动画的lastForward
        lastForward = transform.forward;

        // 将摄影机父对象设置为 player
        camera.transform.SetParent(transform, false);

        // 观察Player的前进方向
        camera.transform.rotation = transform.rotation;

        // 将摄像头设置到头部位置
        camera.transform.position = headPosition;
        
        // 记住相机的原始位置
        originalCameraPosition = camera.transform.localPosition;
    }

    void Update()
    {
        // 计算水平和垂直旋转 steps
        float xExtra = Input.GetAxis("Mouse X") * XSensitivity;
        float yExtra = Input.GetAxis("Mouse Y") * YSensitivity;
        
        if (camera.transform.parent != transform)
            InitializeForcedLook();

        // 按住右键？然后旋转
        if (Input.GetMouseButton(mouseRotateButton) &&
            !Utils.IsCursorOverUserInterface())
        {
            // 水平旋转角色，垂直旋转摄像机
            transform.Rotate(new Vector3(0, xExtra, 0));
            camera.transform.Rotate(new Vector3(-yExtra, 0, 0));
        }
    }
    void LateUpdate(){
        camera.transform.localRotation = Utils.ClampRotationAroundXAxis(camera.transform.localRotation, MinimumX, MaximumX);

        // camera zoom
        float step = Utils.GetZoomUniversal() * zoomSpeed;
        distance = Mathf.Clamp(distance - step, minDistance, maxDistance);


        // 计算目标和缩放位置
        Vector3 origin = originalCameraPosition;
        Vector3 offsetBase = thirdPersonOffset;
        Vector3 offsetMult = thirdPersonOffsetMultiplier;

        Vector3 target = transform.TransformPoint(origin + offsetBase + offsetMult * distance);
        Vector3 newPosition = target - (camera.transform.rotation * Vector3.forward * distance);

        //避免视图阻塞（仅限第三人称，第一人称无意义）
        float finalDistance = distance;
        Debug.DrawLine(target, camera.transform.position, Color.white);
        if (Physics.Linecast(target, newPosition, out RaycastHit hit, viewBlockingLayers))
        {
            // 计算一个更好的距离（中间留一些空间）
            finalDistance = Vector3.Distance(target, hit.point) - 0.1f;
            Debug.DrawLine(target, hit.point, Color.red);
        }
        else Debug.DrawLine(target, newPosition, Color.green);

        // 设置最终位置
        camera.transform.position = target - (camera.transform.rotation * Vector3.forward * finalDistance);
    }

    public void InitializeForcedLook()
    {
        camera.transform.SetParent(transform, false);
    }
}
