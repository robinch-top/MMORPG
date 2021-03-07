using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller2k;

[RequireComponent(typeof(AudioSource))]
public class CharacterMovement : Movement
{
    public enum MoveState : byte { IDLE, RUNNING, AIRBORNE, SWIMMING, MOUNTED, MOUNTED_AIRBORNE, MOUNTED_SWIMMING, DEAD }

    [Header("Components")]
    public Player player;
    public Animator animator;
    public CapsuleCollider controllerCollider;
    public CharacterController2k controller;
    
    [Header("Animation")]
    public float directionDampening = 0.05f;
    public float turnDampening = 0.1f;
    Vector3 lastForward;

    [Header("State")]
    public MoveState state = MoveState.IDLE;
    MoveState lastState = MoveState.IDLE;
    [HideInInspector] public Vector3 moveDir;

    // 支持movement (WASD) and rotations (QE)

    [Header("Running")]
    float runSpeed = 8; // 不公开，通过 Entity's speed 设置速度
    [Range(0f, 1f)] public float runStepLength = 0.7f;
    public float runStepInterval = 3;
    public float runCycleLegOffset = 0.2f; 
    float stepCycle;
    float nextStep;
    public bool runWhileBothMouseButtonsPressed = true;

    [Header("Rotation")]
    public float rotationSpeed = 190;

    [Header("Mounted")]
    public float mountedRotationSpeed = 100;

    [Header("Physics")]
    public float gravityMultiplier = 2;

    [Header("Swimming")]
    public float swimSpeed = 4;
    public float swimSurfaceOffset = 0.25f;
    Collider waterCollider;
    bool inWater => waterCollider != null; 
    bool underWater; 
    public LayerMask canStandInWaterCheckLayers = Physics.DefaultRaycastLayers; 
    [Header("Swimming [CAREFUL]")]
    [Range(0, 1)] public float underwaterThreshold = 0.7f; 

    [Header("Jumping")]
    public float jumpSpeed = 7;
    [HideInInspector] public float jumpLeg;
    bool jumpKeyPressed;

    [Header("Airborne")]
    public bool airborneSteering = true;
    public float fallMinimumMagnitude = 6; 
    public float fallDamageMinimumMagnitude = 13;
    public float fallDamageMultiplier = 2;
    [HideInInspector] public Vector3 lastFall;

    public bool isGroundedWithinTolerance =>
        controller.isGrounded || controller.velocity.y > -fallMinimumMagnitude;
    public Vector3 velocity { get; private set; }
    float lastClientSendTime;

    void Start()
    {
        
    }

    void Update(){
        // 当输入与交易等状态时，不允许移动
        if (player.IsMovementAllowed())
        {
            if (!jumpKeyPressed)
                jumpKeyPressed = Input.GetButtonDown("Jump");
        }

        // 所有player角色动画更新
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        // 只控制本地角色的Movement
        if (true)
        {
            // 根据摄像机和地面获取输入和所需方向
            Vector2 inputDir = player.IsMovementAllowed() ? GetInputDirection() : Vector2.zero;
            Vector3 desiredDir = GetDesiredDirection(inputDir);
            Debug.DrawLine(transform.position, transform.position + desiredDir, Color.cyan);

            // 更新状态机
            if      (state == MoveState.IDLE)             state = UpdateIDLE(inputDir, desiredDir);
            else if (state == MoveState.RUNNING)          state = UpdateRUNNING(inputDir, desiredDir);
            else if (state == MoveState.AIRBORNE)         state = UpdateAIRBORNE(inputDir, desiredDir);
            else if (state == MoveState.SWIMMING)         state = UpdateSWIMMING(inputDir, desiredDir);
            else if (state == MoveState.MOUNTED)          state = UpdateMOUNTED(inputDir, desiredDir);
            else if (state == MoveState.MOUNTED_AIRBORNE) state = UpdateMOUNTED_AIRBORNE(inputDir, desiredDir);
            else if (state == MoveState.MOUNTED_SWIMMING) state = UpdateMOUNTED_SWIMMING(inputDir, desiredDir);
            //else if (state == MoveState.DEAD)             state = UpdateDEAD(inputDir, desiredDir);
            else Debug.LogError("Unhandled Movement State: " + state);

            if(state == MoveState.SWIMMING){
                print(state);
            }

            // 缓存此移动的状态以检测下次着陆等
            if (!controller.isGrounded) lastFall = controller.velocity;

            // 根据最新的moveDir更改移动
            controller.Move(moveDir * Time.fixedDeltaTime); 
            velocity = controller.velocity; // for animations and fall damage

            // 广播到服务器
            CmdFixedMove(new Move(route, state, transform.position, transform.rotation.eulerAngles.y));

            
            float runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
            jumpLeg = (runCycle < 0.5f ? 1 : -1);// * move.z;

            // reset keys no matter what
            jumpKeyPressed = false;
        }

        // 在所有其他操作完成后设置最后一个状态
        lastState = state;
    }

