using System;
using System.Globalization;

namespace TagStore.Service
{
    public static class Extensions
    {
        public static long? ParseLongOrDefault(this object value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch (FormatException) // value is not in an appropriate format.
            {
                return null;
            }
            catch (InvalidCastException) // value does not implement the System.IConvertible interface. -or- The conversion is not supported.
            {
                return null;
            }
            catch (OverflowException) // value represents a number that is less than System.Int64.MinValue or greater than System.Int64.MaxValue.
            {
                return null;
            }
        }

        public static decimal? ParseDecimalOrDefault(this object value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (FormatException) // value is not in an appropriate format.
            {
                return null;
            }
            catch (InvalidCastException) // value does not implement the System.IConvertible interface. -or- The conversion is not supported.
            {
                return null;
            }
            catch (OverflowException) // value represents a number that is less than System.Decimal.MinValue or greater than System.Decimal.MaxValue.
            {
                return null;
            }
        }

        public static DateTimeOffset? ParseDateTimeOffsetOrDefault(this object value)
        {
            if (value is DateTimeOffset dto)
                return dto;

            if (value is DateTime dt)
                return new DateTimeOffset(dt);

            if (value is TimeSpan ts)
                return new DateTimeOffset(new DateTime(0,0,0), ts);

            if (DateTimeOffset.TryParse(value as string ?? $"{value}", 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.AssumeUniversal | 
                    DateTimeStyles.AllowInnerWhite | DateTimeStyles.AllowLeadingWhite |  DateTimeStyles.AllowTrailingWhite |DateTimeStyles.AllowWhiteSpaces, 
                    out dto))
                return dto;

            return null;
        }
    }
}
