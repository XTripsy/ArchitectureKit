using PurrNet;
using PurrLobby;
using PurrNet.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterLobbyState : CustomStateNode
{
    [SerializeField] private NetworkIdentity _playerPrefab;
    [SerializeField] private List<Transform> _spawnPoints = new();

    private SyncList<NetworkIdentity> _spawnedPrefabs = new();
    private IEventBus _bus;
    private LobbyManager _lobbyManager;
    private IUIManager _ui;
    private int currentSpawnIndex = 0;

    public override void Init(IEventBus bus, LobbyManager lobbyManager)
    {
        _bus = bus;
        _lobbyManager = lobbyManager;
    }

    public override void Enter(bool asServer)
    {
        base.Enter(asServer);

        if (!asServer) return;
        _lobbyManager.OnAllReady.AddListener(CallOnAllReady);
        StartCoroutine(SpawnNewPlayer());
    }

    private void CallOnAllReady()
    {
        Debug.Log("<color=green>PlayerEnterLobbyState class : All Ready");
        machine.Next();
    }

    [ServerRpc(requireOwnership: false)]
    private void SpawnPlayers(PlayerID playerId)
    {
        Transform spawnPoint = _spawnPoints[currentSpawnIndex];
        NetworkIdentity newPlayer = Instantiate(_playerPrefab, spawnPoint.position, spawnPoint.rotation);
        newPlayer.GiveOwnership(playerId);
        _spawnedPrefabs.Add(newPlayer);
        currentSpawnIndex++;

        if (currentSpawnIndex >= _spawnPoints.Count)
        {
            currentSpawnIndex = 0;
        }
    }

    private IEnumerator SpawnNewPlayer()
    {
        while (true)
        {
            // Remove null item
            for (int i = _spawnedPrefabs.Count - 1; i >= 0; i--)
            {
                if (_spawnedPrefabs[i] == null)
                {
                    _spawnedPrefabs.RemoveAt(i);
                    currentSpawnIndex--;
                }
            }

            int currentSpawnedCount = _spawnedPrefabs.Count;
            int totalPlayers = networkManager.players.Count;
            int missing = totalPlayers - currentSpawnedCount;

            Debug.Log($"missing : {missing} ");

            for (int i = 0; i < missing; i++)
            {
                if ((currentSpawnedCount + i) > networkManager.players.Count) yield return null;
                PlayerID playersToSpawnFor = networkManager.players[currentSpawnedCount + i];
                SpawnPlayers(playersToSpawnFor);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public override void Exit(bool asServer)
    {
        base.Exit(asServer);

        _lobbyManager.OnAllReady.RemoveListener(CallOnAllReady);
        StopCoroutine(SpawnNewPlayer());
    }

}
