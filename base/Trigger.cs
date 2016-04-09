using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.october
{
    [Serializable]
    public enum Trigger
    {
        Escape,
        Submit,

        XMov,
        YMov,
        ZMov,

        XRot,
        YRot,
        ZRot,

        ShootShortstand,
        ShootLongstand,

        Respawn
    }
}
