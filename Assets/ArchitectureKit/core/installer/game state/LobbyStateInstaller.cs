using Namespace_ActionSpawnPlayer;
using Namespace_Input;
using Namespace_InputLobby;
using Namespace_InputLobby_Event;
using Namespace_StateLobby_Event;
using Namespace_UILobby;
using UnityEngine.InputSystem;
using PurrLobby;
using Namespace_Level;
using PurrNet;
using PurrNet.Transports;
using UnityEngine;
using UnityEditor;

namespace Namespace_StateLobby
{
    internal sealed class LobbyStateInstaller : IInstaller<IBootstrapContext>
    {
        public void Install(IBootstrapContext installer)
        {
            string name_state = "lobby_state";
            string name_mapping = "lobby_mapping";

            IEventBus bus = installer.IGetBus;
            IStateRegistry state = installer.IGetStateRegistry;
            IInputManager input = installer.IResolve<IInputManager>();
            IUIManager ui = installer.IResolve<IUIManager>();
            IGameLoopManager gameLoopManager = installer.IResolve<IGameLoopManager>();
            IObjectManager objectManager = installer.IResolve<IObjectManager>();
            LobbyManager lobbyManager = installer.IResolve<LobbyManager>();

            bus.ISubscribe<LevelLoad>(e =>
            {
                if (e.level != "gameplay_scene") return;

                CustomFriendList friendList = Object.FindFirstObjectByType<CustomFriendList>();
                friendList?.Init(bus, lobbyManager);

                ConnectionStarter connectionStarter = Object.FindFirstObjectByType<ConnectionStarter>();
                connectionStarter.Init(bus, ui);

                NetworkManager networkManager = Object.FindFirstObjectByType<NetworkManager>();
                networkManager.onClientConnectionState += (state) =>
                {
                    if (state != ConnectionState.Disconnected) return;
                    bus.IPublish(new RequestStateEnter("mainmenu_state"));
                    bus.IPublish(new LevelRequest("mainmenu_scene"));
                };
            });

            state.IRegister(name_state, new LobbyState(bus));

            _InstallInput(installer, bus, input, name_mapping);
            _InstallInputAction(bus, lobbyManager);
            _InstallUI(bus, ui, lobbyManager);
            _InstallObject(bus, gameLoopManager, objectManager);
        }

        private void _InstallInput(IBootstrapContext installer, IEventBus bus, IInputManager input, string name_mapping)
        {
            InputGroup group = installer.IGetGroup<InputGroup>();
            int index = input.IGetIndexCatalogInputAction(name_mapping, group.catalog);
            InputActionMap inputAction = group.action.FindActionMap(name_mapping, throwIfNotFound: false);
            InputCatalog.Mapping mapping = group.catalog.InputAction[index];
            IAction action = new ActionLobbyState(bus, inputAction, mapping);
            input.IRegisterActionInput(name_mapping, action);

            bus.ISubscribe<LobbyStateEnter>(_ => input.IActiveActionInput(name_mapping));
        }

        private void _InstallInputAction(IEventBus bus, LobbyManager lm)
        {
            InputActionLobbyState temp_input = new InputActionLobbyState(bus, lm);
            bus.ISubscribe<ActionReadyLobbyState>(_ => temp_input.ReadyLobby());
            bus.ISubscribe<ActionLeaveLobbyState>(_ => temp_input.LeaveLobby());
        }

        private void _InstallUI(IEventBus bus, IUIManager ui, LobbyManager lobbyManager)
        {
            UIActionLobbyState temp = new UIActionLobbyState(ui, bus, lobbyManager);
            bus.ISubscribe<LobbyStateEnter>(_ => temp.OnLobbyEnter());
            bus.ISubscribe<LobbyStateExit>(_ => temp.OnLobbyExit());
        }

        private void _InstallObject(IEventBus bus, IGameLoopManager gameLoopManager, IObjectManager objectManager)
        {
            ActionPlayer temp_player = new ActionPlayer(bus, gameLoopManager, objectManager);
        }
    }
}
