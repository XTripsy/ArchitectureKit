using UnityEngine;
using PurrNet;
using PurrNet.Modules;
using Namespace_Spawner.Player;
using System.Collections.Generic;

public sealed class NetworkPlayerSpawner : PurrMonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    private int _index;

    private IEventBus _bus;
    private ScenesModule _scenes;
    private ScenePlayersModule _scenePlayers;
    private List<(PlayerID, SceneID)> _pendingSpawns = new();


    public void Init(IEventBus b)
    {
        Debug.Log("<color=yellow> bus initialized");
        _bus = b;
        _bus.ISubscribe<RequestSpawnPlayer>(OnRequestSpawnPlayer);
    }

    public override void Subscribe(NetworkManager manager, bool asServer)
    {
        if (!asServer) return;

        if (manager.TryGetModule(out _scenePlayers, true))
            _scenePlayers.onPlayerLoadedScene += OnPlayerLoadedScene;

        manager.TryGetModule(out _scenes, true);
    }

    public override void Unsubscribe(NetworkManager manager, bool asServer)
    {
        if (!asServer) return;

        if (_scenePlayers != null)
            _scenePlayers.onPlayerLoadedScene -= OnPlayerLoadedScene;
    }

    private void OnPlayerLoadedScene(PlayerID player, SceneID scene, bool asServer)
    {
        if (!asServer) return;

        if (_bus == null)
        {
            _pendingSpawns.Add((player, scene));
            return;
        }

        _bus.IPublish(new RequestSpawnPlayer(player, scene));
    }


    private void OnRequestSpawnPlayer(RequestSpawnPlayer e)
    {
        if (!_scenes.TryGetSceneID(gameObject.scene, out var id)) return;
        if (id != e.scene) return;

        Vector3 pos;
        Quaternion rot;

        if (spawnPoints.Length > 0)
        {
            var sp = spawnPoints[_index];
            pos = sp.position;
            rot = sp.rotation;
            _index = (_index + 1) % spawnPoints.Length;
        }
        else
        {
            pos = playerPrefab.transform.position;
            rot = playerPrefab.transform.rotation;
        }

        var go = UnityProxy.Instantiate(playerPrefab, pos, rot, gameObject.scene);

        if (go.TryGetComponent(out NetworkIdentity net))
            net.GiveOwnership(e.player);

        _bus.IPublish(new PlayerSpawned(go, e.player));
    }
}
