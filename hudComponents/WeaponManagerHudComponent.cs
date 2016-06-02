
using Assets.october.controllables.abilities;
using Assets.october.viewpoints;
using System;
using UnityEngine;

namespace Assets.october.hudComponents
{
    public partial class WeaponManagerHudComponent : HudComponent
    {
        [SerializeField]
        private Transform reticleEdgeTilePrefab = null;
        [SerializeField]
        private Transform reticleAmmoBarTilePrefab = null;
        [SerializeField]
        private CatchableWeaponSignal catchableSignalPrefab = null;
        private CatchableWeaponSignal catchableSignal;

        private Weapon catchableWeapon;
        
        private Transform reticleHolder;

        private Reticle[] reticles = new Reticle[Weapon.KindSize];
        private Weapon[] weapons = new Weapon[Weapon.KindSize];

        private WeaponManager _weaponManager;
        public WeaponManager weaponManager
        {
            get
            {
                return _weaponManager;
            }
            set
            {
                _weaponManager = value;

                if (value != null)
                {
                    enabled = true;

                    Weapon[] weapons = _weaponManager.weapons;
                    for (int i = 0; i < reticles.Length; i++)
                    {
                        setWeapon((Weapon.Kind)i, weapons[i]);
                    }
                }
                else
                {
                    enabled = false;

                    for (int i = 0; i < reticles.Length; i++)
                    {
                        setWeapon((Weapon.Kind)i, null);
                    }
                }
            }
        }
        
        public override Hud hud
        {
            get
            {
                return base.hud;
            }

            set
            {
                base.hud = value;

                if (catchableSignal != null)
                    catchableSignal.hud = value;
            }
        }

        public void setWeapon(Weapon.Kind kind, Weapon weapon)
        {
            weapons[(int)kind] = weapon;
            reticles[(int)kind].setWeapon(weapon);
        }

        public void Awake()
        {
            createReticleHolder();
            createReticles();
            createCatchableWeaponSignal();

            if (weaponManager == null)
                enabled = false;
            else
            {
                Debug.LogError("weaponManager set before WeaponManagerHudComponent Awake call");
            }
        }

        private void createCatchableWeaponSignal()
        {
            if (catchableSignalPrefab == null)
            {
                Debug.LogError("Cant instantiate null catchableSignalPrefab");
                return;
            }

            catchableSignal = Instantiate(catchableSignalPrefab);

            catchableSignal.transform.SetParent(transform, false);
            
            catchableSignal.addButtonListener(catchSignaledWeapon);

            if (hud != null)
                catchableSignal.hud = hud;
        }

        public void setCatchableWeapon(Weapon weapon)
        {
            catchableWeapon = weapon;
            catchableSignal.setWeapon(weapon);
        }

        public void catchSignaledWeapon()
        {
            weaponManager.catchWeapon(catchableWeapon);
        }

        public void Update()
        {
            for(int i = 0; i < Weapon.KindSize; i++)
            {
                if (weapons[i] != null)
                {
                    reticles[i].resizeAmmoBar(weapons[i].ammoFactor);
                }
            }
        }

        public void setFocus(Vector3 focus)
        {
            reticleHolder.position = focus;

            float distance = (focus - transform.position).magnitude;
            reticleHolder.localScale = new Vector3(1, 1, 1) * distance;
        }

        private void createReticles()
        {
            for (int i = 0; i < reticles.Length; i++)
            {
                reticles[i] = new Reticle(reticleHolder, reticleEdgeTilePrefab, reticleAmmoBarTilePrefab);
            }
        }

        private void createReticleHolder()
        {
            reticleHolder = new GameObject("Reticle holder").transform;
            reticleHolder.parent = transform;
            reticleHolder.localRotation = Quaternion.identity;
        }


        [Serializable]
        protected class Reticle
        {
            private static readonly float ammoBarEdgeSeparation = 0.02f;
            private static readonly float ammoEdgeDistanceToX0 = 0.17f;

            private Transform transform;

            private Transform[] squareEdges = new Transform[4];
            private Transform ammoBar;
            private Transform ammoEndEdge;
            private float fullAmmoBarLength;

            public Reticle(Transform holder, Transform edgeTilePrefab, Transform ammoTilePrefab)
            {
                createReticleGameobject(holder);
                createSquare(edgeTilePrefab);
                createAmmoTiles(edgeTilePrefab, ammoTilePrefab);

                transform.gameObject.SetActive(false);
            }

