using Sharpion.Configuration;
using Sharpion.Operations.SendTransaction;
using Sharpion.Platforms.Dotnet.Client;

namespace Sharpion.Platforms.Dotnet
{
    /// <summary>
    /// .NET platform implementation using WebSocket client.
    /// </summary>
    public sealed class IonDotnet : IonPlatformBase
    {
        private readonly SharpionClient _client;

        public IonDotnet(SharpionOptions options)
        {
            _client = new SharpionClient(options ?? throw new System.ArgumentNullException(nameof(options)));
        }

        public override void ConnectToServer() => _client.ConnectToServer();
        public override void DisconnectFromServer() => _client.DisconnectFromServer();
        public override bool IsServerConnected() => _client.IsSocketAlive();
        public override void ConnectWallet() => _client.ConnectWallet();
        public override void DisconnectWallet() => _client.DisconnectWallet();
        public override void BalanceOf(string walletAddress) => _client.BalanceOfWallet(walletAddress);
        public override void SendTransaction(TransactionInteraction transaction) => _client.SendTransaction(transaction);
    }
}
