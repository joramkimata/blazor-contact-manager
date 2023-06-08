using System.ComponentModel.DataAnnotations;

namespace GameStore.Client.Models;

public class ContactInput
{
    [Required]
    public string PhoneNumber { get; set; }
}