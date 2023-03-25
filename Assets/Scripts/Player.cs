using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference pointer;


    [Space]
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float accel;
    [SerializeField] Vector2 moveVel;
    [SerializeField] float minSpeedForAnim = 0.1f;
    public bool flipped;

    [Space]
    [Header("References")]
    [SerializeField] Camera mainCamera;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator playerSpriteAnimator;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        flipped = GetMousePositionWorld().x < transform.position.x;
        playerSprite.flipX = flipped;

        playerSpriteAnimator.SetBool("Walk", moveVel.sqrMagnitude > minSpeedForAnim);
    }


    private void FixedUpdate()
    {

        Vector2 goalVel = moveInput.action.ReadValue<Vector2>().normalized * speed;

        moveVel = Vector2.Lerp(moveVel, goalVel, Time.fixedDeltaTime * accel);

        rb.velocity = moveVel;
    }

    public Vector2 GetMousePositionWorld() => mainCamera.ScreenToWorldPoint(pointer.action.ReadValue<Vector2>());
    public Vector2 DirToMouse(Vector2 rootPos) => (GetMousePositionWorld() - rootPos).normalized;
    public Vector2 DirToMouse() => (GetMousePositionWorld() - transform.position.ToV2()).normalized;
}
