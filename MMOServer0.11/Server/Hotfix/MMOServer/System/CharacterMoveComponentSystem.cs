using ETModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ETHotfix
{

    [ObjectSystem]
    public class CharacterMoveComponentUpdateSystem : FixedUpdateSystem<CharacterMoveComponent>
    {
        public override void FixedUpdate(CharacterMoveComponent self)
        {
            self.FixedUpdate();
        }
    }

    public static class CharacterMoveComponentSystem
    {

        public static void MoveAsync(this CharacterMoveComponent self, Move target)
        {
            // 验证移动是否有效,延迟补偿等
            // ...

            // 验证通过或不需补偿，更新targetMove，获得服务器currFrame
            self.targetMove = target;
            int key = Game.Scene.GetComponent<UnitStateMgrComponent>().currFrame;
            self.targetMove.frame = key;
            self.unit.GetComponent<UnitStateComponent>().currFrame = key;
            
            // 获得新的移动速度
            // ...

            // 暂时使用baseMoveSpeed，实际上要加上buff,技能,装备,坐骑能产生的速度，这是一个变化的数值
            self.MoveTo(self.baseMoveSpeed);
        }

        public static void MoveTo(this CharacterMoveComponent self,float speed)
        {
            self.yRotation = self.targetMove.yRotation;
            self.yEuler = Quaternion.Euler(new Vector3(0,self.targetMove.yRotation,0));

            // 移动距离过大或过小都不更新移动位置
            float distance = Vector3.Distance(self.unit.Position, self.targetMove.position);
            if (distance < 0.02f || distance >15) return;

            self.startPosition = self.unit.Position;
            self.targetPosition = self.targetMove.position;
            self.moveSpeed = speed;
            
            // 计算移动到新位置需要的与结束的时间点
            self.startTime = TimeHelper.Now();
            float time = distance / speed;
            self.needTime = (long)(time * 1000);
            self.endTime = self.startTime + self.needTime;
        }


        public static void FixedUpdate(this CharacterMoveComponent self)
        {
            long timeNow = TimeHelper.Now();

            // 移动距离过大不进行插值移动
            float distance = Vector3.Distance(self.unit.Position, self.targetPosition);
            if (distance >15) return;

            // 目标move的位置与角度插值
            self.unit.Rotation = Quaternion.Slerp(self.unit.Rotation, self.yEuler, EventSystem.FixedUpdateTime * 15);
            
            float amount = (timeNow - self.startTime) * 1f / self.needTime;
            self.unit.Position = Vector3.Lerp(self.startPosition, self.targetPosition, amount);
            
        }

    }
}

