using System;
using UnityEngine;

public class LookTarget : MonoBehaviour
{
    [SerializeField] Transform referenceObject;
    [SerializeField] Vector3 offset;    
    private void LateUpdate()
    {
        var refPos = referenceObject.localPosition + offset;
        var dir = referenceObject.TransformDirection(Vector3.forward) * offset.z;
        var newPos = refPos + dir + new Vector3(offset.x, offset.y , 0);
        transform.localPosition = newPos;// new Vector3(newPos.x + offset.x, newPos.y + offset.y, newPos.z );
    }
}
