using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class ServerFrameComponentSystem : AwakeSystem<ServerFrameComponent>
    {
	    public override void Awake(ServerFrameComponent self)
	    {
		    self.Awake();
	    }
    }
	
    public static class ServerFrameComponentEx
    {
        public static void Awake(this ServerFrameComponent self)
        {
            self.Frame = 0;
            self.FrameMessage = new FrameMessage() {Frame = self.Frame};

            self.UpdateFrameAsync().Coroutine();
        }

        public static async ETVoid UpdateFrameAsync(this ServerFrameComponent self)
        {
            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();

            long instanceId = self.InstanceId;

            while (true)
            {
                if (self.InstanceId != instanceId)
                {
                    return;
                }

                await timerComponent.WaitAsync(100);
				
                // 不论 FrameMessage.Message 是否有消息内容，都会向客户端转发
                MessageHelper.Broadcast(self.FrameMessage);
                
                // 转发完，reset FrameMassage
                ++self.Frame;
                self.FrameMessage = new FrameMessage() { Frame = self.Frame };
            }
        }

        public static void Add(this ServerFrameComponent self, OneFrameMessage oneFrameMessage)
        {
            // 添加帧消息到 FrameMessage.Message
            self.FrameMessage.Message.Add(oneFrameMessage);
        }
    }
}