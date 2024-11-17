using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnderworldExporter.Game
{
    public sealed class AndroidRootView : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _gamePathText;
        
        [SerializeField]
        private Button _startGameButton;

        [SerializeField] 
        private Button _setPathToGameButton;

        [SerializeField]
        private Button _exitGameButton;

        private AndroidRootViewController _rootViewController;
        
        private void Awake()
        {
            _rootViewController = new AndroidRootViewController(this);
            _exitGameButton.onClick.AddListener(_rootViewController.OnExitButtonClicked);
            _startGameButton.onClick.AddListener(_rootViewController.OnStartGameButtonClicked);
            _setPathToGameButton.onClick.AddListener(_rootViewController.OnSetGamePathButtonClicked);
            UpdateGamePath(_rootViewController.BasePath);
        }

        public void UpdateGamePath(string gamePath) => _gamePathText.text = gamePath;
    }
}