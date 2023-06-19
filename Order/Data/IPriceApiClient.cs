namespace Order.Data
{
    internal interface IPriceApiClient
    {
        public Task<int> GetPriceAsync(int tractorId);
    }
}
