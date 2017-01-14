namespace AccountingSystem.Core.Base
{
    public class StatusMessage
    {
        public string Message { get; set; }
        public StatusMessageType StatusMessageType { get; set; }

        public StatusMessage(string message, StatusMessageType statusMessageType)
        {
            Message = message;
            StatusMessageType = statusMessageType;
        }
    }

    public enum StatusMessageType
    {
        Success,
        Worng,
        Dangers,
    }
}