    struct Move
    {
        public byte route;
        public MoveState state;
        public Vector3 position; 
        public float yRotation;
        public Move(byte route, MoveState state, Vector3 position, float yRotation)
        {
            this.route = route;
            this.state = state;
            this.position = position;
            this.yRotation = yRotation;
        }
    }
    Queue<Move> pendingMoves = new Queue<Move>();
    byte route = 0;
    public int minMoveBuffer = 2;
    public int combineMovesAfter = 5;
    public int maxMoveBuffer = 10; // 20ms fixedDelta * 10 = 200 ms. no need to buffer more than that.
    void CmdFixedMove(Move move)
    {
        // check route 检查路由以确保接收到的移动是在最后一次强制重置后发送的。
        // 我们不想应用在上一次强制重置期间正在传输的移动（我们不希望应用旧的移动）。
        if (move.route != route)
            return;

        if (!isLocalPlayer && pendingMoves.Count < maxMoveBuffer)
            pendingMoves.Enqueue(move);

        // 向附近所有玩家广播
        RpcFixedMove(move);
    }

    void RpcFixedMove(Move move)
    {
        // 如果是localplayer，则不执行任何操作。它自己移动。
        if (isLocalPlayer) return;

        // 添加到挂起的移动，直到发生FixedUpdate
        if (pendingMoves.Count < maxMoveBuffer)
            pendingMoves.Enqueue(move);
    }

    // 之后放到前端的Handler中，reset postion与route
    void RpcWarp(Vector3 position, byte newRoute)
    {
        // force reset position
        transform.position = position;

        // set new route
        route = newRoute;

        pendingMoves.Clear();
    }

    // rotate with QE keys
    void RotateWithKeys()
    {
        float horizontal2 = Input.GetAxis("Horizontal2");
        transform.Rotate(Vector3.up * horizontal2 * rotationSpeed * Time.fixedDeltaTime);
    }

    bool EventDied()
    {
        //return health.current == 0;
        return false;
    }

    bool EventJumpRequested()
    {
        return isGroundedWithinTolerance &&
               controller.slidingState == SlidingState.NONE &&
               jumpKeyPressed;
    }

    bool EventFalling()
    {
        return !isGroundedWithinTolerance;
    }

    bool EventLanded()
    {
        return controller.isGrounded;
    }

    void OnTriggerEnter(Collider co)
    {
        //print(co.name);
        // touching water? then set water collider
        if (co.tag == "Water")
            waterCollider = co;
    }

    void OnTriggerExit(Collider co)
    {
        if (co.tag == "Water")
            waterCollider = null;
    }
    
    // animations //////////////////////////////////////////////////////////////
    float GetJumpLeg()
    {
        return jumpLeg;
    }

    static float AnimationDeltaUnclamped(Vector3 lastForward, Vector3 currentForward)
    {
        Quaternion rotationDelta = Quaternion.FromToRotation(lastForward, currentForward);
        float turnAngle = rotationDelta.eulerAngles.y;
        return turnAngle >= 180 ? turnAngle - 360 : turnAngle;
    }

