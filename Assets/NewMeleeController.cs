using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem;
using Sirenix.OdinInspector;

public class NewMeleeController : MonoBehaviour
{
    [Header("Projectile Data")]
    [InlineEditor]
    public WeaponTypeData weaponType;

    public LayerMask groundLayer;
    public LayerMask targetLayer;

    private float attackCooldown;
    private bool attackCooldownIsOver => attackCooldown <= 0;

    [Space]
    [Header("Anim")]
    public int attackSequence;
    public int maxAttackSequence;

    [Space]
    [Header("References")]
    public InputActionReference fire;
    public Transform playerHands;
    public Animator anim;
    Player player;

    public WeaponInstance weaponInstance;


    public void Equip(WeaponInstance weapon)
    {
        weaponInstance = weapon;
        gameObject.SetActive(true);
        //transform.GetChild(0).gameObject.SetActive(true);

    }
    public void Unequip()
    { 
    
        gameObject.SetActive(false);
        //transform.GetChild(0).gameObject.SetActive(false);
    }

    void Start()
    {
        player = GetComponentInParent<Player>();
        attackSequence = 1;
        attackCooldown = 0;
        weaponInstance = WeaponGenerator.GenerateWeapon(WeaponType.sword, InventorySystem.ItemRarity.Common);
    }

    void Update()
    {
        var dir = player.DirToMouse();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        playerHands.eulerAngles = new Vector3(0, 0, angle);
        playerHands.localScale = player.flipped ? new Vector3(1, -1, 1) : Vector3.one;


        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;

        if (fire.action.IsPressed() && attackCooldownIsOver)
        {
            Debug.Log("Fire");
            anim.InterruptMatchTarget();
            anim.Play($"Attack {attackSequence}");
            attackSequence++;
            if (attackSequence > maxAttackSequence)
                attackSequence = 1;
            Attack(dir);
        }
    }
    void Attack(Vector2 dir)
    {
        attackCooldown = weaponType.GetAttackSpeed(weaponInstance);

        var p = Instantiate(weaponType.projectile, transform.position + dir.ToV3() * weaponType.projectileOffset, Quaternion.identity);

        //var extraVel = Vector2.Dot(dir, player.velocity) > 0 ? player.velocity : Vector2.zero; 
        float speedMulti = Vector2.Dot(dir, player.velocity.normalized).Remap(-1, 1, 0.75f, 1.5f);

        p.SetVelocity(dir, weaponType.GetAttackRange(weaponInstance), weaponType.projectileLifetime, speedMulti);
        p.SetLayers(groundLayer, targetLayer);
        p.SetDamage(new DamageContainer(weaponType.GetDamage(weaponInstance), weaponInstance.element, WillInflictStatus()));
    }

    bool WillInflictStatus() => Random.value >= weaponType.GetStatus(weaponInstance);

    private void OnDrawGizmosSelected()
    {
        if (weaponType == null)
            return;

        Gizmos.color = Color.red;

        Vector3 projStartPos = transform.position + transform.right * weaponType.projectileOffset;

        Gizmos.DrawWireSphere(projStartPos, 0.1f);
        Vector3 endPos = projStartPos + transform.right * weaponType.attackRange;

        Gizmos.DrawLine(projStartPos, endPos);
        Gizmos.DrawLine(endPos + transform.up * .2f, endPos - transform.up * .2f);

    }
}
