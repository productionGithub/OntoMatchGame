﻿using Cysharp.Threading.Tasks;
using StarterCore.Core.Services.Network;
using StarterCore.Core.Services.Network.Models;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.GameState;

using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace StarterCore.Core.Scenes.Signup
{
    // IInitializable : Zenject Start() equivalent
    // ITickable : Zenject Update() equivalent
    // IDisposable : C# OnDestroy() equivalent

    public class SignupManager : IInitializable
    {   
        [Inject] private MockNetService _net;
        [Inject] private SignupController _controller;
        [Inject] private NavigationService _navService;
        [Inject] private GameStateManager _gameState;

        public void Initialize()
        {
            Debug.Log("SignupManager initialized!");
            _controller.Show();
            _controller.OnFormSubmittedEvent += SubmitClicked;
            _controller.OnBackEvent += BackEventClicked;
        }

        private async void SubmitClicked(SignupEventData signupData)
        {
            //Test load of game data
            TestLoadGame().Forget();


            //Check Email
            string email = signupData.Email;

            var result = await CheckEmail(email);

            if (result.DoesExist == false)//False = Did not find the email in DB
            {
                //Register user
                SignupModelUp netModel = new SignupModelUp
                {
                    Email = signupData.Email,
                    Password = signupData.Password,
                    Country = signupData.Country,
                    CountryCode = signupData.CountryCode,
                    Optin = signupData.Optin,
                    Lang = _gameState.Locale//Locale of the game when account created
                };

                RegisterUser(netModel).Forget();
            } else
            {
                _controller.HideAllAlerts();
                _controller.EmailAlreadyExists();
            }


        }

        private async UniTask<EmailValidationDown> CheckEmail(string email)
        {
            EmailValidationDown result =  await _net.CheckEmail(email);
            return result;
        }

        private async UniTaskVoid RegisterUser(SignupModelUp form)
        {
            SignupModelDown code = await _net.Register(form);
            if (code.Code != "")
            {
                _controller.HideAllAlerts();
                _controller.AccountCreated();
                //Debug.Log("Activation code is :" + code.Code);
            }
            else
            {
                _controller.HideAllAlerts();
                _controller.AccountCreatedFailed();
                Debug.LogError("Sorry. Coud not create account due to server issue.");
            }
        }

        private async UniTaskVoid TestLoadGame()
        {
            GameModelDown game = await _net.LoadGame();
            Debug.Log("Game loaded game title : " + game.Title);
            Debug.Log("Game loaded is : " + game.DomainTags[1]);
            Debug.Log("Game loaded is : " + game.OntologyTags[0]);
            Debug.Log("Game loaded is in language : " + game.LanguageTags[0]);
            Debug.Log("Game loaded first challenge title is : " + game.ChallengeList[0].Title);
        }

        private void BackEventClicked()
        {
            _navService.Pop();
        }
    }
}