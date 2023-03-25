using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public DamageContainer dmg;
    public LayerMask groundLayer;
    public LayerMask targetMask;
    //public bool ignoreFriendlyCollision = true;

    public bool destroyAfterTime;
    public float timeUntilDestroy;

    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (destroyAfterTime)
        {
            timeUntilDestroy -= Time.fixedDeltaTime;
            if (timeUntilDestroy <=0)
                Destroy(gameObject);
        }
    }
    public void SetDamage(DamageContainer dmg) => this.dmg = dmg;
    public void SetVelocity(Vector2 velocity, bool updateRotation = true)
    {
        rb.velocity = velocity;
        if (updateRotation)
            UpdateRotation();
    }
    public void UpdateRotation()
    {
        float angle = Mathf.Atan2(rb.velocity.x, rb.velocity.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0,0,angle);
    }
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        //if (ignoreFriendlyCollision)
        //{
        //    if (collider.gameObject.layer == gameObject.layer)
        //    {
        //        Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), true);
        //        return;
        //    }
        //}
        var colLayer = collider.gameObject.layer;

        if (LayerIsInMask(colLayer, targetMask))
        {
            Entity entity;
            if (collider.hasComponent(out entity))
                OnHitTarget(entity);
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
        Destroy(gameObject, Time.fixedDeltaTime);
    }

    //https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
    public static bool LayerIsInMask(int layer, LayerMask mask)
    {
        return mask == (mask | (1 << layer));
    }
}
