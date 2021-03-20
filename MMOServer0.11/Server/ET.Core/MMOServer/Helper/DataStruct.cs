using System;
using UnityEngine;
namespace ETModel
{
    public struct Move
    {
        public byte route;
        public MoveState state;
        public int frame;
        public Vector3 position; 
        public float yRotation;
        public Move(byte route, MoveState state, int frame ,Vector3 position, float yRotation)
        {
            this.route = route;
            this.state = state;
            this.frame = frame;
            this.position = position;
            this.yRotation = yRotation;
        }
    }

    public enum MoveState : byte { IDLE, RUNNING, AIRBORNE, SWIMMING, MOUNTED, MOUNTED_AIRBORNE, MOUNTED_SWIMMING, DEAD }
}