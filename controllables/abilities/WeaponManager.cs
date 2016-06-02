using Assets.git.hudComponents;
using System;
using System.Collections;

using UnityEngine;

namespace Assets.git.controllables.abilities
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField]
        private WeaponManagerHudComponent _hudComponentPrefab = null;
        private WeaponManagerHudComponent _hudComponent;
        public WeaponManagerHudComponent hudComponent
        {
            get
            {
                if (_hudComponent == null)
                {
                    if (_hudComponentPrefab == null)
                    {
                        Debug.LogError("Can't instantiate unassigned WeaponManagerHudComponent");
                        return null;
                    }
                    else
                    {
                        _hudComponent = Instantiate(_hudComponentPrefab);
                        _hudComponent.weaponManager = this;
                    }
                }

                return _hudComponent;
            }
        }

        private readonly Weapon[] _weapons = new Weapon[Weapon.KindSize];
        public Weapon[] weapons { get { return _weapons; } }
        
        private Weapon[] weaponsOnCatch = new Weapon[Weapon.KindSize];

        [SerializeField]
        private Vector3[] weaponLocalPositions = new Vector3[Weapon.KindSize];

        [SerializeField]
        private float maximumCatchingDistance = 7;

        [SerializeField]
        private float catchDuration = 0.3f;

        private Weapon _nearestWeapon = null;
        private IPlayer user;

        public Weapon nearestWeapon
        {
            get
            {
                return _nearestWeapon;
            }
            private set
            {
                if (_hudComponent != null)
                    if (_nearestWeapon != value)
                        _hudComponent.setCatchableWeapon(value);

                _nearestWeapon = value;
            }
        }

        public void Update()
        {
            updateNearestWeapon();
        }

        public void updateNearestWeapon()
        {
            Weapon result = null;
            float distanceToResult = float.PositiveInfinity;

            foreach(Weapon weapon in Weapon.getFreeWeapons())
            {
                float distanceToWeapon = (weapon.transform.position - transform.position).sqrMagnitude;

                if (distanceToWeapon <= distanceToResult && 
                    distanceToWeapon <= maximumCatchingDistance)
                {
                    result = weapon;
                    distanceToResult = distanceToWeapon;
                }
            }
            
            nearestWeapon = result;
        }

        public void setFocus(Vector3 focus)
        {
            if (_hudComponent != null)
                _hudComponent.setFocus(focus);

            foreach(Weapon weapon in weapons)
            {
                if (weapon != null)
                {
                    Vector3 direction = focus - weapon.transform.position;
                    weapon.transform.rotation = Quaternion.LookRotation(direction, transform.up);
                }
            }
        }

        public void catchNearestWeapon()
        {
            if (nearestWeapon != null)
            {
                catchWeapon(nearestWeapon);
            }
        }

        public void catchWeapon(Weapon weapon)
        {
            if (weapon == null)
            {
                Debug.LogError("Trying to catch null weapon");
                return;
            }

            if (weaponsOnCatch[(int)weapon.kind] == null)
            {
                ejectWeapon(weapon.kind);

                StartCoroutine(catchWeaponRoutine(weapon));
            }
        }

        private IEnumerator catchWeaponRoutine(Weapon weapon)
        {
            weaponsOnCatch[(int)weapon.kind] = weapon;

            weapon.occupy(user);

            float elapsedTime = 0;

            while(elapsedTime < catchDuration)
            {
                elapsedTime += Time.deltaTime;

                float factor = elapsedTime / catchDuration;

                weapon.transform.position = Vector3.Lerp(
                    weapon.transform.position,
                    transform.TransformPoint(weaponLocalPositions[(int)weapon.kind]),
                    factor
                    );

                weapon.transform.rotation = Quaternion.Lerp(
                    weapon.transform.rotation,
                    transform.rotation,
                    factor
                    );

                yield return null;
            }

            weaponsOnCatch[(int)weapon.kind] = null;
            setWeapon(weapon.kind, weapon);
        }

        private void groundWeapon(Weapon weapon)
        {
            int position = (int)weapon.kind;

            weapon.transform.parent = transform;
            weapon.transform.localPosition = weaponLocalPositions[position];
        }

        public void ejectWeapon(Weapon.Kind kind)
        {
            Weapon weapon = weapons[(int)kind];

            if (weapon != null)
            {
                weapon.setShooting(false);
                weapon.transform.parent = null;
                weapon.disoccupy();
            }

            setWeapon(kind, null);
        }

        public void ejectAll()
        {
            foreach (Weapon.Kind kind in Enum.GetValues(typeof(Weapon.Kind)))
            {
                ejectWeapon(kind);
            }
        }

        private void setWeapon(Weapon.Kind kind, Weapon weapon)
        {
            if (weapon == null)
                return;

            groundWeapon(weapon);

            weapons[(int)kind] = weapon;

            if (_hudComponent != null)
                _hudComponent.setWeapon(kind, weapon);
        }

        public void setWeaponShooting(Weapon.Kind kind, bool shooting)
        {
            Weapon weapon = weapons[(int)kind];
            
            if (weapon != null)
                weapon.setShooting(shooting);
        }

        public void setLongstandShooting(bool shooting)
        {
            setWeaponShooting(Weapon.Kind.LongstandLeft, shooting);
            setWeaponShooting(Weapon.Kind.LongstandRight, shooting);
        }

        public void setShortstandShooting(bool shooting)
        {
            setWeaponShooting(Weapon.Kind.ShortstandLeft, shooting);
            setWeaponShooting(Weapon.Kind.ShortstandRight, shooting);
        }

        public void setUser(IPlayer user)
        {
            this.user = user;
        }
    }
}
