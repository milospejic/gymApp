using System.Runtime.Serialization;

namespace Backend.Enums
{
    public enum Duration
    {
        [EnumMember(Value = "1 month")]
        OneMonth = 1,

        [EnumMember(Value = "3 months")]
        ThreeMonths = 3,

        [EnumMember(Value = "6 months")]
        SixMonths = 6,

        [EnumMember(Value = "1 year")]
        OneYear = 12
    }
}
