public interface ICombatBonus
{
    float GetDpsBonus();
    float GetAttackRateBonus();
    int GetHurtBonus();
    int GetMagicBonus();
    int GetPhysicalBonus();
    float GetMagicHitrateBonus();
    float GetPhysicHitrateBonus();
    float GetMagicCriticalBonus();
    float GetPhysicCriticalBonus();

    int GetAgilityBonus();
    int GetStrengthBonus();
    int GetSpiritBonus();

    int GetArmorBonus();
    float GetDodgeBonus();
    float GetBlockBonus();
    int GetPhysicalDefenseBonus();
    int GetMagicDefenseBonus();
}