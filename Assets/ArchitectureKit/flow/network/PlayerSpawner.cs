using PurrNet;
using UnityEngine;

namespace Namespace_Spawner.Player
{
    internal readonly struct RequestSpawnPlayer : IEvent
    {
        public readonly PlayerID player;
        public readonly SceneID scene;
        public RequestSpawnPlayer(PlayerID p, SceneID s) { player = p; scene = s; }
    }

    internal readonly struct PlayerSpawned : IEvent
    {
        public readonly GameObject go;
        public readonly PlayerID player;

        public PlayerSpawned(GameObject go, PlayerID p)
        {
            this.go = go;
            this.player = p;
        }
    }
}