    void UpdateAnimations()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        float turnAngle = AnimationDeltaUnclamped(lastForward, transform.forward);
        lastForward = transform.forward;

        // 将动画参数应用于所有动画，角色使用了多个蒙皮网格的装备道具。
        foreach (Animator animator in GetComponentsInChildren<Animator>())
        {
            animator.SetFloat("DirX", localVelocity.x, directionDampening, Time.deltaTime); // smooth idle<->run transitions
            animator.SetFloat("DirY", localVelocity.y, directionDampening, Time.deltaTime); // smooth idle<->run transitions
            animator.SetFloat("DirZ", localVelocity.z, directionDampening, Time.deltaTime); // smooth idle<->run transitions
            animator.SetFloat("LastFallY", lastFall.y);
            animator.SetFloat("Turn", turnAngle, turnDampening, Time.deltaTime); // smooth turn
            animator.SetBool("SWIMMING", state == MoveState.SWIMMING);

            animator.SetBool("OnGround", state != MoveState.AIRBORNE);
            if (controller.isGrounded) animator.SetFloat("JumpLeg", GetJumpLeg());
        }
    }

    bool EventUnderWater()
    {
        if (inWater) 
        {
            // 从水底到玩家位置的光线投射
            Vector3 origin = new Vector3(transform.position.x,
                                         waterCollider.bounds.max.y,
                                         transform.position.z);
            float distance = controllerCollider.height * underwaterThreshold;
            Debug.DrawLine(origin, origin + Vector3.down * distance, Color.cyan);

            // 如果光线投射没有击中任何东西就在水下
            return !Utils.RaycastWithout(origin, Vector3.down, out RaycastHit hit, distance, gameObject, canStandInWaterCheckLayers);
        }
        return false;
    }

    float ApplyGravity(float moveDirY)
    {
        // 坠落时施加全重力
        if (!controller.isGrounded)
            return moveDirY + Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        return 0;
    }


    MoveState UpdateIDLE(Vector2 inputDir, Vector3 desiredDir)
    {
        // QE key rotation
        if (player.IsMovementAllowed())
            RotateWithKeys();

        // move
        moveDir.x = desiredDir.x * runSpeed;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = desiredDir.z * runSpeed;

        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        else if (EventFalling())
        {
            return MoveState.AIRBORNE;
        }
        else if (EventJumpRequested())
        {
            moveDir.y = jumpSpeed;
            PlayJumpSound();
            return MoveState.AIRBORNE;
        }
        else if (EventUnderWater())
        {
            // rescale capsule
            if (controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false))
            {
                return MoveState.SWIMMING;
            }
        }
        else if (inputDir != Vector2.zero)
        {
            return MoveState.RUNNING;
        }

        return MoveState.IDLE;
    }

    MoveState UpdateRUNNING(Vector2 inputDir, Vector3 desiredDir)
    {
        // QE key rotation
        if (player.IsMovementAllowed())
            RotateWithKeys();

        // move
        moveDir.x = desiredDir.x * runSpeed;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = desiredDir.z * runSpeed;

        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        else if (EventFalling())
        {
            return MoveState.AIRBORNE;
        }
        else if (EventJumpRequested())
        {
            moveDir.y = jumpSpeed;
            PlayJumpSound();
            return MoveState.AIRBORNE;
        }
        else if (EventUnderWater())
        {
            if (controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false))
            {
                return MoveState.SWIMMING;
            }
        }
        // 完全减速后进入 idle (y无所谓)
        else if (moveDir.x == 0 && moveDir.z == 0)
        {
            return MoveState.IDLE;
        }

        ProgressStepCycle(inputDir, runSpeed);
        return MoveState.RUNNING;
    }

    MoveState UpdateAIRBORNE(Vector2 inputDir, Vector3 desiredDir)
    {
        if (airborneSteering)
        {
            // QE key rotation
            if (player.IsMovementAllowed())
                RotateWithKeys();

            // move
            moveDir.x = desiredDir.x * runSpeed;
            moveDir.y = ApplyGravity(moveDir.y);
            moveDir.z = desiredDir.z * runSpeed;
        }
        // 否则，继续向同一方向移动，仅施加重力
        else
        {
            moveDir.y = ApplyGravity(moveDir.y);
        }

        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        else if (EventLanded())
        {
            PlayLandingSound();
            return MoveState.IDLE;
        }
        else if (EventUnderWater())
        {
            if (controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false))
            {
                return MoveState.SWIMMING;
            }
        }

        return MoveState.AIRBORNE;
    }

    MoveState UpdateSWIMMING(Vector2 inputDir, Vector3 desiredDir)
    {
        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        // not under water anymore?
        else if (!EventUnderWater())
        {
            if (controller.TrySetHeight(controller.defaultHeight * 1f, true, true, false))
            {
                return MoveState.IDLE;
            }
        }

        // QE key rotation
        if (player.IsMovementAllowed())
            RotateWithKeys();

        // move
        moveDir.x = desiredDir.x * swimSpeed;
        moveDir.z = desiredDir.z * swimSpeed;

        // gravitate toward surface
        if (waterCollider != null)
        {
            float surface = waterCollider.bounds.max.y;
            float surfaceDirection = surface - controller.bounds.min.y - swimSurfaceOffset;
            moveDir.y = surfaceDirection * swimSpeed;
        }
        else moveDir.y = 0;

        return MoveState.SWIMMING;
    }

    MoveState UpdateMOUNTED(Vector2 inputDir, Vector3 desiredDir)
    {
        // 忽略inputDir水平部分时重新计算所需方向
        desiredDir = GetDesiredDirection(new Vector2(0, inputDir.y));

        // 水平输入轴旋转角色而不是平移
        if (player.IsMovementAllowed())
            transform.Rotate(Vector3.up * inputDir.x * mountedRotationSpeed * Time.fixedDeltaTime);

        // 还没有加入坐骑判断，如果有坐骑速度要切换
        float speed = runSpeed;

        // move
        moveDir.x = desiredDir.x * speed;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = desiredDir.z * speed;

        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        else if (EventFalling())
        {
            return MoveState.MOUNTED_AIRBORNE;
        }
        else if (EventJumpRequested())
        {
            moveDir.y = jumpSpeed;
            PlayJumpSound();
            return MoveState.MOUNTED_AIRBORNE;
        }
        else if (EventUnderWater())
        {
            return MoveState.MOUNTED_SWIMMING;
        }

        return MoveState.MOUNTED;
    }

    MoveState UpdateMOUNTED_AIRBORNE(Vector2 inputDir, Vector3 desiredDir)
    {
        // 忽略inputDir水平部分时重新计算所需方向
        desiredDir = GetDesiredDirection(new Vector2(0, inputDir.y));

        // 水平输入轴旋转角色而不是平移
        if (player.IsMovementAllowed())
            transform.Rotate(Vector3.up * inputDir.x * mountedRotationSpeed * Time.fixedDeltaTime);

        // 还没有加入坐骑判断，如果有坐骑速度要切换
        float speed = runSpeed;

        // move
        moveDir.x = desiredDir.x * speed;
        moveDir.y = ApplyGravity(moveDir.y);
        moveDir.z = desiredDir.z * speed;

        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        else if (EventLanded())
        {
            PlayLandingSound();
            return MoveState.MOUNTED;
        }
        else if (EventUnderWater())
        {
            return MoveState.MOUNTED_SWIMMING;
        }

        return MoveState.MOUNTED_AIRBORNE;
    }

    MoveState UpdateMOUNTED_SWIMMING(Vector2 inputDir, Vector3 desiredDir)
    {
        if (EventDied())
        {
            controller.TrySetHeight(controller.defaultHeight * 0.25f, true, true, false);
            return MoveState.DEAD;
        }
        // not under water anymore?
        else if (!EventUnderWater())
        {
            return MoveState.MOUNTED;
        }
        
        // 忽略inputDir水平部分时重新计算所需方向
        desiredDir = GetDesiredDirection(new Vector2(0, inputDir.y));

        // 水平输入轴旋转角色而不是平移
        if (player.IsMovementAllowed())
            transform.Rotate(Vector3.up * inputDir.x * mountedRotationSpeed * Time.fixedDeltaTime);

        // move with acceleration (feels better)
        moveDir.x = desiredDir.x * swimSpeed;
        moveDir.z = desiredDir.z * swimSpeed;

        // gravitate toward surface
        if (waterCollider != null)
        {
            float surface = waterCollider.bounds.max.y;
            float surfaceDirection = surface - controller.bounds.min.y - swimSurfaceOffset;
            moveDir.y = surfaceDirection * swimSpeed;
        }
        else moveDir.y = 0;

        return MoveState.MOUNTED_SWIMMING;
    }

    float GetMaximumSpeedForState(MoveState moveState)
    {
        switch (moveState)
        {
            // idle, running, mounted use runSpeed which is set by Entity
            case MoveState.IDLE:
            case MoveState.RUNNING:
            case MoveState.MOUNTED:
                return runSpeed;
            // swimming uses swimSpeed
            case MoveState.SWIMMING:
            case MoveState.MOUNTED_SWIMMING:
                return swimSpeed;
            // airborne accelerates with gravity.
            // maybe check xz and y speed separately.
            case MoveState.AIRBORNE:
            case MoveState.MOUNTED_AIRBORNE:
                return float.MaxValue;
            case MoveState.DEAD:
                return 0;
            default:
                Debug.LogWarning("Don't know how to calculate max speed for state: " + moveState);
                return 0;
        }
    }


    void PlayLandingSound()
    {
        // feetAudio.clip = landSound;
        // feetAudio.Play();
        // nextStep = stepCycle + .5f;
    }

    void PlayJumpSound()
    {
        // feetAudio.clip = jumpSound;
        // feetAudio.Play();
    }

    void ProgressStepCycle(Vector3 inputDir, float speed)
    {
        if (controller.velocity.sqrMagnitude > 0 && (inputDir.x != 0 || inputDir.y != 0))
        {
            stepCycle += (controller.velocity.magnitude + (speed * runStepLength)) *  Time.fixedDeltaTime;
        }

        if (stepCycle > nextStep)
        {
            nextStep = stepCycle + runStepInterval;
            PlayFootStepAudio();
        }
    }

    void PlayFootStepAudio()
    {
        if (!controller.isGrounded) return;

        // do we have any footstep sounds?
        //...
    }


    Vector3 GetDesiredDirection(Vector2 inputDir)
    {
        // 始终沿着相机向前移动
        return transform.forward * inputDir.y + transform.right * inputDir.x;
    }

    Vector2 GetInputDirection()
    {
        float horizontal = 0;
        float vertical = 0;

        // keyboard input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(horizontal, vertical);
        if (input.magnitude > 1)
        {
            input = input.normalized;
        }
        return input;
    }

    public override Vector3 GetVelocity()
    {
        return velocity;
    }

    public override bool IsMoving()
    {
        return velocity != Vector3.zero;
    }

    public override void SetSpeed(float speed)
    {
        runSpeed = speed;
    }

    public override void Reset()
    {
        // we have no navigation, so we don't need to reset any paths
    }

    public override void LookAtY(Vector3 position)
    {
        transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
    }

    public override bool CanNavigate()
    {
        return false;
    }

    public override void Navigate(Vector3 destination, float stoppingDistance)
    {
        // character controller movement doesn't allow navigation (yet)
    }

    public override bool IsValidSpawnPoint(Vector3 position)
    {
        return true;
    }

    public override Vector3 NearestValidDestination(Vector3 destination)
    {
        // character controller movement doesn't allow navigation (yet)
        return destination;
    }

    public override bool DoCombatLookAt()
    {
        // player should use keys/mouse to look at. don't overwrite it.
        return false;
    }

    public override void Warp(Vector3 destination)
    {
        // set new position
        transform.position = destination;
    }
}
