using System;

using WebSocketSharp;

using System.Text;
using Newtonsoft.Json;
using Sharpion.Dotnet.Handlers;
using Sharpion.IonUtils.PacksOps.SendTransaction;

namespace Sharpion.Dotnet
{
    public class Client
    {
        public static Client instance;
        public static WebSocket ws;

        public static int SocketClientID;
        public string UserWalletAddress;
        public string UserBalanceOfEth;

        public void ConnectToServer() {

            ws = new WebSocket($"ws://{Settings.Settings.webSocketipandport}");
            ws.OnMessage += async (sender, e) => await Handler.HandShake(e.Data);
            ws.OnOpen += (sender, e) => Console.WriteLine("WebSocket Connection Open.");
            ws.OnClose += (sender, e) => Console.WriteLine("WebSocket Connection Close.");
            ws.OnError += (sender, e) => Console.WriteLine($"WebSocket Connection Error: {e.Message}");
            ws.Connect();
        }
        public void DisconnectFromServer()
        {
            if (ws != null && ws.ReadyState == WebSocketState.Open)
            {
                ws.Close();
                Console.WriteLine("The WebSocket connection was closed manually.");
            }
        }
        public bool IsSocketAlive() => ws?.IsAlive ?? false;
        public void ConnectWallet() => instance.SendDataFromJson(JsonConvert.SerializeObject(Packs.CreateLoginPack(false, false)));
        public void DisconnectWallet() => instance.SendDataFromJson(JsonConvert.SerializeObject(Packs.CreateDisconnectPack(SocketClientID, false, false)));
        public void BalanceOfWallet(string WalletAdress) => instance.SendDataFromJson(JsonConvert.SerializeObject(Packs.CreateBalanceOfPack(SocketClientID, WalletAdress)));
        public void SendTransaction(TransactionInteraction transactionInteraction) => instance.SendDataFromJson(JsonConvert.SerializeObject(Packs.CreateTransactionPack(SocketClientID, transactionInteraction)));

        public void SendDataFromJson(string sendjson)
        {
            try
            {
                // This line checks if the given string is in valid JSON format.
                JsonConvert.DeserializeObject(sendjson);
            }
            catch (JsonReaderException jex)
            {
                // If an error occurs, it means the string is not in JSON format.
                Console.WriteLine("The provided string is not in a valid JSON format.");
                Console.WriteLine(jex.Message); // Prints the specific error message.
                return; // Exits the method.
            }
            try
            {
                if (ws.ReadyState == WebSocketState.Open)
                {
                    byte[] dataToSend = Encoding.UTF8.GetBytes(sendjson);
                    ws.Send(dataToSend);
                }
            }
            catch (Exception ex) // General exception for any WebSocket related error.
            {
                Console.WriteLine("Error occurred while sending data.");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
