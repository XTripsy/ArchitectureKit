using Namespace_InputLobby_Event;

public interface IPlayerSpawnService
{
    void ISpawnPlayer(ActionJoinLobbyState _event);
    void IDespawnPlayer(int id);
}