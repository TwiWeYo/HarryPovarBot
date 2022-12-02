using System.Collections.Generic;

namespace HarryPovarBot
{
    public class Sticker
    {
        public string Id { get; set; }
        public string ReferenceString { get; set; }

        public Sticker(string stickerId, string referenceString)
        {
            Id = stickerId;
            ReferenceString = referenceString;
        }
    }
}
