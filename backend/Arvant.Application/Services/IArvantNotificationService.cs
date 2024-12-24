using Arvant.Common.Dto.Call;

namespace Arvant.Application.Services;

public interface IArvantNotificationService
{
    Task SendIncomingCallEventAsync(IncomingCallEventDto incomingCallEventDto, string connectionId);
}
