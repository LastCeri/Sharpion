using System;
using Sharpion.Configuration;
using Sharpion.Operations.SendTransaction;
using Sharpion.Platforms;
using Sharpion.Platforms.Dotnet;

namespace Sharpion
{
    /// <summary>
    /// Entry point and facade for Sharpion: creates a platform-specific client and exposes connection and wallet operations.
    /// </summary>
    public class SharpionManager
    {
        private readonly IIonPlatform _platform;

        public SharpionManager(IIonPlatform platform)
        {
            _platform = platform ?? throw new ArgumentNullException(nameof(platform));
        }

        /// <summary>
        /// Creates a Sharpion manager for the given platform with the provided options.
        /// </summary>
        public static SharpionManager Create(PlatformName platformName, SharpionOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            IIonPlatform platform = platformName switch
            {
                PlatformName.Dotnet => new IonDotnet(options),
                _ => throw new NotSupportedException($"Platform '{platformName}' is not supported.")
            };
            return new SharpionManager(platform);
        }

        public virtual void ConnectToServer() => _platform.ConnectToServer();
        public virtual void DisconnectFromServer() => _platform.DisconnectFromServer();
        public virtual bool IsServerConnected() => _platform.IsServerConnected();
        public virtual void ConnectWallet() => _platform.ConnectWallet();
        public virtual void DisconnectWallet() => _platform.DisconnectWallet();
        public virtual void BalanceOf(string walletAddress) => _platform.BalanceOf(walletAddress);
        public virtual void SendTransaction(TransactionInteraction transaction) => _platform.SendTransaction(transaction);
    }
}
