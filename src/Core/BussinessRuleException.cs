using System;
using JetBrains.Annotations;

namespace X.Core {
    [PublicAPI]
    public class InvalidBusinessRuleException : Exception {
        public IBusinessRule BrokenRule { get; }

        public InvalidBusinessRuleException(IBusinessRule brokenRule)
            : base($"{brokenRule.Descriptor.Code}: {brokenRule.Descriptor.Description}") {
            BrokenRule = brokenRule;
        }

        public override string ToString() {
            return Message;
        }
    }
}
