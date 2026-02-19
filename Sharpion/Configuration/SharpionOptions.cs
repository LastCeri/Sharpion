namespace Sharpion.Configuration
{
    /// <summary>
    /// Configuration options for Sharpion WebSocket client connection.
    /// </summary>
    public class SharpionOptions
    {
        /// <summary>
        /// WebSocket server endpoint (host and port), e.g. "localhost:8080" or "wss://example.com".
        /// Used to build the connection URL as ws://{WebSocketEndpoint} or wss:// when UseSecureConnection is true.
        /// </summary>
        public string WebSocketEndpoint { get; set; } = "localhost:8080";

        /// <summary>
        /// When true, uses wss:// instead of ws:// for the connection.
        /// </summary>
        public bool UseSecureConnection { get; set; }
    }
}
