using Microsoft.AspNetCore.Mvc;

namespace Helium.Controllers
{
    public class Peer
    {
        public string Name { get ; set; }
        public string Ip { get; set; }
        public long UdpPort { get; set; }
        public DateTime LastSyncTime { get; set; }

        public Peer DeepCopy()
        {
            return new Peer()
            {
                Name = Name,
                Ip = Ip,
                UdpPort = UdpPort,
                LastSyncTime = LastSyncTime
            };
        }
    }

    public class ClientPort
    {
        public long ReceivePort { get; set; }
    }

    /// <summary>
    /// Module for sim game peer communicatino.
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class SimServerController : ControllerBase
    {
        static List<Peer> peers = new List<Peer>();
        static Mutex peerLock = new Mutex(initiallyOwned: false);

        public SimServerController()
        {
        }

        [HttpPost]
        public IActionResult LobbySync([FromBody]ClientPort port)
        {
            string clientIp = null;
            IHeaderDictionary headers = this.HttpContext.Request.Headers;
            if (headers.Keys.Contains("X-Real-IP"))
            {
                clientIp = string.Join(", ", headers["X-Real-IP"]);
            }

            bool foundSelf = false;

            peerLock.WaitOne();

            // Update the current peer's sync time + scan for old peers
            foreach (Peer peer in peers)
            {
                if (peer.Ip == clientIp)
                {
                    foundSelf = true;
                    peer.LastSyncTime = DateTime.UtcNow;
                }
                else
                {
                    if ((DateTime.UtcNow - peer.LastSyncTime).TotalSeconds > 10)
                    {
                        peer.Name = string.Empty;
                    }
                }
            }

            // Clear any expired/old peers
            peers = peers.Where(p => !string.IsNullOrWhiteSpace(p.Name)).ToList();

            // Add the new peer if necessary
            if (!foundSelf)
            {
                Peer callerPeer = new Peer()
                {
                    Name = Guid.NewGuid().ToString(),
                    Ip = clientIp,
                    UdpPort = port.ReceivePort,
                    LastSyncTime = DateTime.UtcNow,
                };

                peers.Add(callerPeer);
            }

            // Copy + return the extracted peers
            List<Peer> copiedPeers = peers.Select(p => p.DeepCopy()).ToList();
            peerLock.ReleaseMutex();

            return this.Ok(new
            {
                peers = copiedPeers,
            });
        }
    }
}