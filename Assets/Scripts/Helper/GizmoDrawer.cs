using System;
using UnityEngine;

namespace Helper
{
    public class GizmoDrawer : MonoBehaviour
    {
        [SerializeField] GizmoType gizmoType; 
        [SerializeField] float radius;
        [SerializeField] Color color = Color.white;

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            switch (gizmoType)
            {
                case GizmoType.Sphere:
                    Gizmos.DrawWireSphere(transform.position, radius);
                    break;
                case GizmoType.Cube:
                    Gizmos.DrawWireCube(transform.position, Vector3.one * radius);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum GizmoType
    {
        Sphere,
        Cube
    }
}
