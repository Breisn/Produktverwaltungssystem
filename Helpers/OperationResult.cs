namespace ProductManagementSystem.Helpers
{
    public class OperationResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }

        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
