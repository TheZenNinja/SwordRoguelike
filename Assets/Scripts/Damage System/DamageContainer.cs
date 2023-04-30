using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageContainer 
{
    public int damage;
    public ElementalType element;
    public bool inflictStatus;

    public DamageContainer(int damage) : this(damage, ElementalType.Physical, false) { }
    public DamageContainer(int damage, ElementalType element) : this(damage, element, false) { }
    public DamageContainer(int damage, ElementalType element, bool inflictStatus)
    {
        this.damage = damage;
        this.element = element;
        this.inflictStatus = inflictStatus;
    }
}
