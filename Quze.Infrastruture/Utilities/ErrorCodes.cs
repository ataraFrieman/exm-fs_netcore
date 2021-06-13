using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Infrastruture.Utilities
{
    public enum ErrorCodes
    {
        /// <summary>
        /// The request is null - 1000
        /// </summary>
        NullRequest = 1000,
        EntityEmpty = 1001,
        UnexpectedException = 1002,
        EntityToUpdateDoesNotExist = 1003
    }
}
