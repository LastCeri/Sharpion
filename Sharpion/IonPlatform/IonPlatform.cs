
using Sharpion.IonUtils.PacksOps.SendTransaction;

namespace Sharpion.Manager.IonPlatforms
{
    public class IonPlatform
    {
        public enum PlatformName { Unity, Dotnet }
        public virtual void ConnectToServer() { }
        public virtual void DisconnectToServer() { }
        public virtual bool ServerConnectionStatus() => false;
        public virtual void ConnectWallet() { }
        public virtual void DisconnectWallet() { }
        public virtual void BalanceOf(string walletadress) { }
        public virtual void SendTransaction(TransactionInteraction transactionInteraction) { }

    }
}
