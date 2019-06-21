using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_Demo
{
    [RequireComponent(typeof(LineRenderer))]
    // [RequireComponent(typeof(Rigidbody))]
    public class MouseShooter : MonoBehaviour
    {
        #region fields
        public Texture2D splashTexture_Wall;
        public Texture2D splashTexture_Target;

        public Camera followCamera;

        //shooter
        [Space(10)]
        [Range(2.0f, 10.0f)]
        public float rayMaxDistance = 10.0f;
        [Range(.1f, .15f)]
        public float shootingTimeOut = .10f;
        [Space(10)]
        [Range(.80f, 20.0f)]
        public float recoilStrength = 1.0f;
        [Space(10)]
        [Range(25.0f, 30.0f)]
        public float hitDamage = 25f;


        LineRenderer m_lineRender;
        // Rigidbody m_rigibody;
        bool shootWhilePressed = false;
        float lastShotTime;
        public const float LineHeight = 0.5f;
        public bool useCurve = true;
        public float lineCurveWidth = .05f;


        //Emit ball
        Vector3 hitPos;

        #endregion

        #region methods

        private void Start()
        {
            m_lineRender = GetComponent<LineRenderer>();
            // m_rigibody = GetComponent<Rigidbody>();

            for (int i = 0; i < transform.childCount; i++)
            {
                var obj = transform.GetChild(i).GetComponent<Camera>();
                if (obj != null)
                {
                    followCamera = obj;
                    break;
                }
            }

            lastShotTime = Time.time;
        }

        void Update()
        {
            HandleInput();
        }

        void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandelShoot();
            }
        }

        void EmitBall(Vector3 localPos, Vector3 originalDirection)
        {
            var rigiBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var script = rigiBall.AddComponent<EmitterBall>();
            script.Initiliaze(localPos, originalDirection, hitPos);
        }


        public void HandelShoot()
        {
            RaycastHit hit;

            // if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            if (Physics.Raycast(followCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                var hitObject = hit.transform;
                var localPos = new Vector3(transform.position.x, LineHeight, transform.position.z);
                hitPos = new Vector3(hit.point.x, LineHeight, hit.point.z);
                var curDirection = hitPos - localPos;

                var hitTarget = hitObject.gameObject.GetComponent<Target>();
                if (hitTarget != null)
                {
                    // m_rigibody.AddForce(recoilStrength * direction * -1f, ForceMode.Impulse);
                    hitTarget.HandleHit(hitDamage, curDirection, hit.textureCoord, splashTexture_Target);
                }
                else
                {
                    var splatPainting = hitObject.gameObject.GetComponent<SplatPainting>();
                    if (splatPainting != null)
                    {
                        splatPainting.PaintSplat(hit.textureCoord, splashTexture_Wall);
                    }
                }

                //line renderer
                m_lineRender.SetPosition(0, localPos);
                if ((hitPos - localPos).magnitude < rayMaxDistance)
                {
                    m_lineRender.SetPosition(1, hitPos);
                }
                else
                {
                    m_lineRender.SetPosition(1, localPos + curDirection * rayMaxDistance);
                }

                AnimationCurve curve = new AnimationCurve();
                if (useCurve)
                {
                    curve.AddKey(0.0f, 0.0f);
                    curve.AddKey(1.0f, 1.0f);
                }
                else
                {
                    curve.AddKey(0.0f, 1.0f);
                    curve.AddKey(1.0f, 1.0f);
                }
                m_lineRender.widthCurve = curve;
                m_lineRender.widthMultiplier = lineCurveWidth;

                EmitBall(localPos, curDirection);

            }

        }

        #endregion
    }
}
