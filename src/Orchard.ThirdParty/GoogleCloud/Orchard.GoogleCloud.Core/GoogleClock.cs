using System;
using Microsoft.AspNetCore.Modules;

namespace Orchard.GoogleCloud.Core
{
    public class GoogleClock : Google.Api.Gax.IClock
    {
        private readonly IClock _clock;

        public GoogleClock(IClock clock)
        {
            _clock = clock;
        }

        public DateTime GetCurrentDateTimeUtc()
        {
            return _clock.UtcNow;
        }
    }
}
