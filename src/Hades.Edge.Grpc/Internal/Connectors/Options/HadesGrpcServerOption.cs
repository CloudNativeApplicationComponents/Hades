﻿using System;
using System.Collections.Generic;

namespace Hades.Edge.Grpc.Internal.Connectors
{
    //TODO this option must contain server credentioal configurations
    public class HadesGrpcConnectorOption
    {
        public Uri Endpoint { get; set; }
        public List<int> Port { get; set; }
    }
}
