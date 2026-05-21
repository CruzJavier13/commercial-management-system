using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Shared.Contracts;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}
