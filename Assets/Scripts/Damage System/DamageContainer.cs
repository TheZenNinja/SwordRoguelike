using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageContainer 
{
    public int damage;
    public ElementalType element;
    public bool inflictStatus;
    public float impact;

    public DamageContainer(int damage) : this(damage, ElementalType.Physical, false, 0) { }
    public DamageContainer(int damage, ElementalType element) : this(damage, element, false, 0) { }
    public DamageContainer(int damage, ElementalType element, bool inflictStatus) : this(damage, element, inflictStatus, 0) { }
    public DamageContainer(int damage, ElementalType element, float impact) : this(damage, element, false, impact) { }
    public DamageContainer(int damage, ElementalType element, bool inflictStatus, float impact)
    {
        this.damage = damage;
        this.element = element;
        this.inflictStatus = inflictStatus;
        this.impact = impact;
    }
}
