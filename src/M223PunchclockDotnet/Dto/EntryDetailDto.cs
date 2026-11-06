using M223PunchclockDotnet.Model;

namespace M223PunchclockDotnet.Dto
{
    /// <summary>An entry with its category and tags expanded. Nested DTOs contain no back references.</summary>
    public sealed record EntryDetailDto(
        int Id,
        DateTime CheckIn,
        DateTime CheckOut,
        CategoryResponse? Category,
        IReadOnlyList<TagResponse> Tags)
    {
        public static EntryDetailDto FromEntity(Entry entry) =>
            new(entry.Id,
                entry.CheckIn,
                entry.CheckOut,
                entry.Category is null ? null : CategoryResponse.FromEntity(entry.Category),
                entry.Tags.Select(TagResponse.FromEntity).ToList());
    }
}
