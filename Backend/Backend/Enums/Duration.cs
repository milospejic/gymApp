using System.Runtime.Serialization;

namespace Backend.Enums
{
    /// <summary>
    /// Represents the available durations for a membership plan.
    /// </summary>
    public enum Duration
    {
        /// <summary>
        /// Membership valid for one month.
        /// </summary>
        [EnumMember(Value = "1 month")]
        OneMonth = 1,

        /// <summary>
        /// Membership valid for three months.
        /// </summary>
        [EnumMember(Value = "3 months")]
        ThreeMonths = 3,

        /// <summary>
        /// Membership valid for six months.
        /// </summary>
        [EnumMember(Value = "6 months")]
        SixMonths = 6,

        /// <summary>
        /// Membership valid for one year.
        /// </summary>
        [EnumMember(Value = "1 year")]
        OneYear = 12
    }
}
