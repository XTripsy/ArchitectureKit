using PurrNet;
using PurrLobby;
using PurrNet.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterLobbyState : CustomStateNode
{
    [SerializeField] private NetworkIdentity playerPrefab;
    [SerializeField] private List<Transform> spawnPoints = new();

    private IEventBus _bus;
    private LobbyManager _lobbyManager;

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
        ResetPlayerState();
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        int currentSpawnIndex = 0;
        Transform spawnPoint = spawnPoints[currentSpawnIndex];
        NetworkIdentity newPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        newPlayer.GiveOwnership(networkManager.localPlayer);
        currentSpawnIndex++;

        if (currentSpawnIndex >= spawnPoints.Count)
        {
            currentSpawnIndex = 0;
        }
    }

    private void CallOnAllReady()
    {
        machine.Next();
    }

    private void ResetPlayerState()
    {
        Debug.LogWarning("Reset Player State in Game");
    }

    public override void Exit(bool asServer)
    {
        base.Exit(asServer);

        _lobbyManager.OnAllReady.RemoveListener(CallOnAllReady);
    }

}