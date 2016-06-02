using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.git.controllables.mainMenus
{
    public class TestMenuControllable : Menu
    {
        [SerializeField]
        private Dropdown drpNumberOfPlayers = null;
        [SerializeField]
        private Dropdown drpMap = null;
        [SerializeField]
        private Button btnStartGame = null;
        [SerializeField]
        private Canvas canvas = null;
        
        private int _playersNum = 1;
        public int playersNum
        {
            get
            {
                return _playersNum;
            }
            set
            {
                if (value < 1)
                    value = 1;

                if (value > GameManager.users.Length)
                    value = GameManager.users.Length;

                _playersNum = value;
            }
        }

        private string[] mapNames;
        private string mapName;

        public override event Action onCease;

        private IController _controller;
        public override IController controller
        {
            get
            {
                return _controller;
            }

            set
            {
                // TO-DO Setup UI movement keys
                _controller = value;
            }
        }

        public void Start()
        {
            mapNames = GameManager.gameMapNames;
            mapName = mapNames[0];

            if (canvas != null)
                setupCanvas(canvas);

            if (drpMap != null)
                setupDrpMap(drpMap);

            if (drpNumberOfPlayers != null)
                setupDrpNumberOfPlayers(drpNumberOfPlayers);

            if (btnStartGame != null)
                setupStartGameButton(btnStartGame);
        }

        private void setupCanvas(Canvas canvas)
        {
            canvas.worldCamera = viewpoint.GetComponent<Camera>();
        }

        private void setupDrpMap(Dropdown drpMap)
        {
            if (drpMap == null)
            {
                Debug.LogError("Trying to setup null Dropdown as map selector");
                return;
            }

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

            foreach (string map in mapNames)
                options.Add(new Dropdown.OptionData(map));

            drpMap.options = options;

            drpMap.onValueChanged.AddListener(
                new UnityEngine.Events.UnityAction<int>(setMap)
                );
        }

        private void setupStartGameButton(Button btnStartGame)
        {
            if (btnStartGame == null)
            {
                Debug.LogError("Trying to setup null Button as Start game button");
                return;
            }

            btnStartGame.onClick.AddListener(
                new UnityEngine.Events.UnityAction(startGame)
                );
        }

        private void setupDrpNumberOfPlayers(Dropdown drpNumberOfPlayers)
        {
            if (drpNumberOfPlayers == null)
            {
                Debug.LogError("Trying to setup null Dropdown as number of players selector");
                return;
            }

            int maxPlayers = GameManager.users.Length;
            
            List<Dropdown.OptionData> dropdownOptions = new List<Dropdown.OptionData>();

            for (int i = 1; i <= maxPlayers; i++)
            {
                dropdownOptions.Add(new Dropdown.OptionData(i.ToString()));
            }

            drpNumberOfPlayers.options = dropdownOptions;

            drpNumberOfPlayers.onValueChanged.AddListener(
                new UnityEngine.Events.UnityAction<int>(setNumberOfPlayers)
                );
        }

        public void setNumberOfPlayers(int players)
        {
            players++;
            //Debug.Log("number of players set to " + players);
            this.playersNum = players;
        }

        private void setMap(int mapIndex)
        {
            mapName = mapNames[mapIndex];
        }

        public void startGame()
        {
            GameManager.startGame(mapName, playersNum);
        }
    }
}
