using System.ComponentModel.DataAnnotations;
using M223PunchclockDotnet.Model;

namespace M223PunchclockDotnet.Dto
{
    public sealed record CategoryRequest
    {
        [Required]
        public string Title { get; init; } = string.Empty;
    }

    public sealed record CategoryResponse(int Id, string Title)
    {
        public static CategoryResponse FromEntity(Category category) => new(category.Id, category.Title);
    }
}
