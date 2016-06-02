using System;

namespace Assets.git
{
    public interface IControllable : IViewpoint
    {
        IController controller { get; set; }

        event Action onCease;
    }
}
