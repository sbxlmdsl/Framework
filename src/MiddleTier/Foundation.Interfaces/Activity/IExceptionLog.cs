//-----------------------------------------------------------------------
// <copyright file="IExceptionLog.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Foundation.Entity;

namespace Genesys.Foundation.Activity
{
    /// <summary>
    /// Exception logged record
    /// </summary>
    public interface IExceptionLog : ICreatedDate
    {
        /// <summary>
        /// Primary key of the log record
        /// </summary>
        int ExceptionLogID { get; set; }

        /// <summary>
        /// Custom message from exception
        /// </summary>
        string CustomMessage { get; set; }

        /// <summary>
        /// Full exception message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Inner exception
        /// </summary>
        string InnerException { get; }

        /// <summary>
        /// Stack trace of the exception
        /// </summary>
        string StackTrace { get; }
    }
}