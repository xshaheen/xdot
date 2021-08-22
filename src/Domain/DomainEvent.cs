using System;
using MediatR;

namespace X.Domain {
	public abstract class DomainEvent : INotification {
		protected DomainEvent() {
			Id = Guid.NewGuid();
			At = DateTimeOffset.UtcNow;
		}

		public Guid Id { get; protected set; }

		public DateTimeOffset At { get; protected set; }

		public bool IsPublished { get; set; }
	}
}
