namespace Product.Data
{
    internal interface ITractorApiClient
    {
        Task<IEnumerable<Tractor>> GetTractorsAsync();
    }
}