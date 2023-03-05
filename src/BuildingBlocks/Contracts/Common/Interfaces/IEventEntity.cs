using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Common.Events;
using Contracts.Domains.Interfaces;

namespace Contracts.Common.Interfaces;

public interface IEventEntity
{
    void AddDomainEvent(BaseEvent domainEvent);
    void RemoveDomainEvent(BaseEvent domainEvent);
    void ClearDomainEvent();
    IReadOnlyCollection<BaseEvent> DomainEvents();
}

public interface IEventEntity<T> : IEntityBase<T>, IEventEntity
{

}    
