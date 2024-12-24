using Arvant.Domain.Common;
using Arvant.Domain.Entities;

namespace Arvant.Domain.Events;

public class NewCallCreatedEvent : BaseEvent
{
    public Call Call { get; }
    
    public NewCallCreatedEvent(Call call)
    {
        Call = call;
    }
}
