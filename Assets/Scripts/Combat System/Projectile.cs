using System;
using UnityEngine;

namespace CombatSystem
{
    public class Projectile : MonoBehaviour
    {
        public DamageContainer damage;
        public float speed;
        Rigidbody2D rb;
        private LayerMask targetLayer, groundLayer;

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            transform.right = rb.velocity.normalized;
        }

        public void ShootProjectile(Vector2 dir, float speed)
        {
            rb.velocity = dir * speed;
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
        protected virtual void OnHitTerrain(Collider2D collider) => Destroy(gameObject);
        protected virtual void OnHitTarget(Entity entity)
        {
            entity.Damage(damage);
            Destroy(gameObject);
        }

        //https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
        public static bool LayerIsInMask(int layer, LayerMask mask)
        {
            return mask == (mask | (1 << layer));
        }

        public void SetDir(Vector2 dir)
        {
            rb.velocity = dir * speed;
        }
    }
}