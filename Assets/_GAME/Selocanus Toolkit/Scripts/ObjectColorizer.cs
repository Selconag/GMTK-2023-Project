using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Selocanus
{
    public class ObjectColorizer : MonoBehaviour
    {
        private Renderer Renderer;
        public ColorList ColorList;
        public TrailRenderer TrailRenderer;
        private void Awake()
        {
            Renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            ColorizeMe();
        }

        private void ColorizeMe()
        {
            Renderer.material.color = ColorList.ColorObjects[Random.Range(0, ColorList.ColorObjects.Count - 1)].Color;
            if (TrailRenderer != null)
            {
                TrailRenderer.startColor = Renderer.material.color;
                TrailRenderer.endColor = Renderer.material.color;
            }
        }

    }
}
