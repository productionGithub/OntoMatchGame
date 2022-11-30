#define TRACE_ON
using Zenject;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Navigation;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;

namespace StarterCore.Core.Scenes.LeaderBoard
{
    public class LeaderBoardManager : IInitializable
    {
        [Inject] MockNetService _networkService;
        [Inject] NavigationService _navigationService;
        [Inject] LeaderBoardController _leaderBoardController;

        [SerializeField] 
        RankingModelDown _rankings;

        public async void Initialize()
        {
            _leaderBoardController.ShowWaitingIcon();
            _rankings = await _networkService.GetRankings();
            _leaderBoardController.HideWaitingIcon();

            _leaderBoardController.Init();
            _leaderBoardController.OnBackButtonClicked += OnBack;

            Show();
        }

        public void Show()
        {
            //Debug.Log("FOR NEW mmm : " + _rankings.Languages[0].LanguageName);
            _leaderBoardController.Show(_rankings);
        }

        private void OnBack()
        {
            _navigationService.Push("MainMenuScene");
        }

    }
}
