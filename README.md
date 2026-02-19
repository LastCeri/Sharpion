# Sharpion

**Sharpion** is a C# library for integrating web3 games that use WebSocket into game engines. It lets users connect to games from websites without an in-game browser and interact with blockchain transactions and smart contracts.

### Key Features

- **WebSocket communication**: Fast, low-latency client–server protocol.
- **Platform abstraction**: .NET implementation included; Unity and other platforms can be added via `IIonPlatform`.
- **Blockchain operations**: Wallet connect, balance queries, and transaction sending.
- **Configurable**: Connection endpoint and TLS via `SharpionOptions`.
- **Testable design**: Interfaces (`IIonPlatform`, `IClientSession`) and dependency-friendly APIs.

### Build

```bash
cd Sharpion
dotnet build
```

Target: .NET Standard 2.0. Dependencies: Newtonsoft.Json; WebSocket implementation is bundled under `Library/websocket-sharp`.

### Usage

```csharp
using Sharpion;
using Sharpion.Configuration;
using Sharpion.Operations.SendTransaction;
using Sharpion.Platforms;

var options = new SharpionOptions
{
    WebSocketEndpoint = "localhost:8080",
    UseSecureConnection = false
};

var manager = SharpionManager.Create(PlatformName.Dotnet, options);
manager.ConnectToServer();
manager.ConnectWallet();
manager.BalanceOf("0x...");
manager.SendTransaction(new TransactionInteraction
{
    ReceiptAddress = "0x...",
    ContractAddress = "0x...",
    Amount = "0.1"
});
manager.DisconnectWallet();
manager.DisconnectFromServer();
```

### Architecture

- **Sharpion**: Entry facade (`SharpionManager`) and configuration (`SharpionOptions`).
- **Sharpion.Platforms**: Platform abstraction (`IIonPlatform`, `IonPlatformBase`) and `PlatformName`.
- **Sharpion.Platforms.Dotnet**: .NET implementation using `SharpionClient` (WebSocket).
- **Sharpion.Platforms.Dotnet.Client.Handlers**: Packet types (`ClientPacketType`), DTOs (`Packs`), and message handling (`PacketHandler`).
- **Sharpion.Operations.SendTransaction**: DTO for transactions (`TransactionInteraction`).
- **Sharpion.Configuration**: Options for connection (`SharpionOptions`).
