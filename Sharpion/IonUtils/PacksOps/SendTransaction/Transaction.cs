namespace Sharpion.Operations.SendTransaction
{
    /// <summary>
    /// DTO for sending a blockchain transaction (contract call or transfer).
    /// </summary>
    public class TransactionInteraction
    {
        public string? ReceiptAddress { get; set; }
        public string? ContractAddress { get; set; }
        public string? Amount { get; set; }
    }
}
