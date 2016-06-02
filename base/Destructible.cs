using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.git
{
    public class Destructible : MonoBehaviour, IDestructible
    {
        private static HashSet<Destructible> _destructibles = new HashSet<Destructible>();
        public static HashSet<Destructible> destructibles
        {
            get { return _destructibles; }
            private set { _destructibles = value; }
        }

        public event Action<float> onLifeChange;
        public event Action onDeath;
        bool visible;
        
        public void setVisible(bool visible)
        {
            this.visible = visible;

            if (visible)
                destructibles.Add(this);
            else
                destructibles.Remove(this);
        }

        [SerializeField]
        private float _life;
        public float life
        {
            get
            {
                return _life;
            }

            set
            {
                if (value < 0)
                    value = 0;

                _life = value;
                
                if (onLifeChange != null)
                    onLifeChange(value);

                if (_life == 0 && onDeath != null)
                {
                    if (visible)
                        setVisible(false);

                    onDeath();
                }
            }
        }

        public virtual float damage(float damage, IPlayer author)
        {
            life -= damage;

            Debug.Log("Destructible damaged by " + author + " in " + damage + " points ");

            return life;
        }

        public void OnDestroy()
        {
            if (visible)
                setVisible(false);
        }
    }
}
