namespace Related.Data
{
    internal class RecomendationApiClient : IRecomendationApiClient
    {
        public async Task<Recommendation> GetRecommendationAsync(int tractorId)
        {
            if (tractorId == 1)
            {
                return new Recommendation { Image1 = "reco_3.jpg", Image2 = "reco_5.jpg", Image3 = "reco_6.jpg" };
            }
            else if (tractorId == 2)
            {
                return new Recommendation { Image1 = "reco_3.jpg", Image2 = "reco_6.jpg", Image3 = "reco_4.jpg" };
            }
            else if (tractorId == 3)
            {
                return new Recommendation { Image1 = "reco_1.jpg", Image2 = "reco_8.jpg", Image3 = "reco_7.jpg" };
            }

            return null;
        }
    }
}
