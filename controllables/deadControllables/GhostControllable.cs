using Assets.git.controllables.abilities;
using System;

using UnityEngine;

namespace Assets.git.controllables.deadControllables
{
    [RequireComponent(typeof(VelocityApplier))]
    public class GhostControllable : HudlessDeadControllable
    {
        VelocityApplier velocityApplier;

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
                    _controller.removePulseListener(Trigger.Respawn, onPulseRespawn);
                    _controller.onMovVectorUpdate += velocityApplier.setMovVector;
                    _controller.onRotVectorUpdate += velocityApplier.setRotVector;
                }

                _controller = value;

                if (_controller != null)
                {
                    _controller.addPulseListener(Trigger.Respawn, onPulseRespawn);
                    _controller.onMovVectorUpdate -= velocityApplier.setMovVector;
                    _controller.onRotVectorUpdate -= velocityApplier.setRotVector;
                }
            }
        }

        public override event Action onCease;
        public override event Action<Vector3, Quaternion> onRespawn;

        public override void Awake()
        {
            velocityApplier = GetComponent<VelocityApplier>();
        }

        private void onPulseRespawn(float newValue, float difference)
        {
            onRespawn(transform.position, transform.rotation);
        }
    }
}
