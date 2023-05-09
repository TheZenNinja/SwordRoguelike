using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ContextSteeringTut
{
    public class TargetDetector : MonoBehaviour, IDetector
    {
        [SerializeField] float range = 5;

        [SerializeField] LayerMask targetLayer, obsLayer;

        Transform target;

        Transform nonRayTarget;

        public void Detect(AIData aiData)
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, range, targetLayer);

            if (player != null)
                nonRayTarget = player.transform;
            else
                nonRayTarget = null;

            if (player != null)
            {
                var dir = (player.transform.position - transform.position).normalized;
                var hit = Physics2D.Raycast(transform.position, dir, range, (obsLayer | targetLayer));

                if (hit.collider != null && hit.collider == player)
                {
                    target = player.transform;
                }
                else
                {
                    target = null;
                }
            }
            else
                target = null;

            aiData.currentTarget = target;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, range);

            if (nonRayTarget != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(nonRayTarget.position, 0.25f);

                if (target == null)
                {
                    Gizmos.color = Color.red;
                    var dir = (nonRayTarget.position - transform.position).normalized;
                    var hit = Physics2D.Raycast(transform.position, dir, obsLayer);
                    Gizmos.DrawSphere(hit.point, .2f);
                }

                Gizmos.DrawLine(nonRayTarget.position, transform.position);
            }

            if (target != null)
            {

                Gizmos.color = Color.green;
                Gizmos.DrawSphere(target.position, .2f);
            }
        }
    }
}
