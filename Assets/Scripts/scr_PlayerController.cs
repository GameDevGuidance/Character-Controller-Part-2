using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerController : MonoBehaviour
{
    [SerializeField] float movement_speed = 5f;
    [SerializeField] float rotation_speed = 500f;
    [SerializeField] float gravity_multiplier = 2f;
    [SerializeField] float jump_force = 12f;

    private CharacterController character_controller;
    private float downward_velocity = 0f;

    private void Start()
    {
        character_controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float move_amount = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        Vector3 velocity = new Vector3(horizontal, 0f, vertical).normalized * movement_speed;

        if(character_controller.isGrounded)
        {
            downward_velocity = -2f;

            if(Input.GetButtonDown("Jump"))
            {
                downward_velocity = jump_force;
            }
        }
        else
        {
            downward_velocity += Physics.gravity.y * gravity_multiplier * Time.deltaTime;

            if(Input.GetButtonUp("Jump") && downward_velocity > 0)
            {
                downward_velocity *= .5f;
            }
        }

        velocity.y = downward_velocity;

        character_controller.Move(velocity * Time.deltaTime);

        // Handle Rotation
        if(move_amount > 0)
        {
            var target_rotation = Quaternion.LookRotation(new Vector3(velocity.x, 0f, velocity.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);
        }
    }
}
