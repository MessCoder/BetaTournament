using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.git
{
    public abstract class Weapon : MonoBehaviour
    {
        public enum Kind
        {
            LongstandLeft,
            LongstandRight,
            ShortstandLeft,
            ShortstandRight
        }

        public static int KindSize = Enum.GetValues(typeof(Kind)).Length;

        private static HashSet<Weapon> freeWeapons = new HashSet<Weapon>();

        new private Rigidbody rigidbody = null;
        private bool hasRigidbody = false;

        protected IPlayer user = null;

        public abstract float ammoFactor { get; }

        public static Weapon[] getFreeWeapons()
        {
            return freeWeapons.ToArray();
        }

        public virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();

            if (rigidbody != null)
                hasRigidbody = true;
        }

        public virtual void Start()
        {
            freeWeapons.Add(this);
        }

        public virtual void OnDestroy()
        {
            freeWeapons.Remove(this);
        }

        public virtual bool occupy(IPlayer user)
        {
            if (user == null)
                throw new Exception("Can't occupy weapon with no user, use disoccupy to drop weapons");
            
            if (this.user != null)
                return false;

            this.user = user;

            freeWeapons.Remove(this);

            transform.SetParent(null);

            if (rigidbody != null)
                Destroy(rigidbody);

            return true;
        }

        public virtual void disoccupy()
        {
            freeWeapons.Add(this);

            if (hasRigidbody)
                rigidbody = gameObject.AddComponent<Rigidbody>();
        }

        public abstract Kind kind { get; }

        public abstract bool objectInSight(out Vector3 hitPosition);

        /// <summary>
        /// Precision of the weapon in meters of deviation per meters of distance
        /// </summary>
        public abstract float precision { get; }

        public abstract void setShooting(bool active);
    }
}
