﻿using Hades.Transport.Channels.Abstraction.Invokers;

namespace Hades.Transport.Channels.Abstraction
{
    public interface IEventClientChannel :
        IHadesClientChannel<IEventInvoker>
    {
    }
}