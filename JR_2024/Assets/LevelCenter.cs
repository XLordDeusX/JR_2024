using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCenter : MonoBehaviour
{
    public float HalfXBounds = 20f;
    public float HalfYBounds = 15f;
    public float HalfZBounds = 15f;
    [HideInInspector] public Bounds FocusBounds;

    private void Update()
    {
        Vector3 position = gameObject.transform.position;
        Bounds bounds = new Bounds();
        bounds.Encapsulate(new Vector3(position.x - HalfXBounds, position.y - HalfYBounds, position.z - HalfZBounds));
        bounds.Encapsulate(new Vector3(position.x + HalfXBounds, position.y + HalfYBounds, position.z - HalfZBounds));
        bounds.Encapsulate(new Vector3(position.x + HalfXBounds, position.y + HalfYBounds, position.z + HalfZBounds));
        FocusBounds = bounds;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(HalfXBounds*2, HalfYBounds*2, HalfZBounds*2));
    }
#endif
}
