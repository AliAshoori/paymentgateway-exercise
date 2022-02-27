namespace PaymentGateway.Domain.DTOs
{
    public class BankPaymentProcessResponse
    {
        public string Code { get; private init; }

        public string Message { get; private init; }

        public static BankPaymentProcessResponse SuccessPayload()
        {
            return new()
            {
                Code = "SUCCESSFUL",
                Message = "Payment has processed successfully"
            };
        }

        public static BankPaymentProcessResponse FailurePayload()
        {
            return new()
            {
                Code = "FAILED",
                Message = "Payment has not processed successfully"
            };
        }
    }
}