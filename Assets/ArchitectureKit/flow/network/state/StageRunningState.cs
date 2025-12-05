using System.Collections.Generic;
using PurrLobby;
using PurrNet;
using PurrNet.StateMachine;
using UnityEngine;

public class StageRunningState : CustomStateNode
{
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
        Debug.Log("<color=yellow> ENTER StageRunningState");
    }

    public override void Exit(bool asServer)
    {
        base.Exit(asServer);
        if (!asServer) return;
    }
}