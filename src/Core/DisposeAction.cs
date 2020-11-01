﻿using System;

namespace X.Core
{
    /// <summary>
    ///     This class can be used to provide an action when
    ///     Dispose method is called.
    /// </summary>
    public class DisposeAction : IDisposable
    {
        private readonly Action _action;

        /// <summary>
        ///     Creates a new <see cref="DisposeAction"/> object.
        /// </summary>
        /// <param name="action">Action to be executed when this object is disposed.</param>
        public DisposeAction(Action action) { _action = action ?? throw new ArgumentNullException(nameof(action)); }

        public void Dispose() { _action(); }
    }

    public sealed class NullDisposable : IDisposable
    {
        private NullDisposable() { }
        public static NullDisposable Instance { get; } = new NullDisposable();

        public void Dispose() { }
    }
}
