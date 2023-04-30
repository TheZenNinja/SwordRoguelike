using System.Collections;
using UnityEngine;

public class MeleeProjectile : MonoBehaviour
{
    public DamageContainer dmg;
    public LayerMask groundLayer;
    public LayerMask targetLayer;
    //public bool ignoreFriendlyCollision = true;
    public ParticleSystem onDestroyParticle;

    public float percentLifeRemaining => Mathf.Clamp01(timeUntilDestroy / maxLifetime);

    [Tooltip("Use the curve backwards")]
    public AnimationCurve velCurve;
    public float maxDist;
    public float maxLifetime;
    public float timeUntilDestroy;
    public Vector2 dir;



    public Vector2 inheritedVel;
    public float speedMulti = 1;

    Rigidbody2D rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        timeUntilDestroy = maxLifetime;
    }
    protected void FixedUpdate()
    {
        timeUntilDestroy -= Time.fixedDeltaTime;
        if (timeUntilDestroy <= 0)
            Destroy(gameObject);
        UpdateVelocity();
    }

    protected void UpdateVelocity()
    {
        float vel = maxDist / maxLifetime;
        rb.velocity = dir * vel * velCurve.Evaluate(percentLifeRemaining) * speedMulti + inheritedVel;
    }
    protected void UpdateRotation()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public void SetDamage(DamageContainer dmg) => this.dmg = dmg;

    public void SetVelocity(Vector2 dir) => SetVelocity(dir, maxDist, maxLifetime);
    public void SetVelocity(Vector2 dir, float maxDist, float maxLifetime)
    {
        this.dir = dir.normalized;
        this.maxDist = maxDist;
        this.maxLifetime = maxLifetime;
        timeUntilDestroy = maxLifetime;

        UpdateVelocity();
        UpdateRotation();
    }

    public void SetVelocity(Vector2 dir, float maxDist, float maxLifetime, float velMulti)
    {
        SetVelocity(dir, maxDist, maxLifetime);
        speedMulti = velMulti;
    }
    public void SetVelocity(Vector2 dir, float maxDist, float maxLifetime, Vector2 inheritedVel)
    {
        SetVelocity(dir, maxDist, maxLifetime);
        this.inheritedVel = inheritedVel;
    }

    public void SetLayers(LayerMask groundLayer, LayerMask targetLayer)
    {
        this.groundLayer = groundLayer;
        this.targetLayer = targetLayer;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        var colLayer = collider.gameObject.layer;

        if (LayerIsInMask(colLayer, targetLayer))
        {
            Entity entity;
            if (collider.hasComponent(out entity))
            {
                Debug.Log(entity.gameObject.name);
                OnHitTarget(entity);
            }
        }
        else if (LayerIsInMask(colLayer, groundLayer))
            OnHitTerrain(collider);
    }
    protected virtual void OnHitTerrain(Collider2D collider)
    {
        Destroy(gameObject, Time.fixedDeltaTime);
    }
    protected virtual void OnHitTarget(Entity entity)
    {
        entity.Damage(dmg);
        //Destroy(gameObject, Time.fixedDeltaTime);
    }

    //https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
    public static bool LayerIsInMask(int layer, LayerMask mask)
    {
        return mask == (mask | (1 << layer));
    }
    public void OnDestroy()
    {
        if (Application.isPlaying)
            Instantiate(onDestroyParticle, transform.position, transform.rotation).gameObject.SetActive(true);
    }
}