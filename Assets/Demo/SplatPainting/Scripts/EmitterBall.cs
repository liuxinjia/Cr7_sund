using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_Demo
{
    public class EmitterBall : MonoBehaviour
    {
        Rigidbody emitterBall;
        public float hitForce = 5.0f;
        public float hitTolerance = .15f;
        public float hitDestroyDelay = 6.0f;

        Vector3 hitPos;

        private void Update()
        {
            if (emitterBall != null)
            {
                var position = emitterBall.transform.position;
                var distance = Mathf.Abs((hitPos - position).magnitude);

                if (distance < hitTolerance)
                {
                    // Destroy(gameObject);
                }
            }
        }
        public void Initiliaze(Vector3 localPos, Vector3 originalDirection, Vector3 _hitPos)
        {
            hitPos = _hitPos;

            transform.localScale *= .15f;
            transform.position = localPos;
            transform.name = "emitterBall";
            emitterBall = gameObject.AddComponent<Rigidbody>();

            // originalDirection *= -1.0f;
            originalDirection.y *= hitForce;
            emitterBall.AddForce(originalDirection * hitForce, ForceMode.VelocityChange);

            hitDestroyDelay = Mathf.Min(10.0f, Mathf.Abs((localPos - hitPos).magnitude) * .6f);
            Debug.Log(Mathf.Abs((localPos - hitPos).magnitude));
            StartCoroutine(DestroyEmitterBall());
        }

        IEnumerator DestroyEmitterBall()
        {
            yield return new WaitForSeconds(hitDestroyDelay);
            if (emitterBall != null)
                Destroy(gameObject);
        }
    }
}