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
        var newPos = refPos + dir ;
        refPos = new Vector3(refPos.x, newPos.y, refPos.z);
        transform.localPosition = refPos;// new Vector3(newPos.x + offset.x, newPos.y + offset.y, newPos.z );
    }
}
