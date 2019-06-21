using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_Demo
{
    // [RequireComponent(typeof(MeshCollider))]
    public class SplatPainting : MonoBehaviour
    {
        private const int textureHeight = 256;
        private const int textureWidth = 256;
        private readonly Color c_color = new Color(0, 0, 0, 0);

        Material m_material;
        Texture2D m_texture;
        private bool isEnabled = false;
        public Color splashTextureColor;

        private void Start()
        {
            InitInfo();
        }

        protected virtual void InitInfo()
        {
            var meshCollider = GetComponent<MeshCollider>();
            if (meshCollider == null)
                gameObject.AddComponent<MeshCollider>();

            var renderer = GetComponent<Renderer>();
            if (null != renderer)
            {
                foreach (var item in renderer.materials)
                {
                    if (item.shader.name == "Custom/SplatShader")
                    {
                        m_material = item;
                        break;
                    }
                }

                if (null != m_material)
                {
                    m_texture = new Texture2D(textureWidth, textureHeight);
                    for (int x = 0; x < textureWidth; x++)
                    {
                        for (int y = 0; y < textureHeight; y++)
                        {
                            m_texture.SetPixel(x, y, c_color);
                        }
                    }
                    m_texture.Apply();

                    m_material.SetTexture("_DrawingTex", m_texture);
                    isEnabled = true;
                }
            }
        }

        public void PaintSplat(Vector2 textureCoord, Texture2D splashtexture)
        {
            if (isEnabled)
            {
                int x = (int)(textureCoord.x * textureWidth) - (splashtexture.width / 2);
                int y = (int)(textureCoord.y * textureHeight) - (splashtexture.height / 2);

                for (int i = 0; i < splashtexture.width; i++)
                {
                    for (int j = 0; j < splashtexture.height; j++)
                    {
                        int newX = x + i;
                        int newY = y + j;
                        Color existingColor = m_texture.GetPixel(newX, newY);
                        Color targetColor = splashtexture.GetPixel(i, j);
                        float alpha = targetColor.a;
                        targetColor = Color.Lerp(targetColor, splashTextureColor, alpha);
                        if (alpha > 0)
                        {
                            Color result = Color.Lerp(existingColor, targetColor, alpha);
                            result.a = existingColor.a + alpha;
                            m_texture.SetPixel(newX, newY, result);
                        }
                    }
                }

                m_texture.Apply();
            }
        }
        
    }
}
