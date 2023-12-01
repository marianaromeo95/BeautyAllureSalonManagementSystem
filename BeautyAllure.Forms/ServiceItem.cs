namespace BeautyAllure.Forms
{
    public class ServiceItem
    {
        public int ServiceId { get; }
        public string ServiceType { get; }
        public int Duration { get; }
        public decimal Price { get; }

        public ServiceItem(int serviceId, string serviceType, int duration, decimal price)
        {
            ServiceId = serviceId;
            ServiceType = serviceType;
            Duration = duration;
            Price = price;
        }

        public override string ToString()
        {
            return ServiceType;
        }
    }
}
