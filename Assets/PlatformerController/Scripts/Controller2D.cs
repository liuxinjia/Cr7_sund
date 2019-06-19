using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_PlayerController
{

    public abstract class Controller2D : MonoBehaviour
    {
        #region  fields


        #region generalVariables

        [HideInInspector]
        public ControllerType controllerType;
        
        public float MoveSpeed = 5.0f;
        protected Vector3 _velocity;

        public float JumpHeight = 4.0f;
        public float gravity = -9.8f;

        public float DashDistance = 2.0f;
        protected Vector3 Drag = new Vector3(8, 0, 8);

        #endregion

        #region  collisonVariables

        protected Transform _groundCheck;
        public LayerMask GroundMask;
        protected bool _isGrounded;

        #endregion

        #endregion

        #region Methods

        private void Awake()
        {
            InitController();
        }

        protected virtual void InitController()
        {
            _groundCheck = transform.GetChild(0);
            if (_groundCheck.name != "GroundCheck")
                Debug.LogError("Not found GroundCheck : Wrong child index");

            if (GroundMask == 0)
            {
                GroundMask = LayerMask.GetMask("Obstacle");
            }
        }

        public abstract void Move(Vector2 move);

        #endregion

        #region  Enum

        public enum ControllerType
        {
            Rigi,
            CC
        }


        #endregion
    }
}