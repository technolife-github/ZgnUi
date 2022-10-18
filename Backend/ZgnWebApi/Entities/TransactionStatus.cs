namespace ZgnWebApi.Entities
{
    public class TransactionStatus
    {
        private TransactionStatus(string value) { Value = value; }
        public string Value { get; private set; }
        public static TransactionStatus Pending { get { return new TransactionStatus("Pending"); } }
        public static TransactionStatus Ready { get { return new TransactionStatus("Ready"); } }
        public static TransactionStatus Accepted { get { return new TransactionStatus("Accepted"); } }
        public static TransactionStatus Rejected { get { return new TransactionStatus("Rejected"); } }
        public static TransactionStatus Assigned { get { return new TransactionStatus("Assigned"); } }
        public static TransactionStatus Moving { get { return new TransactionStatus("Moving"); } }
        public static TransactionStatus TransportingToSelector { get { return new TransactionStatus("TransportingToSelector"); } }
        public static TransactionStatus SelectingDeliveryFromStart { get { return new TransactionStatus("SelectingDeliveryFromStart"); } }
        public static TransactionStatus Delivering { get { return new TransactionStatus("Delivering"); } }
        public static TransactionStatus Terminated { get { return new TransactionStatus("Terminated"); } }
        public static TransactionStatus Cancelled { get { return new TransactionStatus("Cancelled"); } }
        public static TransactionStatus Error { get { return new TransactionStatus("Error"); } }
        public static TransactionStatus Cancelling { get { return new TransactionStatus("Cancelling"); } }
        public static TransactionStatus SelectingPickUpNode { get { return new TransactionStatus("SelectingPickUpNode"); } }
        public static TransactionStatus SelectingDeliveryFromSelector { get { return new TransactionStatus("SelectingDeliveryFromSelector"); } }
        public static TransactionStatus MovingToDepartureSelector { get { return new TransactionStatus("MovingToDepartureSelector"); } }
        public static TransactionStatus End { get { return new TransactionStatus("End"); } }
    }
}
