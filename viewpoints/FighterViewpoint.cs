using UnityEngine;

namespace Assets.git.viewpoints
{
    [RequireComponent(typeof(Scanner))]
    public class FighterViewpoint : HudEnabledViewpoint
    {
        private Scanner _radar;
        public Scanner radar { get { return _radar; } }

        [SerializeField]
        private float sightDeadDistance = 70;

        public override void Awake()
        {
            base.Awake();

            _radar = GetComponent<Scanner>();
        }

        public Vector3 getFocus()
        {
            var target = radar.getTargetInRange();

            if (target == null)
            {
                Vector3 origin = transform.position;
                Vector3 direction = transform.forward;
                Vector3 destiny = origin + direction * sightDeadDistance;

                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(origin, direction, out hit, sightDeadDistance))
                {
                    destiny = hit.point;
                }

                return destiny;
            }
            else
            {
                return target.transform.position;
            }
        }
    }
}
