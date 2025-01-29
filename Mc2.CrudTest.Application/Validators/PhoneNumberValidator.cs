using PhoneNumbers;

namespace Mc2.CrudTest.Application.Validators;
public static class PhoneNumberValidator
{
    public static bool IsValidMobileNumber(string phoneNumber, string region = "US")
    {
        try
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            var number = phoneUtil.Parse(phoneNumber, region);
            return phoneUtil.IsValidNumber(number) && phoneUtil.GetNumberType(number) == PhoneNumberType.FIXED_LINE_OR_MOBILE;
        }
        catch
        {
            return false;
        }
    }
}
