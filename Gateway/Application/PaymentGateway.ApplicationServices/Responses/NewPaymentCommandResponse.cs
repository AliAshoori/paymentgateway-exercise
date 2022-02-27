namespace PaymentGateway.ApplicationServices.Responses
{
    public class NewPaymentCommandResponse : BaseApiResponse
    {
        public long PaymentId { get; set; }

        public long MerchantId { get; set; }

        public bool Succeeded { get; set; }
    }
}
