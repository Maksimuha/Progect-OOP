using System.Text.RegularExpressions;
namespace Core.Shared;

public record TimeZonee
{
    public DOnotEmptyString Value { get; }

        private static readonly Regex TimezonePattern = new(
            @"^[A-Za-z]+(?:[_+-][A-Za-z0-9]+)*(?:/[A-Za-z]+(?:[_+-][A-Za-z0-9]+)*)+$",
            RegexOptions.Compiled);

        private TimeZonee(DOnotEmptyString value) => Value = value;

        public static TimeZonee Create(DOnotEmptyString value)
        {
            if (value is null)
                throw new ArgumentException(nameof(TimeZonee), "TimeZone не может быть null.");

            if (!TimezonePattern.IsMatch(value.Value))
                throw new ArgumentException(nameof(TimeZonee), $"Неверный формат TimeZone: {value.Value}");

            return new TimeZonee(value);
        }
    }
