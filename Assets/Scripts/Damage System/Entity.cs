using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector2 position => transform.position;

    [field:SerializeField]
    public int Health { get; protected set; }
    public float HealthPercent => (float)Health / maxHealth;
    [SerializeField] protected int maxHealth;

    public System.Action onDie;

    [Header("Status Effects")]
    protected bool isBurning;
    protected bool isFrozen;

    [Header("Debug")]
    [SerializeField]
    protected DebugFlags debugFlags;
    [System.Serializable]
    protected class DebugFlags {
        public bool showLightningRange = false;
        public bool logOnHit = false;
    }

    void Start()
    {
        Health = maxHealth;
    }

    public bool Damage(DamageContainer dmgCont)
    {
        if (debugFlags.logOnHit)
            Debug.Log($"{gameObject.name} took damage:" +
                $"\n\tElement: {dmgCont.element}" +
                $"\n\tInflicted Status: {dmgCont.inflictStatus}" +
                $"\n\tImpact: {dmgCont.impact}");

        float dmg = dmgCont.damage;

        if (isFrozen)
        {
            float shatterDmg = Mathf.RoundToInt(ElementalFunctions.I_GetShatterDamage(dmg, dmgCont.impact));
            TakeDamage(shatterDmg, DamageSourceType.shatter);
            isFrozen = false;
        }

        if (dmgCont.inflictStatus)
            switch (dmgCont.element)
            {
                default:
                case ElementalType.Physical:
                    dmg *= ElementalFunctions.P_CritDamageMulti;
                    break;
                case ElementalType.Fire:
                    Burn(dmgCont);
                    break;
                case ElementalType.Ice:
                    Freeze(dmgCont);
                    break;
                case ElementalType.Lightning:
                    ElementalFunctions.L_ChainLightning(this, dmgCont);
                    break;
            }

        

        return TakeDamage(dmg);
    }
    protected bool TakeDamage(float damage, DamageSourceType damageSource = DamageSourceType.attack)
    {
        Health -= Mathf.RoundToInt(damage);
        if (debugFlags.logOnHit)
            Debug.Log($"{gameObject.name} took damage: {Mathf.RoundToInt(damage)}\n\tFrom: {damageSource}");

        if (Health <= 0)
        {
            Health = 0;
            onDie?.Invoke();
            return true;
        }
        return false;
    }

    public void Burn(DamageContainer dmgCont)
    {
        if (isBurning)
            StopCoroutine(nameof(BurnRoutine));
        Burn(dmgCont);
    }
    protected IEnumerator BurnRoutine(DamageContainer dmgCont)
    {
        isBurning = true;
        float dmg = ElementalFunctions.F_GetBurnDamage(dmgCont);
        for (int i = 0; i < ElementalFunctions.F_MaxBurnTicks; i++)
        {
            yield return new WaitForSeconds(ElementalFunctions.F_TimeBetweenBurnTicks);
            TakeDamage(dmg, DamageSourceType.burning);
        }
        isBurning = false;
    }
    
    public void Freeze(DamageContainer dmgCont)
    {
        //yield return 0; // wait until next frame. we want to freeze them after they take damage
        isFrozen = true;
    }

    public float SqrDistFromTarget(Vector2 goalPos) => (transform.position.ToV2() - goalPos).sqrMagnitude;

    private void OnDrawGizmosSelected()
    {
        if (debugFlags.showLightningRange)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, ElementalFunctions.L_LightningChainRange);
        }
    }
}
