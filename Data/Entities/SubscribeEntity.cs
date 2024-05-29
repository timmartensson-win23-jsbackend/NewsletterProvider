
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class SubscribeEntity
{
    [Key]
    public string Email { get; set; } = null!;
    public string? OldEmail { get; set; }
    public bool DailyNewsLetter { get; set; }
    public bool AdvertisingUpdates { get; set; } 
    public bool WeekInReviews { get; set; } 
    public bool EventUpdates { get; set; } 
    public bool StartupsWeekly { get; set; } 
    public bool Podcasts { get; set; } 
}
