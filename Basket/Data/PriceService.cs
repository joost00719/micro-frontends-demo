namespace Basket.Data
{
    internal class PriceService : IPriceService
    {
        public Task<int> GetPrice(int tractorId)
        {
            var price = (((float)tractorId / 3) * 27) * 100;
            return Task.FromResult((int)price);
        }
    }
}