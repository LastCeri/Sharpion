namespace Sharpion.Platforms.Dotnet.Client.Handlers
{
    /// <summary>
    /// Packet type identifiers for client-server protocol.
    /// </summary>
    public enum ClientPacketType
    {
        Register = 1,
        Login = 2,
        Disconnect = 3,
        Command = 4,
        WalletPack = 5,
        Balance = 6,
        Transaction = 7
    }
}
