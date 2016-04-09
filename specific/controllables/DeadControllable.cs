using System;
using UnityEngine;

namespace Assets.october.controllables
{
    public abstract class DeadControllable : Controllable
    {
        public abstract event Action<Vector3, Quaternion> onRespawn;
    }
}
