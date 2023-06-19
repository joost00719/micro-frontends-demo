namespace Order.Data
{
    internal class PriceApiClient : IPriceApiClient
    {
        public Task<int> GetPriceAsync(int tractorId)
        {
            // For this demo we just generate a price based on the tractor id.
            // In a real world scenario you'll want to implement an actual price service.
            var price = (float)tractorId * 1300;

            return Task.FromResult((int)price);
        }
    }
}
