using Sharpion.Operations.SendTransaction;

namespace Sharpion.Platforms
{
    /// <summary>
    /// Supported platform targets for Sharpion.
    /// </summary>
    public enum PlatformName
    {
        Unity,
        Dotnet
    }

    /// <summary>
    /// Abstraction for platform-specific WebSocket and wallet operations.
    /// </summary>
    public interface IIonPlatform
    {
        void ConnectToServer();
        void DisconnectFromServer();
        bool IsServerConnected();
        void ConnectWallet();
        void DisconnectWallet();
        void BalanceOf(string walletAddress);
        void SendTransaction(TransactionInteraction transaction);
    }

    /// <summary>
    /// Base implementation of <see cref="IIonPlatform"/> with no-op defaults.
    /// </summary>
    public abstract class IonPlatformBase : IIonPlatform
    {
        public virtual void ConnectToServer() { }
        public virtual void DisconnectFromServer() { }
        public virtual bool IsServerConnected() => false;
        public virtual void ConnectWallet() { }
        public virtual void DisconnectWallet() { }
        public virtual void BalanceOf(string walletAddress) { }
        public virtual void SendTransaction(TransactionInteraction transaction) { }
    }
}
