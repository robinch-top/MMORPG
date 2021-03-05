using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.MMORPG.Scripts.RPGGame.Player
{

    [RequireComponent(typeof(Entity.Player))]
    [RequireComponent(typeof(PlayerIndicator))]
    [DisallowMultipleComponent]
    public class PlayerTargeting : Base.NetBehaviourNonAlloc
    {
        [Header("Components")]
        public Entity.Player player;
        public PlayerIndicator indicator;

        [Header("Targeting")]
        public KeyCode key = KeyCode.Tab;

        public Transform po;
        public List<GameObject> detectObjs = new List<GameObject>();

        int tabIndex = 0;
        bool doTab = true;
        public float radius = 18f;

        void Update()
        {
            if (player !=Entity.Player.localPlayer) return;

            // 有些状态是不可以被选中的，比如说潜行消失
            if (player.state == "IDLE" ||
                player.state == "MOVING" ||
                player.state == "CASTING" ||
                player.state == "STUNNED")
            {
                // 按下tab键
                if (Input.GetKeyDown(key) && doTab)
                {
                    doTab = false;
                    TargetNearest();
                }
            }
        }
        //tab选中和切换最近的目标
        public void TargetNearest(int skillID = 0)
        {
            doTab = true;
            //检测半径
            if (skillID > 0)
                radius = 5f;

            Collider[] outputCols = Physics.OverlapSphere(po.position, radius, LayerMask.GetMask("Target"));
            detectObjs.Clear();

            //检测到敌人在主角attackRange半径的圆里面
            if (outputCols != null && outputCols.Length > 0)
            {
                foreach (var target in outputCols)
                {
                    if (target.transform.parent.gameObject.CompareTag("Monster") ||
                        target.transform.parent.gameObject.CompareTag("Npc"))
                    {
                        detectObjs.Add(target.transform.parent.gameObject);
                    }
                }
            }
            if (detectObjs.Count <= 0)
                return;

            //未死亡目标按距离排序
            List<Base.Entity> targets = detectObjs.Select(go => go.GetComponent<Base.Entity>()).Where(m => m.health.current > 0).ToList();
            List<Base.Entity> sorted = targets.OrderBy(m => Vector3.Distance(transform.position, m.transform.position)).ToList();

            //如果是技能调用（非tab，有些技能释放时会尝试寻找范围内的目标）
            if (skillID > 0 && sorted.Count > 0)
            {
                //如果当前已有目标
                // return;

                //如果当前无目标，自动选择最近目标
                indicator.SetViaParent(sorted[0].transform);
                player.CmdSetTarget(sorted[0].netIdentity);

                //如果是远程技能，与目标距离大于远程技能需要距离提示距离太远了
                //...


                //如果是近战技能，与目标距离大于近战技能需要距离提示距离太远了
                //...
                return;
            }

            // 设置一个目标或者切换目标
            if (sorted.Count > 0 && sorted.Count < 2)
            {
                indicator.SetViaParent(sorted[0].transform);
                player.CmdSetTarget(sorted[0].netIdentity);
                // 如果有多个目标可以tab切换
            }
            else if (sorted.Count > 1)
            {
                if (tabIndex >= sorted.Count)
                    tabIndex = 0;

                indicator.SetViaParent(sorted[tabIndex].transform);
                player.CmdSetTarget(sorted[tabIndex].netIdentity);
                tabIndex++;
            }
        }


        // 选中目标附近2-10个目标，作为范围攻击目标，不作为当前目标显示在目标界面
        // 参数1 选中几个，参数2 距离最近or随机，参数2 目标附近的有效范围半径


        // mouse over目标，成为技能目标，但并不作为当前目标显示在目标界面


        // shift tab选中附近2-10个目标，提前选中并不攻击，不作为当前目标显示在目标界面
        // 参数1 选中几个，参数2 距离攻击者有效范围半径
        public void OnCancelAction()
        {
            // 重置tab循环序号
            tabIndex = 0;
            indicator.Clear();
        }

    }
}
