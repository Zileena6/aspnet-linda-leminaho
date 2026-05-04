namespace CoreFitness.Domain.Common
{
    public sealed class Error
    {
        public string Code { get; }
        public string Message { get; }

        private Error(string code, string message)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public static Error NotFound(string entity, object id) =>
            new("NotFound", $"{entity} with id {id} was not found");

        public static Error Validation(string message) =>
            new("Validation", message);

        public static Error Conflict(string message) =>
            new("Conflict", message);

        public static Error Unauthorized(string message) =>
            new("Unauthorized", message);

        public static Error Failure(string message) =>
            new("Failure", message);
    }
}
