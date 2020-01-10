using System;
using MassTransit;

namespace CRM.Contact.IntegrationEvents
{
    public interface ContactCreated : CorrelatedBy<Guid>
    {
         
    }
}