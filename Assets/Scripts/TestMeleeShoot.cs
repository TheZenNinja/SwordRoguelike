using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMeleeShoot : MonoBehaviour
{
    public MeleeProjectile projectile;

    public InputActionReference fire, pointer;

    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector2 GetMousePositionWorld() => cam.ScreenToWorldPoint(pointer.action.ReadValue<Vector2>());
    public Vector2 DirToMouse(Vector2 rootPos) => (GetMousePositionWorld() - rootPos).normalized;

    void Update()
    {
        var dirToMouse = DirToMouse(transform.position.ToV2());

        if (fire.action.triggered)
        {
            var p = Instantiate(projectile, transform.position, Quaternion.identity);
            p.SetVelocity(dirToMouse.normalized);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + DirToMouse(transform.position.ToV2()).ToV3());
    }
}
