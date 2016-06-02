using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.october.flints
{
    /// <summary>
    /// Flints are used to setup scenes to work, whether or not 
    /// they were loaded in a normal execution order.
    /// 
    /// To do this, flints check for the game manager singleton, 
    /// as any normal execution of the game will create one before 
    /// anything else.
    /// If no game manager is found, a new one will be created, 
    /// which will provoke it to be set as the singleton, 
    /// so the game can play normally.
    /// 
    /// After that, each flint will setup the scene in a different 
    /// way.
    /// </summary>
    public abstract class Flint : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManagerPrefab = null;

        public void Start()
        {
            if (gameManagerPrefab == null)
            {
                if (GameManager.instance != null)
                {
                    Debug.LogWarning(
                        "In order to be able to start the game using this flint, " +
                        "a game manager prefab is needed. Please assign one.");
                }
                if (GameManager.instance == null)
                {
                    Debug.LogError(
                        "A game manager prefab is necessary to start the game on this scene."
                        );
                }
            }

            if (GameManager.instance == null)
            {
                Instantiate(gameManagerPrefab);
                PreviewSetup();
            }
            else
            {
                NormalSetup();
            }
        }

        /// <summary>
        /// Do any initializations needed for the scene to run correctly
        /// after being loaded by other scene.
        /// </summary>
        protected abstract void NormalSetup();

        /// <summary>
        /// Do any initailizations needed for the scene to run correctly 
        /// when the game starts in this scene.
        /// </summary>
        protected abstract void PreviewSetup();
    }
}
