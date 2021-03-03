using Hades.Transport.Abstraction.EndpointAccessors;
using System;

namespace Hades.Transport.Grpc.Internal
{
    internal abstract class CorrelativeSubmissionBase : ICorrelativeSubmission,
            IEquatable<CorrelativeSubmissionBase>
    {
        private bool _disposedValue;

        public string Id { get; }

        protected CorrelativeSubmissionBase(string? id = default)
        {
            Id = id ?? Guid.NewGuid().ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj is CorrelativeSubmissionBase value)
                return Equals(value);
            return false;
        }

        public bool Equals(CorrelativeSubmissionBase other)
        {
            if (other == null)
                return false;
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