            public void setWeapon(Weapon weapon)
            {
                if (weapon != null)
                {
                    transform.gameObject.SetActive(true);

                    positionSquare(weapon.precision);
                    positionAmmoTiles(weapon.kind, weapon.precision);
                    resizeAmmoBar(weapon.ammoFactor);
                }
                else
                {
                    transform.gameObject.SetActive(false);
                }
            }

            public void resizeAmmoBar(float ammoFactor)
            {
                ammoBar.localScale = new Vector3(
                    fullAmmoBarLength * ammoFactor, 
                    ammoBar.localScale.y, 
                    ammoBar.localScale.z
                    );
            }

            private void createReticleGameobject(Transform holder)
            {
                transform = new GameObject("Reticle").transform;
                transform.parent = holder;
                transform.localPosition = new Vector3();
                transform.localRotation = Quaternion.identity;
            }

            private void createSquare(Transform edgeTilePrefab)
            {
                for (int i = 0; i < squareEdges.Length; i++)
                {
                    Transform edgeTile = Instantiate(edgeTilePrefab);
                    squareEdges[i] = edgeTile;

                    edgeTile.parent = transform;
                    edgeTile.localRotation = Quaternion.identity;

                    edgeTile.Rotate(new Vector3(0, 0, 90 * i), Space.Self);
                }
            }

            private void createAmmoTiles(Transform edgeTilePrefab, Transform ammoTilePrefab)
            {
                ammoEndEdge = Instantiate(edgeTilePrefab);
                ammoEndEdge.parent = transform;
                ammoEndEdge.localRotation = Quaternion.identity;

                ammoBar = Instantiate(ammoTilePrefab);
                ammoBar.parent = transform;
                ammoBar.localRotation = Quaternion.identity;
            }

            /// <summary>
            /// Positionates the edges of the square of this reticle in function of the symbolized precision
            /// </summary>
            /// <param name="precision">The precision here is specified in meters of deviation by meters of distance</param>
            private void positionSquare(float precision)
            {
                squareEdges[0].localPosition = new Vector3(1, 1, 0) * precision;
                squareEdges[1].localPosition = new Vector3(-1, 1, 0) * precision;
                squareEdges[2].localPosition = new Vector3(-1, -1, 0) * precision;
                squareEdges[3].localPosition = new Vector3(1, -1, 0) * precision;
            }

            /// <summary>
            /// Positionates the ammo tiles after the SquareEdges
            /// </summary>
            /// <param name="kind">The kind of gun represented</param>
            private void positionAmmoTiles(Weapon.Kind kind, float precision)
            {
                Vector3 origin = new Vector3();
                Vector3 expandingDirection = new Vector3();
                Vector3 ammoEndEdgeScale = new Vector3();
                Vector3 ammoBarScale = new Vector3();

                switch (kind)
                {
                    case Weapon.Kind.LongstandLeft:
                        origin = new Vector3(-1, -1, 0) * precision;
                        expandingDirection = Vector3.left;
                        ammoBarScale = Vector3.down;
                        ammoEndEdgeScale = Vector3.right + Vector3.down;
                        break;
                    case Weapon.Kind.LongstandRight:
                        origin = new Vector3(1, -1, 0) * precision;
                        expandingDirection = Vector3.right;
                        ammoBarScale = Vector3.down;
                        ammoEndEdgeScale = Vector3.left + Vector3.down;
                        break;
                    case Weapon.Kind.ShortstandLeft:
                        origin = new Vector3(-1, 1, 0) * precision;
                        expandingDirection = Vector3.left;
                        ammoBarScale = Vector3.up;
                        ammoEndEdgeScale = Vector3.right + Vector3.up;
                        break;
                    case Weapon.Kind.ShortstandRight:
                        origin = new Vector3(1, 1, 0) * precision;
                        expandingDirection = Vector3.right;
                        ammoBarScale = Vector3.up;
                        ammoEndEdgeScale = Vector3.left + Vector3.up;
                        break;
                }

                Vector3 ammoEdgePos = Vector3.up * origin.y + expandingDirection * ammoEdgeDistanceToX0;

                Vector3 ammoBarPos = origin + expandingDirection * ammoBarEdgeSeparation;
                Vector3 ammoBarEnd = ammoEdgePos - expandingDirection * ammoBarEdgeSeparation;

                fullAmmoBarLength = ammoBarEnd.x - ammoBarPos.x;

                ammoBar.localPosition = ammoBarPos;
                ammoBar.localScale = ammoBarScale;
                resizeAmmoBar(1);

                ammoEndEdge.localPosition = ammoEdgePos;
                ammoEndEdge.localScale = ammoEndEdgeScale;
            }
        }
    }
}
