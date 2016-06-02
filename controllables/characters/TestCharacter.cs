using Assets.git.controllables.abilities;
using System.Collections;
using UnityEngine;
using System;
using Assets.git.hudComponents;
using Assets.git.viewpoints;

namespace Assets.git.controllables.characters
{
    [RequireComponent(typeof(VelocityApplier))]
    [RequireComponent(typeof(Destructible))]
    class TestCharacter : Character
    {
        VelocityApplier velocityApplier;
        Destructible destructible;

        [SerializeField]
        float _life = 100;

        [SerializeField]
        HudEnabledViewpoint _viewpoint = null;
        protected override Viewpoint viewpoint { get { return _viewpoint; } }

        [SerializeField]
        TestHudComponent testHudComponentPrefab = null;
        TestHudComponent testHudComponent = null;

        [SerializeField]
        GameObject shootImpact = null;

        public override void Awake()
        {
            destructible = GetComponent<Destructible>();
            destructible.onDeath += () => { StartCoroutine(shrinkAndDie()); };

            velocityApplier = GetComponent<VelocityApplier>();

            _viewpoint.hud.AddHudComponent(createHudComponent());
        }

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
                    _controller.addPulseListener(Trigger.ShootLongstand, onPulseShootLongstand);

                    _controller.onMovVectorUpdate += velocityApplier.setMovVector;
                    _controller.onRotVectorUpdate += velocityApplier.setRotVector;
                }

                _controller = value;

                if (_controller != null)
                {
                    _controller.removePulseListener(Trigger.ShootLongstand, onPulseShootLongstand);

                    _controller.onMovVectorUpdate -= velocityApplier.setMovVector;
                    _controller.onRotVectorUpdate -= velocityApplier.setRotVector;
                }
            }
        }

        private void onPulseShootLongstand(float newValue, float difference)
        {
            if (difference > TriggerPack.deadZone)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position, transform.forward);

                if (Physics.Raycast(ray, out hit, 15))
                {
                    if (shootImpact != null)
                    {
                        Instantiate(shootImpact, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                    else
                    {
                        Debug.LogError("No shootImpact assigned");
                    }
                }
            }
        }

        IEnumerator shrinkAndDie()
        {
            while (transform.localScale.magnitude > 0.01)
            {
                transform.localScale = transform.localScale * 0.9f;
                yield return null;
            }

            yield return new WaitForSeconds(2);

            die();
        }

        private HudComponent createHudComponent()
        {
            if (testHudComponentPrefab != null)
            {
                testHudComponent = Instantiate(testHudComponentPrefab);
                return testHudComponent;
            }

            return null;
        }
    }
}
