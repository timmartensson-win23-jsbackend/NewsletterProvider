
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class SubscribeEntity
{
    [Key]
    public string Email { get; set; } = null!;
    public bool IsSubscribed { get; set; } = false;
    public bool DailyNewsLetter { get; set; } = false;
    public bool AdvertisingUpdates { get; set; } = false;
    public bool WeekInReviews { get; set; } = false;
    public bool EventUpdates { get; set; } = false;
    public bool StartupsWeekly { get; set; } = false;
    public bool Podcasts { get; set; } = false;
}
