using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Hammer : MonoBehaviour
{
    [SerializeField]
    private Vector3 Offset = Vector3.zero;

    [SerializeField]
    private float PlaneY = 1.6f;

    private Plane Plane;

    private void Start()
    {
        Plane = new Plane(Vector3.up, new Vector3(0.0f, PlaneY, 0.0f));
    }

    void Update()
    {
        float t;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane.Raycast(ray, out t);
        var point = ray.GetPoint(t);

        transform.position = point + Offset;
    }
}
