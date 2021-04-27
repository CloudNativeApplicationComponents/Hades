namespace Hades.Broker.Abstraction
{
    public class DeliveryResult
    {
        public DeliveryStatus Status { get; }
        public DeliveryResult(DeliveryStatus status)
        {
            Status = status;
        }

        public static DeliveryResult NotDelivered { get; } = new DeliveryResult(DeliveryStatus.NotDelivered);
        public static DeliveryResult PossiblyDelivered { get; } = new DeliveryResult(DeliveryStatus.PossiblyDelivered);
        public static DeliveryResult Delivered { get; } = new DeliveryResult(DeliveryStatus.Delivered);
    }
}
