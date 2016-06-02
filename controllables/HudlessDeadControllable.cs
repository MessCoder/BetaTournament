using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.october.controllables
{
    public abstract class HudlessDeadControllable : DeadControllable
    {
        [SerializeField]
        private Viewpoint _viewpoint;

        protected override Viewpoint viewpoint
        {
            get
            {
                return _viewpoint;
            }
        }
    }
}
