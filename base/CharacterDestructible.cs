using Assets.git.controllables;
using UnityEngine;

namespace Assets.git
{
    [RequireComponent(typeof(Character))]
    public class CharDestructible : Destructible
    {
        Character character;
        IPlayer _user;
        public IPlayer user
        {
            get { return _user; }
            set { _user = value; }
        }

        public void Awake()
        {
            character = GetComponent<Character>();
            user = character.user;
        }

        public override float damage(float damage, IPlayer author)
        {
            life -= damage;

            IngameManager.recordDamage(author, user, damage, life <= 0);

            return life;
        }
    }
}
