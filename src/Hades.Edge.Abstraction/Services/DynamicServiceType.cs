namespace Hades.Edge.Abstraction.Services
{
    public enum DynamicServiceType
    {
        Event = 1,
        Unary = 2,
        ClientStreaming = 3,
        ServerStreaming = 4,
        DuplexStreaming = 5,
    }
}
