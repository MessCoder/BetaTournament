using Assets.october.viewpoints;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.october.controllables.ingameMenus
{
    public class TestIngameMenu : IngameMenu
    {
        private IController _controller;

        [SerializeField]
        private Hud _viewpoint = null;
        [SerializeField]
        private Button btnExit = null;
        [SerializeField]
        private Button btnResume = null;
        
        public override event Action onCease;
        public override event Action onReturnToGame;

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
                    _controller.removePulseListener(Trigger.Escape, onEscapePulse);
                }

                _controller = value;

                if (_controller != null)
                {
                    _controller.addPulseListener(Trigger.Escape, onEscapePulse);
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void onEscapePulse(float newValue, float difference)
        {
            if (difference > 0)
                resumeGame();
        }

        public override void Awake()
        {
            base.Awake();
            
            setupBtnExit(btnExit);
            setupBtnResume(btnResume);
        }

        private void setupBtnResume(Button btnResume)
        {
            if (btnResume == null)
                Debug.LogError("Can't setup null button as Resume game button");
            else
                btnResume.onClick.AddListener(resumeGame);
        }

        private void setupBtnExit(Button btnExit)
        {
            if (btnExit == null)
                Debug.LogError("Can't setup null button as Exit game button");
            else
                btnExit.onClick.AddListener(exitGame);
        }

        private void doNothing()
        {
            Debug.Log("Nothing");
        }

        private void resumeGame()
        {
            if (onReturnToGame != null)
                onReturnToGame();
            else
                Debug.LogError("onReturnToGame event is null");
        }

        private void exitGame()
        {
            IngameManager.exitToMenu();
        }
        
        protected override Viewpoint viewpoint
        {
            get
            {
                return _viewpoint;
            }
        }

        public override void setUser(User user)
        {
            // Wooo!
        }
    }
}