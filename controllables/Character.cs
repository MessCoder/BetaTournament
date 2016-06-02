using System;

namespace Assets.git.controllables
{
    public abstract class Character : Controllable
    {
        private IPlayer _user;
        public virtual IPlayer user
        {
            get { return _user; }
            set { _user = value; }
        }

        public override event Action onCease;

        protected virtual void die()
        {
            onCease();
            Destroy(viewpoint.gameObject);
            Destroy(gameObject);
        }
    }
}
