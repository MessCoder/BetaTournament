using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.git.viewpoints;
using UnityEngine;

namespace Assets.git.hudComponents
{
    public class UIHudComponent : HudComponent
    {
        [SerializeField]
        protected Canvas canvas;

        public override Hud hud
        {
            get
            {
                return base.hud;
            }

            set
            {
                if (hud != null)
                    hud.onUIRespondingChange -= onUIRespondingChange;

                base.hud = value;

                if (hud != null)
                    hud.onUIRespondingChange += onUIRespondingChange;
            }
        }

        // Theoretically, this should be called only if hud is set
        protected virtual void onUIRespondingChange(bool responding)
        {
            if (hud == null)
                Debug.LogError("onUIRespondingChange method called without a hud assigned to the object. "
                    + "This method should be called only by a hud, and such hud should be referenced by this object");

            if (responding)
                canvas.worldCamera = hud.camera;
            else
                canvas.worldCamera = null;
        }

        public void OnDestroy()
        {
            hud = null;
        }
    }
}
