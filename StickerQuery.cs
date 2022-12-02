using System.Collections.Generic;

namespace HarryPovarBot
{
    public class StickerQuery
    {
        public int Id { get; set; }
        public List<string> ReferenceStrings { get; set; }
        public string? StickerId { get; set; }

        public StickerQuery()
        {
            ReferenceStrings = new List<string>();
        }
    }

    public class Sticker
    {
        public int Id { get; set; }
        public string ReferenceString { get; set; }
        public string StickerId { get; set; }

        public Sticker(string stickerId, string referenceString)
        {
            StickerId = stickerId;
            ReferenceString = referenceString;
        }
    }
}
