public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    
    // Hata mesajı için yeni property
    public string? Message { get; set; }
} 