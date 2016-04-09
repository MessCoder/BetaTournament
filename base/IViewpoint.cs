using System;
using UnityEngine;

namespace Assets.october
{
    public interface IViewpoint
    {
        void setViewrect(Rect viewRect);
        void setRendering(bool rendering);
        void setUIResponding(bool responding);
    }
}
