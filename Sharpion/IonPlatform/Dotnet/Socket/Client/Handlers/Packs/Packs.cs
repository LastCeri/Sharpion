using Newtonsoft.Json;
using Sharpion.Operations.SendTransaction;
using static Sharpion.Platforms.Dotnet.Client.Handlers.ClientPacketType;

namespace Sharpion.Platforms.Dotnet.Client.Handlers
{
    /// <summary>
    /// Factory and DTOs for client-server packets.
    /// </summary>
    public static class Packs
    {
        private static readonly System.Random Random = new System.Random();

        public static LoginPacket CreateLoginPack(bool isLoggedIn, bool isAuthenticated)
        {
            var packet = new LoginPacket
            {
                PacketId = Random.Next(0, 99999999).ToString(),
                Type = (int)ClientPacketType.Login,
                IsLoggedIn = isLoggedIn,
                WorkOrder = null
            };
            return packet;
        }

        public static DisconnectPacket CreateDisconnectPack(int socketId, bool isDisconnected, bool isAuthenticated)
        {
            var packet = new DisconnectPacket
            {
                PacketId = Random.Next(0, 99999999).ToString(),
                SocketId = socketId,
                Type = (int)ClientPacketType.Disconnect,
                IsDisconnected = isDisconnected
            };
            return packet;
        }

        public static BalancePacket CreateBalanceOfPack(int socketId, string walletAddress)
        {
            var packet = new BalancePacket
            {
                PacketId = Random.Next(0, 99999999).ToString(),
                Type = (int)ClientPacketType.Balance,
                WalletAddress = walletAddress
            };
            return packet;
        }

        public static TransactionPacket CreateTransactionPack(int socketId, TransactionInteraction interaction)
        {
            var packet = new TransactionPacket
            {
                PacketId = Random.Next(0, 99999999).ToString(),
                ContractPack = interaction,
                Type = (int)ClientPacketType.Transaction
            };
            return packet;
        }

        /// <summary>Base packet structure for client-server protocol.</summary>
        public class Packet
        {
            [JsonProperty("packetid")]
            public string PacketId { get; set; }
            [JsonProperty("SocketID")]
            public int SocketId { get; set; }
            [JsonProperty("type")]
            public int Type { get; set; }
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("message")]
            public string Message { get; set; }
            [JsonProperty("wo")]
            public string? WorkOrder { get; set; }
        }

        public class ConnectionWalletPack : Packet
        {
            public string PublicWallet { get; set; }
            public string WalletBalance { get; set; }
        }

        public class BalancePacket : Packet
        {
            [JsonProperty("WalletAdress")]
            public string WalletAddress { get; set; }
            public string BalanceOfEth { get; set; }
        }

        public class LoginPacket : Packet
        {
            [JsonProperty("islog")]
            public bool IsLoggedIn { get; set; }
            [JsonProperty("auth")]
            public bool Auth { get; set; }
        }

        public class DisconnectPacket : Packet
        {
            [JsonProperty("disconnect")]
            public bool IsDisconnected { get; set; }
        }

        public class RegisterPacket : Packet
        {
            [JsonProperty("MacAdress")]
            public string MacAddress { get; set; }
        }

        public class TransactionPacket : Packet
        {
            public string TransactionStatusMessage { get; set; }
            public object ContractPack { get; set; }
        }
    }
}
