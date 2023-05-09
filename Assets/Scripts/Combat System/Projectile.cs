using System;
using UnityEngine;

namespace CombatSystem
{
    public class Projectile : MonoBehaviour
    {
        public float speed;
        Rigidbody2D rb;
        public LayerMask targetLayer, groundLayer;

        void Awake()
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
                    OnHitTarget(entity);
                }
            }
            else if (LayerIsInMask(colLayer, groundLayer))
                OnHitTerrain(collider);
        }
        protected virtual void OnHitTerrain(Collider2D collider) => Destroy(gameObject);
        protected virtual void OnHitTarget(Entity entity)
        {
            Debug.Log($"Hit {entity.gameObject.name}");
            entity.Damage(1);
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
            Destroy(gameObject, 5);
        }
    }
}