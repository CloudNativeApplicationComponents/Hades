using System;
using System.Collections.Specialized;
using System.Net.Mime;

namespace Hades.Core.Abstraction.Distributions
{
    public class Envelope
    {
        public Guid EnvelopeId { get; set; }
        public Guid? CorrelationId { get; set; }
        public NameValueCollection Headers { get; } = new NameValueCollection();
        public object Body { get; set; }
        public HadesTopic Topic { get; }
        public ContentType ContentType { get; }

        public Envelope(HadesTopic topic, ContentType contentType)
        {
            Topic = topic
                ?? throw new ArgumentNullException(nameof(topic));

            ContentType = contentType
                ?? throw new ArgumentNullException(nameof(contentType));

            EnvelopeId = Guid.NewGuid();
        }
        public Envelope(Envelope envelope)
        {
            _ = envelope
                ?? throw new ArgumentNullException(nameof(envelope));
            Topic = envelope.Topic;
            ContentType = envelope.ContentType;
            Body = envelope.Body;

            foreach (string key in envelope.Headers.Keys)
            {
                Headers[key] = envelope.Headers[key];
            }
        }
    }
}
