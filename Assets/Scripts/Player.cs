using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference pointer;
    [SerializeField] InputActionReference dodge;

    [Space]
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float accel;
    [SerializeField] bool canMove = true;
    [SerializeField] Vector2 moveVel;
    [SerializeField] float minSpeedForAnim = 0.1f;
    [SerializeField] float dodgeDistance;
    [SerializeField] float dodgeDuration;
    [SerializeField] float dodgeCooldown;
    float currentCooldown = 0;
    bool canDodge => currentCooldown <= 0;
    public bool flipped;

    [Space]
    [Header("References")]
    [SerializeField] Camera mainCamera;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator animator;
    [SerializeField] Entity playerEntity;
    [SerializeField] RectTransform HPBar;
    [SerializeField] TextMeshProUGUI hpTxt;
    [SerializeField] TrailRenderer trail;
    Rigidbody2D rb;

    public Vector2 velocity => rb.velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null)
            mainCamera = Camera.main;

        currentCooldown = 0;
        trail.emitting = false;
        canMove = true;
        playerEntity.onDie += () => SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        flipped = GetMousePositionWorld().x < transform.position.x;
        playerSprite.flipX = flipped;

        animator.SetBool("IsMoving", moveVel.sqrMagnitude > minSpeedForAnim);

        if (dodge.action.triggered && canDodge)
            StartCoroutine(Dodge());

        HPBar.localScale = new Vector3(playerEntity.HealthPercent, 1, 1);
        hpTxt.text = $"{playerEntity.Health}/{playerEntity.MaxHealth}";
    }

    IEnumerator Dodge()
    {
        var dir = moveInput.action.ReadValue<Vector2>().normalized;
        var vel = dodgeDistance / dodgeDuration;

        trail.emitting = true;

        canMove = false;
        rb.velocity = dir * vel;
        yield return new WaitForSeconds(dodgeDuration);
        canMove = true;

        trail.emitting = false;

        currentCooldown = dodgeCooldown;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 goalVel = moveInput.action.ReadValue<Vector2>().normalized * speed;
            moveVel = Vector2.Lerp(moveVel, goalVel, Time.fixedDeltaTime * accel);
            rb.velocity = moveVel;
        }

        if (currentCooldown > 0)
            currentCooldown -= Time.fixedDeltaTime;
    }

    public Vector2 GetMousePositionWorld() => mainCamera.ScreenToWorldPoint(pointer.action.ReadValue<Vector2>());
    public Vector2 DirToMouse(Vector2 rootPos) => (GetMousePositionWorld() - rootPos).normalized;
    public Vector2 DirToMouse() => (GetMousePositionWorld() - transform.position.ToV2()).normalized;
}
