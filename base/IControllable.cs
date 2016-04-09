using System;

namespace Assets.october
{
    public interface IControllable : IViewpoint
    {
        IController controller { get; set; }

        event Action onCease;
    }
}
