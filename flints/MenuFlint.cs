using System;
using Assets.git.controllables;
using UnityEngine;

namespace Assets.git.flints
{
    /// <summary>
    /// Initializes a menu scene of the game
    /// </summary>
    public class MenuFlint : Flint
    {
        [SerializeField]
        Menu menuControllable = null;
        
        protected override void NormalSetup()
        {
            if (menuControllable == null)
                Debug.LogError("A menu controllable is necessary to initialize a menu.");
            else
            {
                menuControllable.controller = GameManager.users[0].getController();

                menuControllable.setViewrect(new Rect(0, 0, 1, 1));
                menuControllable.setRendering(true);
                menuControllable.setUIResponding(true);
            }
        }

        protected override void PreviewSetup()
        {
        }
    }
}
