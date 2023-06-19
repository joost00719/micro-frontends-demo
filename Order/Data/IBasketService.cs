namespace Order.Data
{
    internal interface IBasketService
    {
        public Task AddAsync(int tractorId, int quantity);

        public int Count { get; }

        public event Action? OnChange;
    }
}
