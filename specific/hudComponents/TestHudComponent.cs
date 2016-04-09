using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.october.hudComponents
{
    public class TestHudComponent : HudComponent
    {
        [SerializeField]
        private Transform rotatingElement = null;
        [SerializeField]
        private float angularVelocity = 0.2f;

        public void Update()
        {
            if (rotatingElement != null)
                rotatingElement.Rotate(Vector3.down * angularVelocity);
        }
    }
}
