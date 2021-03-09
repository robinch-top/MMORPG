using ETModel;
namespace MMOGame
{
    [ObjectSystem]
    public class UserAwakeSystem : AwakeSystem<User, long>
    {
        public override void Awake(User self, long id)
        {
            self.Awake(id);
        }
    }

    /// <summary>
    /// 玩家对象
    /// </summary>
    public sealed class User : ETModel.Entity
    {
        //用户ID（唯一）
        public long UserId { get; private set; }

        public void Awake(long id)
        {
            this.UserId = id;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.UserId = 0;
        }
    }
}