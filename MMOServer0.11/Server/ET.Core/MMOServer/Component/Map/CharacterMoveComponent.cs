using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public enum MoveType
    {
        Move,
        PushedBack, //被击退
        Launched, // 被击飞
        Floated, //漂浮
        Sunk, //沉没
        Attract, // 吸引
    }
    [ObjectSystem]
    public class CharacterMoveComponentAwakeSystem : AwakeSystem<CharacterMoveComponent>
    {
        public override void Awake(CharacterMoveComponent self)
        {
            self.Awake();
        }
    }
    public class CharacterMoveComponent : Component
    {
        

        public MoveType moveType;
        public Unit unit;

        public Vector3 targetPosition;
        public Vector3 startPosition;
        public float moveSpeed;
        public Vector3 moveDir;
        public Quaternion yEuler;
        public float yRotation;
        public long startTime;
        public long needTime;
        public long endTime;

        public Move targetMove;

        public byte route = 0; 
        public float rubberDistance = 1;
        public float epsilon = 0.1f;
        public float validSpeedTolerance = 0.2f;
        public float runSpeed = 8;
        public float swimSpeed = 4;

        public int rubbered = 0;

        public float baseMoveSpeed = 5f;// 这个应该从配置表里读

        public void Awake()
        {
            unit = GetParent<Unit>();

        }

      
    }
}
