using Namespace_Player;
using UnityEngine.InputSystem;

public interface IPlayerManager
{
    void IAddPlayer(PlayerComponents playerComponents);
    void IRemovePlayer(int id);
    void IRemoveAllPlayer();
    bool IIsCanSpawn(InputDevice device);
}