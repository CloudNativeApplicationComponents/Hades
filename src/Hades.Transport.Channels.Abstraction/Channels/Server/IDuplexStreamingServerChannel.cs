﻿using Hades.Transport.Channels.Abstraction.Handlers;

namespace Hades.Transport.Channels.Abstraction
{
    public interface IDuplexStreamingServerChannel :
        IHadesServerChannel<IDuplexStreamingHandler>
    {
    }
}