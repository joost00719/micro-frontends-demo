namespace Related.Data
{
    public class Recommendation
    {
        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        public IEnumerable<string> AsEnumerable()
        {
            yield return Image1;
            yield return Image2;
            yield return Image3;
        }
    }
}