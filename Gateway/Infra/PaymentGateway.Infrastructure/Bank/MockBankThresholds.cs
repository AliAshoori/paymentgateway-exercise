namespace PaymentGateway.Infrastructure.Bank
{
    public class MockBankThresholds
    {
        public static readonly int ProcessableAmountThreshold = 1000;
        public static readonly int ServiceUnAvailableAmountThreshold = 1000000;
    }
}