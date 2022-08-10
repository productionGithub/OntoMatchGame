﻿using StarterCore.Core.Services.Network;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.GameState;

namespace StarterCore.Core
{
    [CreateAssetMenu(fileName = "GlobalInstaller", menuName = "StarterCore/GlobalInstaller", order = 0)]
    public class GlobalInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private NavigationSetup _navSetup;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle().NonLazy();

            Container.Bind<NetworkService>().AsSingle();
            Container.Bind<MockNetService>().AsSingle();

            Container.Bind<NavigationSetup>().FromInstance(_navSetup);
            Container.BindInterfacesAndSelfTo<NavigationService>().AsSingle().NonLazy();
        }
    }
}

