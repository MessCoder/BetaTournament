using Assets.october.controllables.abilities;
using System.Linq;
using UnityEngine;
using System.Collections;
using System;
using Assets.october.viewpoints;

namespace Assets.october.controllables.characters
{
    [RequireComponent(typeof(CharDestructible))]
    [RequireComponent(typeof(Levitator))]
    [RequireComponent(typeof(WeaponManager))]
    class BetaFighter : Character
    {
        CharDestructible destructible;
        Levitator levitator;
        WeaponManager weaponManager;

        [SerializeField]
        FighterViewpoint _viewpoint;
        protected override Viewpoint viewpoint { get { return _viewpoint; } }

        private IController _controller;

        public override IController controller
        {
            get
            {
                return _controller;
            }

            set
            {
                if (_controller != null)
                {
                    _controller.removeUpdateListener(Trigger.ShootLongstand, updateShootLongstand);
                    _controller.removeUpdateListener(Trigger.ShootShortstand, updateShootShorstand);
                                
                    _controller.removePulseListener(Trigger.Submit, onPulseCatchWeapon);

                    _controller.onMovVectorUpdate -= onMovVectorUpdate;
                    _controller.onRotVectorUpdate -= onRotVectorUpdate;

                    if (value == null)
                    {
                        updateShootLongstand(0, 0);
                        updateShootShorstand(0, 0);

                        onMovVectorUpdate(new Vector3());
                        onRotVectorUpdate(new Vector3());
                    }
                }

                _controller = value;

                if (_controller != null)
                {
                    _controller.addUpdateListener(Trigger.ShootLongstand, updateShootLongstand);
                    _controller.addUpdateListener(Trigger.ShootShortstand, updateShootShorstand);
                                
                    _controller.addPulseListener(Trigger.Submit, onPulseCatchWeapon);

                    _controller.onMovVectorUpdate += onMovVectorUpdate;
                    _controller.onRotVectorUpdate += onRotVectorUpdate;
                }
            }
        }
        
        private void onRotVectorUpdate(Vector3 newValue)
        {
            levitator.setRotVector(newValue);
        }

        private void onMovVectorUpdate(Vector3 newValue)
        {
            levitator.setMovVector(newValue);
        }

        private void updateShootShorstand(float newValue, float difference)
        {
            if (weaponManager != null)
                weaponManager.setShortstandShooting(newValue > TriggerPack.deadZone);
        }

        private void updateShootLongstand(float newValue, float difference)
        {
            if (weaponManager != null)
                weaponManager.setLongstandShooting(newValue > TriggerPack.deadZone);
        }

        private void onPulseCatchWeapon(float newValue, float difference)
        {
            if (difference > 0 && weaponManager != null)
                weaponManager.catchNearestWeapon();
        }

        private IEnumerator dieRoutine()
        {
            float size = 1;
            float dyingTime = 2;
            float deadTime = Time.time + dyingTime;

            weaponManager.ejectAll();
            weaponManager.enabled = false;
            levitator.enabled = false;

            while (Time.time < deadTime)
            {
                size -= Time.deltaTime / dyingTime;
                transform.localScale = new Vector3(1, 1, 1) * size;

                yield return null;
            }

            yield return new WaitForSeconds(2);

            die();
        }

        public override void Awake()
        {
            base.Awake();
            destructible = GetComponent<CharDestructible>();
            destructible.onDeath += () => StartCoroutine(dieRoutine());
            destructible.user = user;
            destructible.setVisible(true);

            levitator = GetComponent<Levitator>();

            weaponManager = GetComponent<WeaponManager>();
            weaponManager.setUser(user);
            
            _viewpoint.hud.AddHudComponent(weaponManager.hudComponent);

            _viewpoint.radar.addToWhitelist(destructible);
        }

        public void Update()
        {
            Vector3 focus = getFocus();
            weaponManager.setFocus(focus);
        }

        private Vector3 getFocus()
        {
            return _viewpoint.getFocus();
        }

        public override IPlayer user
        {
            set
            {
                base.user = value;

                weaponManager.setUser(value);
                destructible.user = value;
            }
        }
    }
}
