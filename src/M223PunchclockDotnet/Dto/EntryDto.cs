using System.ComponentModel.DataAnnotations;
using M223PunchclockDotnet.Model;

namespace M223PunchclockDotnet.Dto
{
    /// <summary>Timestamps plus optional category and tags for creating or updating an entry.</summary>
    public sealed record EntryRequest : IValidatableObject
    {
        [Required]
        public DateTimeOffset? CheckIn { get; init; }

        [Required]
        public DateTimeOffset? CheckOut { get; init; }

        /// <summary>ID of the category, or null for no category.</summary>
        public int? CategoryId { get; init; }

        /// <summary>IDs of the tags to assign.</summary>
        public List<int> TagIds { get; init; } = new();

        /// <summary>Creates a new database entity from this request with already loaded relations.</summary>
        public Entry ToEntity(Category? category, IEnumerable<Tag> tags)
        {
            var entry = new Entry
            {
                CheckIn = CheckIn!.Value.UtcDateTime,
                CheckOut = CheckOut!.Value.UtcDateTime
            };
            ApplyTo(entry, category, tags);
            return entry;
        }

        /// <summary>Copies the values of this request onto an existing entity.</summary>
        public void ApplyTo(Entry entry, Category? category, IEnumerable<Tag> tags)
        {
            entry.CheckIn = CheckIn!.Value.UtcDateTime;
            entry.CheckOut = CheckOut!.Value.UtcDateTime;
            entry.Category = category;
            entry.CategoryId = category?.Id;
            entry.Tags.Clear();
            foreach (var tag in tags)
            {
                entry.Tags.Add(tag);
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CheckIn.HasValue && CheckOut.HasValue && CheckIn.Value >= CheckOut.Value)
            {
                yield return new ValidationResult(
                    "Check-In must be before Check-Out.",
                    new[] { nameof(CheckIn), nameof(CheckOut) });
            }
        }
    }

    public sealed record EntryResponse(int Id, DateTime CheckIn, DateTime CheckOut, int? CategoryId, IReadOnlyList<int> TagIds)
    {
        public static EntryResponse FromEntity(Entry entry) =>
            new(entry.Id, entry.CheckIn, entry.CheckOut, entry.CategoryId, entry.Tags.Select(t => t.Id).ToList());
    }
}
