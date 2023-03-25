using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Projectile Data")]
    public float p_distance;
    public float p_lifetime;
    public MeleeProjectile projectile;
    public float offsetDist;
    public LayerMask groundLayer;
    public LayerMask targetLayer;

    [Space]
    [Header("Damage")]
    public int damage;
    public ElementalType element;
    [Range(0, 1)]
    public float statusChance;
    public float impact;

    [Space]
    [Header("References")]
    public InputActionReference fire;
    public Transform playerHands;
    Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        var dir = player.DirToMouse();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        playerHands.eulerAngles = new Vector3(0, 0, angle);
        playerHands.localScale = player.flipped ? new Vector3(1,-1,1) : Vector3.one;


        if (fire.action.triggered)
        {
            Attack(dir);
        }
    }
    void Attack(Vector2 dir)
    {
        //var dir = player.DirToMouse();

        var p = Instantiate(projectile, transform.position + dir.ToV3() * offsetDist, Quaternion.identity);

        p.SetVelocity(dir, p_distance, p_lifetime);
        p.SetLayers(groundLayer, targetLayer);
        p.SetDamage(new DamageContainer(damage, element, WillInflictStatus(), impact));
    }

    bool WillInflictStatus() => statusChance >= Random.value;
}
