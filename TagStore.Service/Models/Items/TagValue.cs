using System;
using System.Globalization;

namespace TagStore.Service.Models.Items
{
    /// <summary>
    /// Base type used by Tag and TagTypeValue to store the value for different data types
    /// </summary>
    public abstract class TagValue
    {
        /// <summary>
        /// Current value of this Tag (contains a value for each type)
        /// </summary>
        public string ValueString { get; set; }
        /// <summary>
        /// Current value for this Tag if it is a integer number.
        /// </summary>
        public long? ValueLong { get; set; }
        /// <summary>
        /// Current value for this Tag if it is a decimal number.
        /// </summary>
        public decimal? ValueDecimal { get; set; }
        /// <summary>
        /// Current value for this Tag if it is a date, time datetime or datetime with timezone.
        /// </summary>
        public DateTimeOffset? ValueDate { get; set; }

        /// <summary>
        /// Returns the value as the specified type or tries to autodetect the stored value.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>May return string, long, decimal or DateTimeOffset</returns>
        public object GetValue(TagDataType type)
        {
            switch (type)
            {
                case TagDataType.String:
                    return ValueString;

                case TagDataType.Long:
                    return ValueLong;

                case TagDataType.Decimal:
                    return ValueDecimal;

                case TagDataType.Date:
                case TagDataType.Time:
                case TagDataType.DateTime:
                    return ValueDate;

                    //case TagDataType.Undefined:
                    //default:
            }

            // fallback: try to auto detect the type
            if (ValueLong.HasValue)
                return ValueLong.Value;

            if (ValueDecimal.HasValue)
                return ValueDecimal.Value;

            if (ValueDate.HasValue)
                return ValueDate.Value;

            return ValueString;
        }

        /// <summary>
        /// Sets the value as the specified type or tries to autodetect and store the value.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value">Value to be saved</param>
        public void SetValue(TagDataType type, object value)
        {
            if (value == null)
            {
                ValueString = null;
                ValueLong = null;
                ValueDecimal = null;
                ValueDate = null;
                return;
            }

            switch (type)
            {
                case TagDataType.String:
                    ValueLong = null;
                    ValueDecimal = null;
                    ValueDate = null;
                    ValueString = value as string ?? $"{value}";
                    return;

                case TagDataType.Long:
                    ValueLong = value.ParseLongOrDefault();
                    ValueDecimal = null;
                    ValueDate = null;
                    ValueString = ValueLong.HasValue ? ValueLong.Value.ToString(CultureInfo.InvariantCulture) : null;
                    return;

                case TagDataType.Decimal:
                    ValueLong = null;
                    ValueDecimal = value.ParseDecimalOrDefault();
                    ValueDate = null;
                    ValueString = ValueDecimal.HasValue ? ValueDecimal.Value.ToString(CultureInfo.InvariantCulture) : null;
                    return;

                case TagDataType.Date:
                case TagDataType.Time:
                case TagDataType.DateTime:
                    ValueLong = null;
                    ValueDecimal = null;
                    ValueDate = value.ParseDateTimeOffsetOrDefault();
                    ValueString = ValueDate.HasValue ? ValueDate.Value.ToString(CultureInfo.InvariantCulture) : null;
                    return;

                    //case TagDataType.Undefined:
                    //default:
            }

            // fallback: try to auto detect the type

            ValueLong = value.ParseLongOrDefault();
            if (ValueLong != null)
            {
                ValueDecimal = null;
                ValueDate = null;
                ValueString = ValueLong.HasValue ? ValueLong.Value.ToString(CultureInfo.InvariantCulture) : null;
                return;
            }

            ValueDecimal = value.ParseDecimalOrDefault();
            if (ValueDecimal != null)
            {
                ValueDate = null;
                ValueString = ValueDecimal.HasValue ? ValueDecimal.Value.ToString(CultureInfo.InvariantCulture) : null;
                return;
            }

            ValueDate = value.ParseDateTimeOffsetOrDefault();
            if (ValueDate != null)
            {
                ValueString = ValueDate.HasValue ? ValueDate.Value.ToString(CultureInfo.InvariantCulture) : null;
                return;
            }

            ValueString = value as string ?? $"{value}";
            return;
        }
    }
}
