using UnityEngine;
using UnityEngine.UI;

// 技能面板UI组件

public partial class UISkills : MonoBehaviour
{
    public KeyCode hotKey = KeyCode.R;
    public GameObject panel;
    public UISkillSlot slotPrefab;
    public Transform content;
    public Text skillExperienceText;
    public int itmeHeight = 36;

    void SwitchDetails(int index){
        UISkillSlot slot = content.GetChild(index).GetComponent<UISkillSlot>();
        bool isDetail = slot.descripButton.gameObject.GetComponent<VerticalLayoutGroup>().childControlHeight;
        if(!isDetail)
            slot.descripButton.gameObject.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
        else
            slot.descripButton.gameObject.GetComponent<VerticalLayoutGroup>().childControlHeight = false;
    }

    void Update()
    {
        Player player = Player.localPlayer;
        if (!(player is null))
        {
            // 热键打开技能面板
            if (Input.GetKeyDown(hotKey) && !UIUtils.AnyInputActive())
                panel.SetActive(!panel.activeSelf);

            // 面板打开时刷新面板中的技能
            if (panel.activeSelf)
            {
                // 删除和创建技能面板技能条目，维持的条目实例
                UIUtils.BalancePrefabs(slotPrefab.gameObject, player.skills.skills.Count, content);

                // refresh all
                for (int i = 0; i < player.skills.skills.Count; i++)
                {
                    UISkillSlot slot = content.GetChild(i).GetComponent<UISkillSlot>();
                    var rt = slot.descriptionText.gameObject.GetComponent<RectTransform>();
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itmeHeight);

                    Skill skill = player.skills.skills[i];
                    // bool isPassive = skill.data is PassiveSkill;

                    // 已学会技能可拖放状态
                    slot.dragAndDropable.name = i.ToString();
                    slot.dragAndDropable.dragable = skill.level > 0;

                    // 可否施放的技能
                    bool canCast = player.skills.CastCheckSelf(skill);
                    int icopy = i;

                    // 点击技能描述区，展开切换技能详情
                    slot.descripButton.onClick.SetListener(() => {
                        SwitchDetails(icopy);
                    });

                    // 点击技能图标释放技能
                    slot.button.interactable = skill.level > 0 &&
                                               canCast;
                    slot.button.onClick.SetListener(() => {
                        ((PlayerSkills)player.skills).TryUse(icopy);
                    });

                    // 技能图片
                    if (skill.level > 0)
                    {
                        slot.image.color = Color.white;
                        slot.image.sprite = skill.image;
                    }

                    // 技能描述
                    slot.descriptionText.text = skill.ToolTip(showRequirements: skill.level == 0);

                    // 学习或升级技能
                    if (skill.level < skill.maxLevel && ((PlayerSkills)player.skills).CanUpgrade(skill))
                    {
                        slot.upgradeButton.gameObject.SetActive(true);
                        slot.upgradeButton.GetComponentInChildren<Text>().text = skill.level == 0 ? "Learn" : "Upgrade";
                        slot.upgradeButton.onClick.SetListener(() => { ((PlayerSkills)player.skills).CmdUpgrade(icopy); });
                    }
                    else slot.upgradeButton.gameObject.SetActive(false);

                    // 技能释放冷却
                    float cooldown = skill.CooldownRemaining();
                    slot.cooldownOverlay.SetActive(skill.level > 0 && cooldown > 0);
                    slot.cooldownText.text = cooldown.ToString("F0");
                    slot.cooldownCircle.fillAmount = skill.cooldown > 0 ? cooldown / skill.cooldown : 0;
                }

                // 技能学习经验点
                skillExperienceText.text = ((PlayerSkills)player.skills).skillExperience.ToString();
            }
        }
        else panel.SetActive(false);
    }
}