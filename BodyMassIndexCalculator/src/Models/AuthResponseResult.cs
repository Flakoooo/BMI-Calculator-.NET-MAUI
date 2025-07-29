namespace BodyMassIndexCalculator.src.Models
{
    public record class AuthResponseResult<T>(T? Result, string? Error)
    {
        public T? Result { get; } = Result;
        public string? Error { get; } = Error;
    }
}
