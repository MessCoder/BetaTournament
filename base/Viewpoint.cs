using System;

using UnityEngine;

namespace Assets.git
{
    [RequireComponent(typeof(Camera))]
    public class Viewpoint : MonoBehaviour, IViewpoint
    {
        new public Camera camera { get; protected set; }

        public event Action<bool> onUIRespondingChange;

        public virtual void Awake()
        {
            Debug.Log("Viewpoint awoken");
            camera = GetComponent<Camera>();
        }

        public virtual void setRendering(bool rendering)
        {
            camera.enabled = rendering;
            enabled = rendering;
        }

        public virtual void setViewrect(Rect viewRect)
        {
            camera.rect = viewRect;
        }

        public virtual void destroy()
        {
            Destroy(gameObject);
        }

        public virtual void setUIResponding(bool responding)
        {
            if (onUIRespondingChange != null)
                onUIRespondingChange(responding);
        }
    }
}
