using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class DrawLine : MonoBehaviour
{
    // Apply these values in the editor
    public LineRenderer LineRenderer;
    public Transform TransformOne;
    public Transform TransformTwo;
 
    void Start()
    {
        // set the color of the line
        LineRenderer.startColor = Color.red;
        LineRenderer.endColor = Color.red;
 
        // set width of the renderer
        LineRenderer.startWidth = 0.01f;
        LineRenderer.endWidth = 0.01f;
 
        // set the position
        LineRenderer.SetPosition(0, TransformOne.position);
        LineRenderer.SetPosition(1, TransformTwo.position);
    }
    void Awake()
    {
        LineRenderer.SetPosition(0, TransformOne.position);
        LineRenderer.SetPosition(1, TransformTwo.position);
    }

    void Update()
    {
        LineRenderer.SetPosition(0, TransformOne.position);
        LineRenderer.SetPosition(1, TransformTwo.position);
    }
}
