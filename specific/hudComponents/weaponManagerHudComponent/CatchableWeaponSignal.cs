using System;
using System.Collections;
using Assets.october.viewpoints;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.october.hudComponents
{
    [Serializable]
    internal class CatchableWeaponSignal : UIHudComponent // Even though it's not directly use by a hud, I want the inheritance and consistency
    {
        [SerializeField]
        private Button btnCatchWeapon = null;
        [SerializeField]
        private Text txtCatchWeapon = null;
        [SerializeField]
        private Text txtWeaponKind = null;

        [SerializeField]
        private string pickUpMessage = "Pick up - {0}";
        [SerializeField]
        private string weaponKindMessage = "> {0} {1}";

        private bool hiding;
        private bool appearing;

        private Weapon followedWeapon;

        public virtual void Awake()
        {
            hide();
        }

        public void Update()
        {
            if (followedWeapon == null)
                hide();
            else
            {
                transform.position = followedWeapon.transform.position;
            }
        }

        public void setWeapon(Weapon weapon)
        {
            if (weapon == null)
                hide();
            else
            {
                updateTxtCatchWeaponText(weapon.gameObject.name);
                updateTxtWeaponKind(weapon.kind);
                followedWeapon = weapon;

                appear();
                Update();
            }
        }

        private void updateTxtCatchWeaponText(string weaponName)
        {
            if (txtCatchWeapon == null)
            {
                Debug.Log("Can't change text of unexisting catch label");
                return;
            }

            txtCatchWeapon.text = string.Format(pickUpMessage, weaponName);
        }

        private void updateTxtWeaponKind(Weapon.Kind kind)
        {
            if (txtWeaponKind == null)
            {
                Debug.Log("Can't change text of unexisting WeaponKind label");
                return;
            }

            string stand = "[?]";
            string side = "?";

            switch (kind)
            {
                case Weapon.Kind.LongstandLeft:
                case Weapon.Kind.LongstandRight:
                    stand = "(L)";
                    break;
                case Weapon.Kind.ShortstandLeft:
                case Weapon.Kind.ShortstandRight:
                    stand = "[S] ";
                    break;
            }
            switch (kind)
            {
                case Weapon.Kind.LongstandLeft:
                case Weapon.Kind.ShortstandLeft:
                    side = "Left";
                    txtWeaponKind.alignment = TextAnchor.UpperLeft;
                    break;
                case Weapon.Kind.LongstandRight:
                case Weapon.Kind.ShortstandRight:
                    side = "Right";
                    txtWeaponKind.alignment = TextAnchor.UpperRight;
                    break;
            }

            txtWeaponKind.text = string.Format(weaponKindMessage, stand, side);
        }

        public void hide()
        {
            gameObject.SetActive(false);
        }

        private void appear()
        {
            gameObject.SetActive(true);
        }

        public void addButtonListener(UnityEngine.Events.UnityAction listener)
        {
            btnCatchWeapon.onClick.AddListener(listener);
        }
    }
}