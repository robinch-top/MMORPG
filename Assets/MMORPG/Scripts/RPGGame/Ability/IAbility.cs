namespace Assets.MMORPG.Scripts.RPGGame.Ability
{
    // 生命能力 全系
    // 蓝量能力 法系
    // 能量能力 盗贼 Rogue energy
    // 怒气能力 战士 Warrior rage
    public interface IManaBonus
    {
        int GetManaBonus(int baseMana);
        /// <summary>
        /// 回蓝量
        /// </summary>
        /// <returns></returns>
        int GetManaRecoveryBonus();
        /// <summary>
        /// 智力总量
        /// </summary>
        /// <returns></returns>
        int GetIntellectBonus();
    }

    public interface IHealthBonus
    {
        int GetHealthBonus(int baseHealth);
        /// <summary>
        /// 回血量
        /// </summary>
        /// <returns></returns>
        int GetHealthRecoveryBonus();
        /// <summary>
        /// 耐力总量
        /// </summary>
        /// <returns></returns>
        int GetEnduranceBonus();
    }

    public interface IEnergyBonus
    {
        int GetEnergyBonus(int baseHealth);
        int GetEnergyRecoveryBonus();
    }

    public interface IRageBonus
    {
        int GetRageBonus(int baseHealth);
        int GetRageRecoveryBonus();
    }
}
