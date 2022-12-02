using LiteDB;
using System.Collections;
using System.Linq;

namespace HarryPovarBot
{
    public class StickerRepository
    {
        private readonly LiteDatabase db;

        public StickerRepository()
        {
            db = new LiteDatabase("Stickers.db");
        }

        public Sticker? FindSticker(string text)
        {
            return db.GetCollection<Sticker>()
                .FindOne(x => text.Contains(x.ReferenceString!, System.StringComparison.InvariantCultureIgnoreCase));
        }

        public Sticker? FindStickerByStickerId(string stickerId)
        {
            return db.GetCollection<Sticker>()
                .FindOne(x => x.StickerId == stickerId);
        }

        public void Upsert(StickerQuery sticker)
        {
            var stickers = sticker.ReferenceStrings
                .Select(q => new Sticker(sticker.StickerId!, q));
            db.GetCollection<Sticker>().Upsert(stickers);
            db.GetCollection<Sticker>().EnsureIndex(q => q.StickerId);
        }
    }
}
