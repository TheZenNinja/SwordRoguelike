using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem;
using Sirenix.OdinInspector;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Projectile Data")]
    [InlineEditor]
    public WeaponTypeData weaponType;
    
    public LayerMask groundLayer;
    public LayerMask targetLayer;

    private float attackCooldown;
    private bool attackCooldownIsOver => attackCooldown <= 0;

    [Space]
    [Header("References")]
    public InputActionReference fire;
    public Transform playerHands;
    Player player;

    public WeaponInstance weaponInstance;

    void Start()
    {
        player = GetComponentInParent<Player>();
        attackCooldown = 0;
        weaponInstance = WeaponGenerator.GenerateWeapon(WeaponType.sword, InventorySystem.ItemRarity.Common);
    }

    void Update()
    {
        var dir = player.DirToMouse();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        playerHands.eulerAngles = new Vector3(0, 0, angle);
        playerHands.localScale = player.flipped ? new Vector3(1,-1,1) : Vector3.one;


        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;

        if (fire.action.IsPressed() && attackCooldownIsOver)
        {
            Debug.Log("Fire");
            Attack(dir);
        }
    }
    void Attack(Vector2 dir)
    {
        attackCooldown = weaponType.attackSpeed;

        var p = Instantiate(weaponType.projectile, transform.position + dir.ToV3() * weaponType.projectileOffset, Quaternion.identity);

        p.SetVelocity(dir, weaponType.attackRange, weaponType.projectileLifetime);
        p.SetLayers(groundLayer, targetLayer);
        p.SetDamage(new DamageContainer(weaponType.damage.RoundToInt(), ElementalType.Physical, WillInflictStatus()));
    }

    bool WillInflictStatus() => weaponType.statusChance >= Random.value;

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
