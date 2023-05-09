using System.Linq;
using UnityEngine;

namespace ContextSteering
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Parameter Vars")]
        public bool debug;
        public float searchRadius;
        public float stopDistance;
        
        [Space]
        public float minMoveDist = 0.1f;
        public float moveSpeed, moveAccel;
        public float minMoveSpeedToAnimate = 0.1f;
        [Range(0.01f, 0.1f)]
        public float updateRate = 0.05f;
        private float updateDelay = 0;

        [Space]
        [Header("Attacking Vars")]
        public float attackDistance;
        public float attackDelay;
        float currentAtkDelay;
        bool canFireProjectile => currentAtkDelay <= 0;
        [Space]
        public CombatSystem.Projectile projectile;
        public float projectileSpeed = 6;


        [Space]
        [Header("Runtime Vars")]
        public Transform player;
        public bool isInLoS;
        public Vector2 lastKnownPlayerLoc;
        public Vector2 goalPosition;
        public float distToTarget = 0;

        [Space]
        [Header("References")]
        public Entity entity;
        public ContextSteeringV2 ctxSteering;
        public LayerMask terrainMask;
        public LayerMask playerMask;
        public LayerMask enemyMask;
        public Animator animator;
        public SpriteRenderer sprite;
        Rigidbody2D rb;

        public bool inAttackRange => player != null && distToTarget <= stopDistance;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            updateDelay = 0;
            GetComponent<Entity>().onDie += () => Destroy(gameObject);

            //ctxSteering.nearbyAI = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None).ToList().Except(new EnemyAI[] { this }).Select(x => x.GetComponent<Collider2D>()).ToArray();
        }
        private void Update()
        {
            animator.SetBool("IsMoving", rb.velocity.sqrMagnitude > minMoveSpeedToAnimate);
            if (player != null)
                sprite.flipX = player.position.x < transform.position.x;
            else
                sprite.flipX = goalPosition.x < transform.position.x;
        }
        private void FixedUpdate()
        {
            if (updateDelay <= 0)
            {
                updateDelay = updateRate;

                GetNearbyAI();
                FindTarget();
                GetTargetLocation();
                MoveToObjective();
                TryAttack();
            }
            updateDelay -= Time.fixedDeltaTime;
            if (currentAtkDelay > 0)
                currentAtkDelay -= Time.fixedDeltaTime;
        }

        public void FindTarget()
        {
            try
            {
                var cols = Physics2D.OverlapCircleAll(transform.position, searchRadius, playerMask);
                var target = cols.Where(c => c.GetComponent<Player>() != null).ElementAt(0);
                player = target.transform;

                isInLoS = IsInLineOfSight(player.gameObject);
                if (isInLoS)
                    lastKnownPlayerLoc = player.transform.position;
            }
            catch (System.Exception)
            {
                player = null;
            }
        }
        public bool IsInLineOfSight(GameObject target)
        {
            var dir = (target.transform.position - transform.position).normalized;
            var hit = Physics2D.Raycast(transform.position, dir, searchRadius, terrainMask | playerMask);
            if (hit.collider != null && hit.collider.gameObject == target)
                return true;
            return false;
        }
        public void GetNearbyAI()
        {
            //TODO convert to collider with OnEnter and OnExit methods?
            var cols = Physics2D.OverlapCircleAll(transform.position, searchRadius, enemyMask);
            ctxSteering.nearbyAI = cols;
        }
        public void GetTargetLocation()
        {
            if (isInLoS)
            {
                var targetDifference = (transform.position.ToV2() - lastKnownPlayerLoc);
                distToTarget = targetDifference.magnitude;
                goalPosition = lastKnownPlayerLoc + targetDifference.normalized * stopDistance;
            }
            else
                goalPosition = lastKnownPlayerLoc;
        }
        public void MoveToObjective()
        {
            if (goalPosition == Vector2.zero)
                return;

            var moveDir = ctxSteering.GetGoalDirection(goalPosition);
            if ((transform.position.ToV2() - goalPosition).sqrMagnitude < minMoveDist * minMoveDist)
                moveDir = Vector2.zero;

            rb.velocity = Vector2.Lerp(rb.velocity, moveDir * moveSpeed, Time.fixedDeltaTime * moveAccel);
            if (rb.velocity.sqrMagnitude < 0.1f)
                rb.velocity = Vector2.zero;
        }
        public void TryAttack()
        {
            if (player == null ||
                Vector2.Distance(transform.position, player.position) > attackDistance ||
                entity.IsFrozen ||
                !canFireProjectile)
                return;

            var dir = (player.position - transform.position).normalized;
            var p = Instantiate(projectile, transform.position, Quaternion.identity, null);
            p.SetDir(dir);
        }
        private void OnDrawGizmos()
        {
            if (!debug)
                return;

            if (lastKnownPlayerLoc != Vector2.zero)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(lastKnownPlayerLoc, 0.2f);

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(lastKnownPlayerLoc, stopDistance);
            }
            if (goalPosition != Vector2.zero)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(goalPosition, 0.2f);
            }
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }
    }
}