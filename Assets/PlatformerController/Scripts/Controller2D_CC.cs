using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_PlayerController
{
    [RequireComponent(typeof(CharacterController))]
    public class Controller2D_CC : Controller2D
    {
        #region  fields

        protected CharacterController controller;

        #endregion


        #region methods

        private void Start()
        {


        }

        protected override void InitController()
        {
            base.InitController();
            controllerType = ControllerType.CC;

            controller = GetComponent<CharacterController>();
        }

        public override void Move(Vector2 move)
        {
            float GroundDistance = move.y + .1f;
            _isGrounded = Physics.CheckSphere(_groundCheck.position, GroundDistance, GroundMask, QueryTriggerInteraction.Ignore);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = 0.0f;
            }

            var v1 = new Vector3(move.x, move.y, 0) * Time.deltaTime * MoveSpeed;

            if (move != Vector2.zero)
            {
                transform.forward = move;
            }

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _velocity.y += Mathf.Sqrt(JumpHeight * -2.0f * gravity);
            }
            if (Input.GetButtonDown("Dash"))
            {
                _velocity += Vector3.Scale(transform.forward,
                 DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime),
                                             0,
                                            (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));

            }

            _velocity.y += gravity * Time.deltaTime;

            //dash
            _velocity.x /= 1 + Drag.x * Time.deltaTime;
            _velocity.y /= 1 + Drag.y * Time.deltaTime;
            _velocity.z /= 1 + Drag.z * Time.deltaTime;

            controller.Move(_velocity * Time.deltaTime + v1);
        }

        #endregion
    }
}

