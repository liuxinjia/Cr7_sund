using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_PlayerController
{

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Controller2D_Rigi : Controller2D
    {
        Rigidbody _body;
        Collider _collider;

        Vector3 _move;
        private void FixedUpdate()
        {
            _body.MovePosition(_body.position + _move * MoveSpeed * Time.fixedDeltaTime);
        }
        
        protected override void InitController()
        {
            base.InitController();
            controllerType = ControllerType.Rigi;

            _body = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public override void Move(Vector2 input)
        {
            _move = new Vector3(input.x, input.y, 0);

            float GroundDistance = _move.y + .1f;
            _isGrounded = Physics.CheckSphere(_groundCheck.position, GroundDistance, GroundMask, QueryTriggerInteraction.Ignore);

            if (_move != Vector3.zero)
            {
                transform.forward = _move;
            }

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            }
            if (Input.GetButtonDown("Dash"))
            {
                var dashVelocity = Vector3.Scale(transform.forward,
                                                 DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime),
                                                                             0,
                                                                            (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));

                _body.AddForce(dashVelocity, ForceMode.VelocityChange);

            }
        }
    }
}
