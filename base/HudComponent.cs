using Assets.october.viewpoints;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.october
{
    public class HudComponent : MonoBehaviour
    {
        public void setLayer(int layer)
        {
            setLayerRecursive(transform, layer);
        }
        
        public virtual Hud hud { get; set; }

        private void setLayerRecursive(Transform transform, int layer)
        {
            transform.gameObject.layer = layer;

            foreach (Transform child in transform)
            {
                setLayerRecursive(child, layer);
            }
        }
    }
}