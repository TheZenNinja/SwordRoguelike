using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TestReflect : MonoBehaviour
{

    private void OnDrawGizmos()
    {
        var hit = Physics2D.Raycast(transform.position, transform.right);

        Gizmos.color = Color.red;
        if (hit.collider == null)
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.right * 10);
            return;
        }
        else
            Gizmos.DrawLine(transform.position, hit.point);

        //https://stackoverflow.com/questions/573084/how-to-calculate-bounce-angle
        var normal = hit.normal;

        var u = (Vector2.Dot(transform.right, normal) / Vector2.Dot(normal, normal)) * normal;
        var w = transform.right.ToV2() - u;

        var newDir = w - u;
        Gizmos.DrawLine(hit.point, hit.point + newDir);
    }
}
