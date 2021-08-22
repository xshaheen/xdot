using System;

namespace X.Domain {
	public interface ICreatorAudit<TId> where TId : IEquatable<TId> {
		TId CreatedById { get; set; }
	}

	public interface ICreatorAudit<TId, out TUser> : ICreatorAudit<TId> where TId : IEquatable<TId> where TUser : class {
		TUser CreatedBy { get; }
	}
}
