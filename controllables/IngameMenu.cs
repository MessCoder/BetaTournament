using System;

namespace Assets.git.controllables
{
    public abstract class IngameMenu : Controllable
    {
        public abstract void setUser(User user);
        public abstract event Action onReturnToGame;
    }
}
