using System.ComponentModel.DataAnnotations;
using M223PunchclockDotnet.Model;

namespace M223PunchclockDotnet.Dto
{
    /// <summary>Contains the timestamps for creating or updating an entry.</summary>
    public sealed record EntryRequest : IValidatableObject
    {
        [Required]
        public DateTimeOffset? CheckIn { get; init; }

        [Required]
        public DateTimeOffset? CheckOut { get; init; }

        /// <summary>Creates a new database entity from this request.</summary>
        public Entry ToEntity() => new Entry
        {
            CheckIn = CheckIn!.Value.UtcDateTime,
            CheckOut = CheckOut!.Value.UtcDateTime
        };

        /// <summary>Copies the values of this request onto an existing entity.</summary>
        public void ApplyTo(Entry entry)
        {
            entry.CheckIn = CheckIn!.Value.UtcDateTime;
            entry.CheckOut = CheckOut!.Value.UtcDateTime;
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
}
