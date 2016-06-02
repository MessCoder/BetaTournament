using System.Collections;
using UnityEngine;

namespace Assets.git.weapons
{
    [RequireComponent(typeof(LineRenderer))]
    public class TestRaygun : Weapon
    {
        private LineRenderer lineRenderer;

        [SerializeField]
        private Vector3 cannonRelativePosition = new Vector3(0, 0, 0.3f);
        [SerializeField]
        private float damage = 5;
        [SerializeField]
        private float rayDistance = 30;
        [SerializeField]
        private float frequency = 0.1f;

        [SerializeField]
        private int magazineCapacity= 50;
        [SerializeField]
        private float secondsToReloadMagazine = 25;

        private float _shootsInMagazine;
        private float shootsInMagazine
        {
            get
            {
                return _shootsInMagazine;
            }
            set
            {
                if (value > magazineCapacity)
                    value = magazineCapacity;

                if (value < 0)
                    value = 0;

                _shootsInMagazine = value;
            }
        }

        [SerializeField]
        private Kind _kind = Kind.LongstandLeft;
        public override Kind kind
        {
            get
            {
                return _kind;
            }
        }

        [SerializeField]
        private float _precision = 0.01f;
        private Coroutine runningShootingCoroutine;
        private bool shooting;

        public override float precision
        {
            get
            {
                return _precision;
            }
        }

        public override float ammoFactor
        {
            get
            {
                return shootsInMagazine / magazineCapacity;
            }
        }

        public override void Start()
        {
            base.Start();

            if (magazineCapacity < 0)
                Debug.LogError("Negative magazine capacity. This will cause errors");

            shootsInMagazine = magazineCapacity;

            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
            lineRenderer.SetVertexCount(2);
        }

        public void Update()
        {
            shootsInMagazine += magazineCapacity * (Time.deltaTime / secondsToReloadMagazine);
        }

        public override bool objectInSight(out Vector3 hitPosition)
        {
            RaycastHit hit = new RaycastHit();

            bool result = raycast(out hit);
            hitPosition = hit.point;

            return result;
        }

        private bool raycast(out RaycastHit hit)
        {
            return Physics.Raycast
                (
                transform.TransformVector(cannonRelativePosition), 
                transform.forward, 
                out hit, 
                rayDistance
                );
        }

        private Vector3 getWorldDeviatedDirection()
        {
            return transform.forward + UnityEngine.Random.onUnitSphere * precision;
        }

        public void shoot()
        {
            if (shootsInMagazine >= 1)
            {
                shootsInMagazine--;

                Vector3 shootOrigin = transform.TransformPoint(cannonRelativePosition);
                Vector3 shootEnd = new Vector3();
                Vector3 rayDirection = getWorldDeviatedDirection();

                RaycastHit hit;
                if (Physics.Raycast(shootOrigin, rayDirection, out hit, rayDistance))
                {
                    shootEnd = hit.point;

                    IDestructible destructible = hit.transform.GetComponent<IDestructible>();

                    if (destructible != null)
                    {
                        destructible.damage(damage, user);
                    }
                }
                else
                {
                    shootEnd = transform.TransformPoint(cannonRelativePosition);
                    shootEnd += rayDirection * rayDistance;
                }

                StartCoroutine(drawShoot(shootOrigin, shootEnd));
            }
        }

        private IEnumerator drawShoot(Vector3 origin, Vector3 end)
        {
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, end);

            lineRenderer.enabled = true;

            yield return new WaitForEndOfFrame();

            lineRenderer.enabled = false;
        }

        public override void setShooting(bool shooting)
        {
            if (shooting && runningShootingCoroutine == null)
            {
                this.shooting = true;
                runningShootingCoroutine = StartCoroutine(shootingCoroutine());
            }
            else
            {
                this.shooting = false;
            }
        }

        private IEnumerator shootingCoroutine()
        {
            while(shooting)
            {
                shoot();

                yield return new WaitForSeconds(frequency);
            }

            runningShootingCoroutine = null;
        }
    }
}
