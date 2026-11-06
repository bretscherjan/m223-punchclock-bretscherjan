using M223PunchclockDotnet.Model;

namespace M223PunchclockDotnet.Dto
{
    /// <summary>Represents an entry as returned by the REST API.</summary>
    public sealed record EntryResponse(int Id, DateTime CheckIn, DateTime CheckOut)
    {
        public static EntryResponse FromEntity(Entry entry) =>
            new(entry.Id, entry.CheckIn, entry.CheckOut);
    }
}
