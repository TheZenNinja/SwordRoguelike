using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Vector2 position => transform.position;

    [field: SerializeField]
    public int Health { get; protected set; }
    public float HealthPercent => (float)Health / maxHealth;
    [SerializeField] protected int maxHealth;
    public int MaxHealth => maxHealth;

    public System.Action onDie;
    public System.Action<Entity> onDieE;

    [Header("Debug")]
    [SerializeField]
    protected DebugFlags debugFlags;
    [System.Serializable]
    protected class DebugFlags
    {
        //public bool showLightningRange = false;
        public bool logOnHit = false;
    }

    void Start()
    {
        Health = maxHealth;
    }
    public void SetMaxHP(int maxHP)
    {
        this.maxHealth = maxHP;
        this.Health = maxHP;
    }
    public bool Damage(DamageContainer dmgCont)
    {
        if (debugFlags.logOnHit)
            Debug.Log($"{gameObject.name} took damage:" +
                $"\n\tElement: {dmgCont.element}" +
                $"\n\tInflicted Status: {dmgCont.inflictStatus}");

        float dmg = dmgCont.damage;

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
            onDieE?.Invoke(this);
            return true;
        }
        return false;
    }
}
