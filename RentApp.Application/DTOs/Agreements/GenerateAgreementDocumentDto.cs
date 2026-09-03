using System;
using System.ComponentModel.DataAnnotations;

namespace RentApp.Application.DTOs.Agreements;

public class GenerateAgreementDocumentDto
{
    public string DocumentUrl { get; set; } = string.Empty;
    
    [Required]
    public string DocumentFileName { get; set; } = string.Empty;
    
    [Required]
    public long DocumentSize { get; set; }
    
    [Required]
    public string DocumentHash { get; set; } = string.Empty;
}
