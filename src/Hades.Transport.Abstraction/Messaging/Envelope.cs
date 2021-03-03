using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mime;

namespace Hades.Transport.Abstraction.Messaging
{
    public class Envelope : IEquatable<Envelope>
    {
        public string EnvelopeId { get; set; }
        public string? CorrelationId { get; set; } = default;
        public NameValueCollection Headers { get; } = new NameValueCollection();
        public object? Body { get; set; } = default;
        public AggregateException? Exception { get; private set; }
        public ContentType ContentType { get; }

        public Envelope(ContentType contentType)
        {
            ContentType = contentType
                ?? throw new ArgumentNullException(nameof(contentType));

            EnvelopeId = Guid.NewGuid().ToString();
        }

        public Envelope(Envelope envelope)
        {
            _ = envelope
                ?? throw new ArgumentNullException(nameof(envelope));
            EnvelopeId = envelope.EnvelopeId;
            ContentType = envelope.ContentType;
            CorrelationId = envelope.CorrelationId;
            Body = envelope.Body;
            Exception = envelope.Exception;

            foreach (string key in envelope.Headers.Keys)
            {
                Headers[key] = envelope.Headers[key];
            }
        }

        public Envelope AddException(Exception exception)
        {
            if (Exception == null)
            {
                Exception = new AggregateException(exception);
            }
            else
            {
                Exception = new AggregateException(Exception.InnerExceptions.Concat(new[] { exception }));
            }
            return this;
        }

        public bool Equals(Envelope other)
        {
            if (other == null)
                return false;
            return EnvelopeId == other.EnvelopeId;
        }

        public override bool Equals(object obj)
        {
            if(obj is Envelope envelope)
                return Equals(envelope);
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EnvelopeId, ContentType);
        }

        public override string ToString()
        {
            return $"EnvelopeId:{EnvelopeId},ContentType:{ContentType}";
        }
    }
}
