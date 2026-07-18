namespace RentApp.Domain.Common
{
    public class Result
    {
        protected Result(bool succeeded, string? error)
        {
            Succeeded = succeeded;
            Error = error;
        }

        public bool Succeeded { get; }

        public string? Error { get; }

        public static Result Success()
        {
            return new Result(true, null);
        }

        public static Result Failure(string error)
        {
            return new Result(false, error);
        }
    }

    public class Result<T>
        : Result
    {
        private readonly T? _value;

        private Result(bool succeeded, string? error, T? value)
            : base(succeeded, error)
        {
            _value = value;
        }

        public T Value => Succeeded ? _value! : default!;

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, null, value);
        }

        public static Result<T> Failure(string error)
        {
            return new Result<T>(false, error, default);
        }
    }
}