using Alkahest.Core.Logging;
using Alkahest.Core.Net;
using Alkahest.Core.Net.Protocol.Packets;
using Alkahest.Core.Plugins;
using System.Linq;
using Alkahest.Core;
using System;
using Alkahest.Core.Game;

namespace Alkahest.Plugins.Lockons
{
    public sealed class LockonsPlugin : IPlugin
    {
        public string Name { get; } = "lockons";

        static readonly Log _log = new Log(typeof(LockonsPlugin));

        public void Start(GameProxy[] proxies)
        {
            foreach (var p in proxies.Select(x => x.Processor))
            {
                p.AddHandler<CCanLockOnTargetPacket>(HandleClientLockRequest);
                p.AddHandler<SCanLockOnTargetPacket>(HandleServerLockPacket);

            }
        }
       
        public void Stop(GameProxy[] proxies)
        {
            foreach (var p in proxies.Select(x => x.Processor))
            {
                p.RemoveHandler<CCanLockOnTargetPacket>(HandleClientLockRequest);
                p.RemoveHandler<SCanLockOnTargetPacket>(HandleServerLockPacket);
            }
        }

        private SkillId VowId = new SkillId(67228964);
        private bool HandleServerLockPacket(GameClient client, Direction direction, SCanLockOnTargetPacket packet)
        {
            if (packet.Skill == VowId) return true;
            return false;
        }

        private bool HandleClientLockRequest(GameClient client, Direction direction, CCanLockOnTargetPacket packet)
        {
            if (packet.Skill == VowId) return true;

            SCanLockOnTargetPacket editedpacket = new SCanLockOnTargetPacket
            {
                Target = packet.Target,
                Skill = packet.Skill,
                Unknown1 = packet.Unknown1,
                CanLockOn = true
            };
            client.SendToClient(editedpacket);
            return true;
        }
    }
}