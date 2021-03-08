using Google.Protobuf.Reflection;

namespace Hades.Edge.Grpc.Internal.Messages
{
    internal static class MessageReflection
    {
        #region Descriptor
        public static FileDescriptor Descriptor
        {
            get { return _descriptor; }
        }

        private static readonly FileDescriptor _descriptor;

        static MessageReflection()
        {
            byte[] descriptorData = System.Convert.FromBase64String(
                string.Concat(
                  "ChRwcm90b3MvbWVzc2FnZS5wcm90bxIJbWVzc2FnaW5nIg4KDEVtcHR5TWVz",
                  "c2FnZWIGcHJvdG8z"));
            _descriptor = FileDescriptor.FromGeneratedCode(descriptorData,
                new FileDescriptor[] { },
                new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[] {
            new GeneratedClrTypeInfo(typeof(ProtobufMessage), ProtobufMessage.Parser, null, null, null, null, null)
                }));
        }
        #endregion
    }
}