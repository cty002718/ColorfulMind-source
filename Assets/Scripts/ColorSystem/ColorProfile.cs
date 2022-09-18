using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorSystem
{
    public enum ColorType
    {
        Black,
        White,
        Blue,
        Green,
        Yellow,
        Orange,
        Red,
        Null
    }


    public class ColorProfile : MonoBehaviour
    {
        static public ColorProfile instance;

        [SerializeField]
        public Material Shader;

        private void Awake()
        {
            instance = this;
        }

        [SerializeField]
        private bool blue = false;
        [SerializeField]
        private bool green = false;
        [SerializeField]
        private bool yellow = false;
        [SerializeField]
        private bool orange = false;
        [SerializeField]
        private bool red = false;
        public bool IsGreenUnlocked { get { return green; } set { green = value; } }
        public bool IsBlueUnlocked { get { return blue; } set { blue = value; } }
        public bool IsYellowUnlocked { get { return yellow; } set { yellow = value; } }
        public bool IsOrangeUnlocked { get { return orange; } set { orange = value; } }
        public bool IsRedUnlocked { get { return red; } set { red = value; } }
    }

    [Serializable]
    public struct ColorInfo
    {
        public Color c;
        public float hue;
        public float saturation;
        public float brightness;
        public float contrast;

        public ColorInfo(Color _c, float _hue, float _saturation, float _brightness, float _contrast)
        {
            c = _c;
            hue = _hue;
            saturation = _saturation;
            brightness = _brightness;
            contrast = _contrast;
        }
    }
}