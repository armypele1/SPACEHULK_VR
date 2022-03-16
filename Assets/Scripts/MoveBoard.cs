using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveBoard : MonoBehaviour
{
    public InputActionReference horizontalMove = null;
    public InputActionReference verticalMove = null;

    private float speed = 1;
    private Vector2 horizontalMoveInput;
    private Vector2 verticalMoveInput;
    private GameObject boardgame;
    private Rigidbody rbody;
    private Camera mainCamera;



    // Start is called before the first frame update
    private void Awake()
    {
        boardgame = GameObject.Find("Boardgame");
        rbody = GetComponent<Rigidbody>();
        rbody.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mainCamera = Camera.main;
        var forward = mainCamera.transform.forward;
        var right = mainCamera.transform.right;

        horizontalMoveInput = horizontalMove.action.ReadValue<Vector2>();

        float horizontalAxis = horizontalMoveInput.x;
        float verticalAxis = horizontalMoveInput.y;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //this is the direction in the world space we want to move:
        var desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;
        
        //apply vertical movement with right stick
        verticalMoveInput = verticalMove.action.ReadValue<Vector2>();
        desiredMoveDirection.y = verticalMoveInput.y;

        //rbody.velocity = desiredMoveDirection * speed;
        transform.Translate(desiredMoveDirection * speed * Time.deltaTime);
    }


}
