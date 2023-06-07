namespace BusinessLayer.Models.ResponseModel
{
    public class ResponseModel<T> where T : class
    {
        public T Data { get; set; }
        public object? AdditionalData { get; set; }
        public string? Message { get; set; }

        public ResponseModel(T data, object? additionalData = null, string? message = null)
        {
            Data = data;
            AdditionalData = additionalData;
            Message = message;
        }
    }
}
