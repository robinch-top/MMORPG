using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using ETModel;
namespace MMOGame
{
    [ObjectSystem]
    public class GamerSystem : AwakeSystem<Gamer, long>
    {
        public override void Awake(Gamer self, long userid)
        {
            self.Awake(userid);
        }
    }

    public sealed class Gamer : ETModel.Entity
    {
        /// <summary>
        /// 每个玩家绑定一个实体 机器人的UserID为0
        /// </summary>
        public long UserId { get; private set; }

        //public GameObject GameObject;

        public void Awake(long userid)
        {
            this.UserId = userid;
        }

        public Vector3 Position
        {
            get
            {
                return GameObject.transform.position;
            }
            set
            {
                GameObject.transform.position = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return GameObject.transform.rotation;
            }
            set
            {
                GameObject.transform.rotation = value;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }
}