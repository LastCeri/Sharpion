using System;
using System.Text;
using Newtonsoft.Json;
using WebSocketSharp;
using Sharpion.Configuration;
using Sharpion.Operations.SendTransaction;
using Sharpion.Platforms.Dotnet.Client.Handlers;

namespace Sharpion.Platforms.Dotnet.Client
{
    /// <summary>
    /// WebSocket client for the Sharpion protocol. Implements session state for wallet and balance.
    /// </summary>
    public sealed class SharpionClient : IClientSession
    {
        private WebSocket? _webSocket;
        private readonly SharpionOptions _options;

        public SharpionClient(SharpionOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(_options.WebSocketEndpoint))
                throw new ArgumentException("WebSocketEndpoint is required.", nameof(options));
        }

        public static int SocketClientId { get; set; }
        public string? UserWalletAddress { get; private set; }
        public string? UserBalanceOfEth { get; private set; }

        public void ConnectToServer()
        {
            var scheme = _options.UseSecureConnection ? "wss" : "ws";
            var url = $"{scheme}://{_options.WebSocketEndpoint}";
            _webSocket = new WebSocket(url);
            _webSocket.OnMessage += async (_, e) => await PacketHandler.HandleHandshakeAsync(e.Data);
            _webSocket.OnOpen += (_, __) => Console.WriteLine("WebSocket connection open.");
            _webSocket.OnClose += (_, __) => Console.WriteLine("WebSocket connection closed.");
            _webSocket.OnError += (_, e) => Console.WriteLine($"WebSocket error: {e.Message}");

            PacketHandler.GetCurrentSession = () => this;
            _webSocket.Connect();
        }

        public void DisconnectFromServer()
        {
            if (_webSocket?.ReadyState == WebSocketState.Open)
            {
                _webSocket.Close();
                Console.WriteLine("WebSocket connection closed manually.");
            }
        }

        public bool IsSocketAlive() => _webSocket?.IsAlive ?? false;

        public void ConnectWallet() =>
            SendJson(JsonConvert.SerializeObject(Packs.CreateLoginPack(false, false)));

        public void DisconnectWallet() =>
            SendJson(JsonConvert.SerializeObject(Packs.CreateDisconnectPack(SocketClientId, false, false)));

        public void BalanceOfWallet(string walletAddress) =>
            SendJson(JsonConvert.SerializeObject(Packs.CreateBalanceOfPack(SocketClientId, walletAddress)));

        public void SendTransaction(TransactionInteraction interaction) =>
            SendJson(JsonConvert.SerializeObject(Packs.CreateTransactionPack(SocketClientId, interaction)));

        public void SetUserWalletAddress(string address) => UserWalletAddress = address;
        public void SetUserBalanceOfEth(string balance) => UserBalanceOfEth = balance;

        private void SendJson(string json)
        {
            try
            {
                JsonConvert.DeserializeObject(json);
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine("Invalid JSON format.");
                Console.WriteLine(ex.Message);
                return;
            }

            try
            {
                if (_webSocket?.ReadyState == WebSocketState.Open)
                    _webSocket.Send(Encoding.UTF8.GetBytes(json));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending data: {ex.Message}");
            }
        }
    }
}
