namespace X.Core {
	public enum ValidationSeverity {
		Information = 0,
		Warning = 1,
		Error = 2,
	}

	public record ErrorDescriptor(string Code, string Description, ValidationSeverity Severity = ValidationSeverity.Information);
}
