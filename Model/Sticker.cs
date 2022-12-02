namespace HarryPovarBot
{
    public class Sticker
    {
        public int Id { get; set; }
        public string StickerId { get; set; }
        public string ReferenceString { get; set; }

        public Sticker(string stickerId, string referenceString)
        {
            StickerId = stickerId;
            ReferenceString = referenceString;
        }
    }
}
