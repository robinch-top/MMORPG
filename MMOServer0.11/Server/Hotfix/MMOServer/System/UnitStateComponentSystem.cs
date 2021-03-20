using System;
using System.Collections.Generic;
using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
    public static class UnitStateComponentSystem
    {
        
        public static void SyncStateFrame(this UnitStateComponent self, Move move)
        {
            // 获取输出数据，传入CharacterMovementComponet
            // 服务器上要有一份同样的角色移动数据，用于服务端的各种计算与判断
            CharacterMoveComponent characterMoveComponent = self.unit.GetComponent<CharacterMoveComponent>();
            characterMoveComponent.MoveAsync(move);


            // 向附近玩家广播targetMove,更新preSendMsgFrame
            if(self.preSendMsgFrame!=self.currFrame){
                MapHelper.BroadcastMove(characterMoveComponent.targetMove,self.unit);
                self.preSendMsgFrame = self.currFrame;
            }
                 
            
        }

    
    }
}