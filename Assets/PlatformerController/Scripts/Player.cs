using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_PlayerController
{
    [RequireComponent(typeof(Controller2D))]
    public class Player : MonoBehaviour
    {
        Controller2D controller;

        private void Start()
        {
            controller = GetComponent<Controller2D>();

        }

        private void Update()
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            controller.Move(input);
        }
    }

}