using System.ComponentModel.DataAnnotations;
using M223PunchclockDotnet.Model;

namespace M223PunchclockDotnet.Dto
{
    public sealed record TagRequest
    {
        [Required]
        public string Title { get; init; } = string.Empty;
    }

    public sealed record TagResponse(int Id, string Title)
    {
        public static TagResponse FromEntity(Tag tag) => new(tag.Id, tag.Title);
    }
}
