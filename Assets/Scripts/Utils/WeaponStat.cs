[System.Serializable]
public struct WeaponStat : System.IComparable<WeaponStat>
{
    public enum StatType
    {
        damage,
        statusChance, 
        attackSpeed, 
        attackRange, 
        peircing
    }
    public StatType statType;
    public bool isFlatValue;
    public float value;

    public WeaponStat(StatType statType, bool isFlatValue, float value)
    {
        this.statType = statType;
        this.isFlatValue = isFlatValue;
        this.value = value;
    }

    public int CompareTo(WeaponStat other)
    {
        var typeComp = statType.CompareTo(other.statType);
        if (typeComp != 0)
            return typeComp;

        return isFlatValue.CompareTo(other.isFlatValue);
    }
    /// <summary>
    /// used to simplify stat calculations
    /// </summary>
    /// <param name="w"></param>
    public static implicit operator UnityEngine.Vector2(WeaponStat w)
    {
        var v = new UnityEngine.Vector2();
        if (w.isFlatValue)
            v.x = w.value;
        else
            v.y = w.value;
        return v;
    }
}
