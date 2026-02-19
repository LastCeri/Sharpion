using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Sharpion.Platforms.Dotnet.Client.Handlers.ClientPacketType;
using static Sharpion.Platforms.Dotnet.Client.Handlers.Packs;

namespace Sharpion.Platforms.Dotnet.Client.Handlers
{
    /// <summary>
    /// Handles incoming WebSocket messages and dispatches to packet-specific handlers.
    /// </summary>
    public static class PacketHandler
    {
        /// <summary>
        /// Callback used to resolve the current client instance for handler updates (e.g. wallet address).
        /// Set by the client when it registers itself.
        /// </summary>
        internal static Func<IClientSession> GetCurrentSession { get; set; }

        public static async Task HandleHandshakeAsync(string messageJson)
        {
            try
            {
                var packet = JsonConvert.DeserializeObject<Packet>(messageJson);
                if (packet == null) return;

                switch (packet.Type)
                {
                    case (int)ClientPacketType.Login:
                        await HandleLoginPacketAsync(JsonConvert.DeserializeObject<LoginPacket>(messageJson));
                        break;
                    case (int)ClientPacketType.WalletPack:
                        await HandleConnectionPacketAsync(JsonConvert.DeserializeObject<ConnectionWalletPack>(messageJson));
                        break;
                    case (int)ClientPacketType.Balance:
                        await HandleBalancePacketAsync(JsonConvert.DeserializeObject<BalancePacket>(messageJson));
                        break;
                    case (int)ClientPacketType.Transaction:
                        await HandleTransactionPacketAsync(JsonConvert.DeserializeObject<TransactionPacket>(messageJson));
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Handshake: {ex.Message}");
            }

            await Task.CompletedTask;
        }

        private static async Task HandleLoginPacketAsync(LoginPacket loginPacket)
        {
            if (loginPacket == null) return;
            try
            {
                if (loginPacket.IsLoggedIn)
                    Console.WriteLine($"Login Data From Server: {loginPacket.Message}");
                else if (loginPacket.Auth)
                    Console.WriteLine($"Login Data From Server (User Connected Wallet): {loginPacket.Message}");
                else
                    Console.WriteLine($"Login Data From Server: {loginPacket.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing login packet: {ex.Message}");
            }
            await Task.CompletedTask;
        }

        private static async Task HandleConnectionPacketAsync(ConnectionWalletPack connectionPacket)
        {
            if (connectionPacket == null) return;
            try
            {
                if (!string.IsNullOrEmpty(connectionPacket.PublicWallet))
                {
                    Console.WriteLine($"Player Wallet Address: {connectionPacket.PublicWallet}");
                    var session = GetCurrentSession?.Invoke();
                    session?.SetUserWalletAddress(connectionPacket.PublicWallet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing connection packet: {ex.Message}");
            }
            await Task.CompletedTask;
        }

        private static async Task HandleBalancePacketAsync(BalancePacket balancePacket)
        {
            if (balancePacket == null) return;
            try
            {
                if (!string.IsNullOrEmpty(balancePacket.BalanceOfEth))
                {
                    Console.WriteLine($"Player's Ethereum Balance: {balancePacket.BalanceOfEth}");
                    var session = GetCurrentSession?.Invoke();
                    session?.SetUserBalanceOfEth(balancePacket.BalanceOfEth);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing balance packet: {ex.Message}");
            }
            await Task.CompletedTask;
        }

        private static async Task HandleTransactionPacketAsync(TransactionPacket transactionPacket)
        {
            if (transactionPacket == null) return;
            try
            {
                if (!string.IsNullOrEmpty(transactionPacket.TransactionStatusMessage))
                    Console.WriteLine($"Player Transaction Status: {transactionPacket.TransactionStatusMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing transaction packet: {ex.Message}");
            }
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Session state updated by packet handlers (wallet address, balance).
    /// </summary>
    public interface IClientSession
    {
        void SetUserWalletAddress(string address);
        void SetUserBalanceOfEth(string balance);
    }
}
