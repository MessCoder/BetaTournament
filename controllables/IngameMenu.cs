using System;

namespace Assets.october.controllables
{
    public abstract class IngameMenu : Controllable
    {
        public abstract void setUser(User user);
        public abstract event Action onReturnToGame;
    }
}
