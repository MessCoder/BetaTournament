using System;
using UnityEngine;

namespace Assets.git.controllables
{
    public abstract class DeadControllable : Controllable
    {
        public abstract event Action<Vector3, Quaternion> onRespawn;
    }
}
