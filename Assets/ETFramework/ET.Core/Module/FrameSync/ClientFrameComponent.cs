#if !SERVER
using System;
using System.Collections.Generic;
using Google.Protobuf;

namespace ETModel
{
    public struct SessionFrameMessage
    {
        public Session Session;
        public FrameMessage FrameMessage;
    }
    
	[ObjectSystem]
	public class ClientFrameComponentUpdateSystem : UpdateSystem<ClientFrameComponent>
	{
		public override void Update(ClientFrameComponent self)
		{
			self.Update();
		}
	}


	public class ClientFrameComponent: Component
    {
        public int Frame;

        public Queue<SessionFrameMessage> Queue = new Queue<SessionFrameMessage>();

        public int count = 1;

	    public int waitTime = 100;

        public const int maxWaitTime = 100;

        public void Add(Session session, FrameMessage frameMessage)
        {
            this.Queue.Enqueue(new SessionFrameMessage() {Session = session, FrameMessage = frameMessage});
        }

        public void Update()
        {
			if (this.Queue.Count == 0)
            {
                return;
            }
            SessionFrameMessage sessionFrameMessage = this.Queue.Dequeue();
            this.Frame = sessionFrameMessage.FrameMessage.Frame;

            // OuterMessageDispatcher中收到服务端的帧消息就添加到SessionFrameMessage消息队列中了
            // 下面就是从消息队伍中取出消息内容进行拿到消息后的操作处理
            // 消息队列中添加消息与拿到消息内容进行后续操作，并不是同步的。这里对消息队列进行遍历操作。
            for (int i = 0; i < sessionFrameMessage.FrameMessage.Message.Count; ++i)
            {
	            OneFrameMessage oneFrameMessage = sessionFrameMessage.FrameMessage.Message[i];

				Session session = sessionFrameMessage.Session;
				OpcodeTypeComponent opcodeTypeComponent = session.Network.Entity.GetComponent<OpcodeTypeComponent>();
	            ushort opcode = (ushort) oneFrameMessage.Op;
	            object instance = opcodeTypeComponent.GetInstance(opcode);

	            byte[] bytes = ByteString.Unsafe.GetBuffer(oneFrameMessage.AMessage);
	            IMessage message = (IMessage)session.Network.MessagePacker.DeserializeFrom(instance, bytes, 0, bytes.Length);
                Game.Scene.GetComponent<MessageDispatcherComponent>().Handle(sessionFrameMessage.Session, new MessageInfo((ushort)oneFrameMessage.Op, message));
            }
        }
    }
}
#endif