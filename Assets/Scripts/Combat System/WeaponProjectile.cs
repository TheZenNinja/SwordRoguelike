using UnityEngine;

namespace CombatSystem
{
    public class WeaponProjectile : MonoBehaviour
    {
        public enum WeaponState
        {
            idle,
            shot,
            returning,
        }

        [Header("Parameters")]
        public float returnAccel;
        public float velocitySmoothing;
        public float returnedDistance = .1f;
        public Vector2 idleDir = Vector2.down;

        [Space]
        [Header("Runtime")]
        public Transform player;
        public WeaponState state;
        public WeaponInstance data;
        float returnSpeed = 0;

        [Space]
        [Header("References")]
        Rigidbody2D rb;
        public ParticleSystem trailParticle;
        public TrailRenderer trailTrail;
        public new Collider2D collider;
        public LayerMask targetLayer;
        public LayerMask groundLayer;

        public bool isIdle => state == WeaponState.idle;
        public bool isShot => state == WeaponState.shot;
        public bool isReturning => state == WeaponState.returning;
        bool useVisuals => isReturning || isShot;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            state = WeaponState.idle;
        }

        public void Initialize(int index, Transform player, WeaponInstance data)
        {
            name = $"{name} {index}";
            this.player = player;
            this.data = data;
        }

        private void FixedUpdate()
        {
            trailTrail.emitting = useVisuals;
            if (useVisuals)
            {
                if (rb.velocity.sqrMagnitude > Mathf.Epsilon)
                    transform.right = rb.velocity.normalized;

                if (trailParticle.isStopped)
                    trailParticle.Play();
            }
            else if (trailParticle.isPlaying)
                trailParticle.Stop();

            collider.enabled = !isIdle;

            if (isReturning)
            {
                if ((player.transform.position - transform.position).sqrMagnitude <= returnedDistance * returnedDistance)
                {
                    state = WeaponState.idle;
                }

                returnSpeed += returnAccel * Time.fixedDeltaTime;
                var dirToPlayer = (player.transform.position - transform.position).normalized;
                rb.velocity = Vector2.Lerp(rb.velocity, dirToPlayer * returnSpeed, Time.fixedDeltaTime * velocitySmoothing);
            }
        }

        public void MoveTowards(Vector2 position)
        {
            var dir = position - transform.position.ToV2();
            rb.velocity = dir.normalized * dir.magnitude * velocitySmoothing * 3;
            transform.right = idleDir.normalized;
        }

        public void ShootProjectile(Vector2 goal) => ShootProjectile(goal, data.Speed);
        public void ShootProjectile(Vector2 goal, float speed)
        {
            rb.velocity = (goal - transform.position.ToV2()).normalized * speed;
            state = WeaponState.shot;
        }
        public void Recall()
        {
            returnSpeed = rb.velocity.magnitude;
            state = WeaponState.returning;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            //prevents the hitbox from dealing damage when it is stationary
            if (rb.velocity.sqrMagnitude <= Mathf.Epsilon)
                return;

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
            if (isReturning)
                return;
            // Broke, dont use
            //if (data.bouncing)
            //{
            //    var hit = Physics2D.Raycast(transform.position, transform.right);
            //    rb.velocity = GetReflectDir(rb.velocity.normalized, hit.point) * rb.velocity.magnitude * .75f;
            //}
            //else
            //{
            rb.velocity = Vector2.zero;
            //}
        }
        protected virtual void OnHitTarget(Entity entity)
        {
            if (isReturning)
                entity.Damage(data.GetRecallDamageContainer());
            else
                entity.Damage(data.GetDamageContainer());

            if (!data.CanPierce())
                rb.velocity = Vector2.zero;
        }
        //https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
        public static bool LayerIsInMask(int layer, LayerMask mask)
        {
            return mask == (mask | (1 << layer));
        }
        public static Vector2 GetReflectDir(Vector2 dir, Vector2 normal)
        {
            //https://stackoverflow.com/questions/573084/how-to-calculate-bounce-angle

            var u = (Vector2.Dot(dir, normal) / Vector2.Dot(normal, normal)) * normal;
            var w = dir - u;

            return w - u;
        }
    }
}