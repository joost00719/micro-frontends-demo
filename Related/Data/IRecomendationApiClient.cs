namespace Related.Data
{
    internal interface IRecomendationApiClient
    {
        public Task<Recommendation> GetRecommendationAsync(int tractorId);
    }
}
