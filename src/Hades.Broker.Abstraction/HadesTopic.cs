using System;

namespace Hades.Broker.Abstraction
{
    public class HadesTopic : IEquatable<HadesTopic>
    {
        public string Name { get; private set; }
        public HadesTopic(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as HadesTopic);
        }

        public bool Equals(HadesTopic other)
        {
            return Name == other?.Name;
        }

        public static implicit operator string(HadesTopic topic)
        {
            return topic.Name;
        }

        public static implicit operator HadesTopic(string topicName)
        {
            return new HadesTopic(topicName);
        }
    }
}
