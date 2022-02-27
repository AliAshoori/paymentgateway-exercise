namespace PaymentGateway.Infrastructure.Bank
{
    public class MockBankConfigOptions
    {
        public const string MockBankSetting = "MockBankSetting";

        public int SlowProcessingWaitTime { get; set; }
    }
}