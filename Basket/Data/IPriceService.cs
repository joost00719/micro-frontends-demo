namespace Basket.Data
{
    internal interface IPriceService
    {
        Task<int> GetPrice(int tractorId);
    }
}