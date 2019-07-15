using System;

namespace GridTableBuilder.GridModel.Helpers
{
    [Serializable]
    public class VersionInfo
    {
        public const int DEFAULT_VERSION = 1;

        public int Version = DEFAULT_VERSION;
    }

}
