using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_Demo
{
    public class Target : SplatPainting
    {

        #region  fields
        //general
        public Shaker screenShaker;
        public float splatOffset;


        //target info
        public Texture2D deathTexture;
        float healthPoints = 100f;
        bool isDead = false;
        private float shakeMagnitude;
        private float shakeDuration;

        #endregion

        #region methods

        protected override void InitInfo()
        {
            base.InitInfo();

            if (screenShaker == null)
            {
                screenShaker = GameObject.FindGameObjectWithTag(Tags.Shaker).GetComponent<Shaker>();
            }

            if (screenShaker != null)
            {
                shakeMagnitude = screenShaker.magnitude * healthPoints / 300f;
                shakeDuration = screenShaker.duration * healthPoints / 300f;
            }

        }

        public void HandleHit(float damage, Vector3 direction, Vector2 texCoord, Texture2D texture)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0f);

            if (healthPoints <= 0.0f)
            {
                HandleDeath(texCoord);
            }
            else
            {
                var hitPos = (Vector3)transform.position + splatOffset * direction;
                PaintSplat(texCoord, texture);
            }
        }

        void HandleDeath(Vector2 texCoord)
        {
            if (isDead) return;
            isDead = true;

            PaintSplat(texCoord, deathTexture);

            if (screenShaker != null)
            {
                screenShaker.magnitude = shakeMagnitude;
                screenShaker.duration = shakeDuration;
                screenShaker.Shake();
            }
            Destroy(this.gameObject);
        }


        #endregion
    }

}