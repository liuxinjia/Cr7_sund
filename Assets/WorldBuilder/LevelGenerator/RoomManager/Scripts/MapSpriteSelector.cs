using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_Level
{
    public class MapSpriteSelector : MonoBehaviour
    {

        public Sprite spU, spD, spR, spL,
                spUD, spRL, spUR, spUL, spDR, spDL,
                spULD, spRUL, spDRU, spLDR, spUDRL;
        [HideInInspector]
        public bool up, down, left, right;
        public RoomType type;
        public Color originalColor, startColor, turnColor, endColor;
        Color mainColor;
        SpriteRenderer rend;
        void Start()
        {
            rend = GetComponent<SpriteRenderer>();
            mainColor = originalColor;
            PickSprite();
            PickColor();
        }
        void PickSprite()
        { //picks correct sprite based on the four door bools
            if (up)
            {
                if (down)
                {
                    if (right)
                    {
                        if (left)
                        {
                            rend.sprite = spUDRL;
                        }
                        else
                        {
                            rend.sprite = spDRU;
                        }
                    }
                    else if (left)
                    {
                        rend.sprite = spULD;
                    }
                    else
                    {
                        rend.sprite = spUD;
                    }
                }
                else
                {
                    if (right)
                    {
                        if (left)
                        {
                            rend.sprite = spRUL;
                        }
                        else
                        {
                            rend.sprite = spUR;
                        }
                    }
                    else if (left)
                    {
                        rend.sprite = spUL;
                    }
                    else
                    {
                        rend.sprite = spU;
                    }
                }
                return;
            }
            if (down)
            {
                if (right)
                {
                    if (left)
                    {
                        rend.sprite = spLDR;
                    }
                    else
                    {
                        rend.sprite = spDR;
                    }
                }
                else if (left)
                {
                    rend.sprite = spDL;
                }
                else
                {
                    rend.sprite = spD;
                }
                return;
            }
            if (right)
            {
                if (left)
                {
                    rend.sprite = spRL;
                }
                else
                {
                    rend.sprite = spR;
                }
            }
            else
            {
                rend.sprite = spL;
            }
        }

        void PickColor()
        { //changes color based on what type the room is
            if (type == RoomType.origin)
            {
                mainColor = originalColor;
            }
            else if (type == RoomType.start)
            {
                mainColor = startColor;
            }
            else if (type == RoomType.turn)
            {
                mainColor = turnColor;
            }
            else if (type == RoomType.end)
            {
                mainColor = endColor;
            }

            rend.color = mainColor;
        }
    }
}