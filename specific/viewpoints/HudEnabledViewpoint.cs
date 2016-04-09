using UnityEngine;

namespace Assets.october.viewpoints
{
    public class HudEnabledViewpoint : Viewpoint
    {
        [SerializeField]
        private Hud _hud;
        public Hud hud
        {
            get
            {
                return _hud;
            }
            private set
            {
                _hud = value;
            }
        }

        public override void setViewrect(Rect viewRect)
        {
            base.setViewrect(viewRect);
            
            hud.setViewrect(viewRect);
        }

        public override void setRendering(bool rendering)
        {
            base.setRendering(rendering);

            hud.setRendering(rendering);
        }

        public override void setUIResponding(bool responding)
        {
            base.setUIResponding(responding);

            hud.setUIResponding(responding);
        }

        public override void Awake()
        {
            base.Awake();
        }
    }
}
