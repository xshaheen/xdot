namespace Core.Tests {
    /// <summary>
    /// The purpose of these is to see there are no unexpected compiler warnings/errors.
    /// Note: project must be set to WarningAsError.
    /// </summary>
    public static class CompilerAssert {
        public static void Nullable(ref string? value) { }

        public static void NotNullable(object value) { }
    }
}
