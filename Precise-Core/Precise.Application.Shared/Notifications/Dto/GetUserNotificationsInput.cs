using Abp.Notifications;
using Precise.Dto;

namespace Precise.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}