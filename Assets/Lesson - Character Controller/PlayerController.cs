using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movement_speed = 5f;
    [SerializeField] float rotation_speed = 500f;
    [SerializeField] float gravity_multiplier = 2f;
    [SerializeField] float jump_force = 10f;

    private CharacterController character_controller;
    private float downward_velocity;

    // Start is called before the first frame update
    void Start()
    {
        character_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float move_amount = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        Vector3 velocity = new Vector3(horizontal, 0f, vertical).normalized * movement_speed;
        velocity = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z)) * velocity;

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

            if(Input.GetButtonUp("Jump") && downward_velocity > 0f)
            {
                downward_velocity *= 0.5f;
            }
        }

        velocity.y = downward_velocity;
        character_controller.Move(velocity * Time.deltaTime);

        if(move_amount > 0)
        {
            var target_rotation = Quaternion.LookRotation(new Vector3(velocity.x, 0f, velocity.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);
        }
    }
}
