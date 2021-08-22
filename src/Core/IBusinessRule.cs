namespace X.Core {
	public interface IBusinessRule {
		bool IsBroken();

		ErrorDescriptor Descriptor { get; }
	}
}
