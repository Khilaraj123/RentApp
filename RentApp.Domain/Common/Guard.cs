namespace RentApp.Domain.Common
{
    public static class Guard
    {
        public static void AgainstNull(object? value, string parameterName)
        {
            if (value is null)
                throw new ArgumentNullException(parameterName);
        }

        public static void AgainstNullOrWhiteSpace(
            string? value,
            string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                $"{parameterName} cannot be empty.",
                parameterName);
    }

    public static void AgainstNegative(decimal value, string parameterName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(parameterName);
    }

    public static void AgainstNegative(int value, string parameterName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(parameterName);
    }

    public static void AgainstOutOfRange(
        DateTime start,
        DateTime end,
        string message = "End date must be after start date.")
    {
        if (end <= start)
            throw new ArgumentException(message);
    }
}
}