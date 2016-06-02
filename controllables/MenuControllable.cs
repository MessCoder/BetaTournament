using UnityEngine;

namespace Assets.git.controllables
{
    public abstract class Menu : Controllable
    {
        [SerializeField]
        private Viewpoint _viewpoint;
            
        protected override Viewpoint viewpoint { get { return _viewpoint; } }
    }
}