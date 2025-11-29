namespace Namespace_UILobby
{
    internal sealed class UIActionLobbyState
    {
        private readonly IUIManager _ui;

        public UIActionLobbyState(IUIManager ui)
        {
            _ui = ui;
        }

        public void OnLobbyEnter()
        {
            _ui.IShow("ui-lobby");
        }

        public void OnLobbyExit()
        {
            _ui.IHide("ui-lobby");
        }
    }
}
