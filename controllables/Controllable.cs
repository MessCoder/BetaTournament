using System;
using UnityEngine;

namespace Assets.git.controllables
{
    public abstract class Controllable : MonoBehaviour, IControllable
    {
        Rect viewRect = new Rect(0, 0, 1, 1);
        bool rendering = false;
        private bool uiResponding;

        public abstract event Action onCease;

        protected abstract Viewpoint viewpoint { get; }

        public abstract IController controller { get; set; }

        public virtual void Awake()
        {
            refreshViewpoint();
        }

        protected void refreshViewpoint()
        {
            if (viewpoint != null)
            {
                viewpoint.setViewrect(viewRect);
                viewpoint.setRendering(rendering);
                viewpoint.setUIResponding(uiResponding);
            }
        }

        public void setViewrect(Rect viewRect)
        {
            this.viewRect = viewRect;

            if (viewpoint == null)
                Debug.LogWarning("Setting viewRect of Controllable without viewpoint. ViewRect will be applied when a viewpoint is assigned");
            else
                viewpoint.setViewrect(viewRect);
        }

        public void setRendering(bool rendering)
        {
            this.rendering = rendering;

            if (viewpoint == null)
                Debug.LogWarning("Setting Controllable without viwepoint rendering = " + rendering + ". Changes will be applied when a viewport is assigned");
            else
                viewpoint.setRendering(rendering);
        }

        public virtual void setUIResponding(bool responding)
        {
            this.uiResponding = responding;

            if (viewpoint == null)
                Debug.LogWarning("Setting Controllable without viwepoint UIResponding = " + responding + ". Changes will be applied when a viewport is assigned");
            else
                viewpoint.setUIResponding(responding);
        }

        public virtual void OnDestroy()
        {
            controller = null;
        }
    }
}
