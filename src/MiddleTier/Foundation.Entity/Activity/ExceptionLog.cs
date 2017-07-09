//-----------------------------------------------------------------------
// <copyright file="ExceptionLog.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extensions;

namespace Genesys.Foundation.Activity
{
    /// <summary>
    /// This data-only object is used by EF code-first to define the schema of the table that log Exceptions
    /// </summary>
    /// <remarks></remarks>
    [CLSCompliant(true)]
    public class ExceptionLog : IExceptionLog
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ExceptionLogID { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// exceptionField
        /// </summary>
        protected Exception CurrentException { get; set; } = new Exception("No exception thrown");

        /// <summary>
        /// Message
        /// </summary>
        public string Message
        {
            get { return CurrentException.Message; }
            protected set { }
        }

        /// <summary>
        /// InnerException
        /// </summary>
        public string InnerException
        {
            get
            {
                if (CurrentException.InnerException == null == false)
                {
                    return CurrentException.InnerException.ToString();
                }
                else
                {
                    return TypeExtension.DefaultString;
                }
            }
            protected set { }
        }

        /// <summary>
        /// StackTrace
        /// </summary>
        public string StackTrace
        {
            get { return CurrentException.StackTrace.ToStringSafe(); }
            protected set { }
        }

        /// <summary>
        /// CustomMessage
        /// </summary>
        public string CustomMessage { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; } = TypeExtension.DefaultDate;
        
        /// <summary>
        /// This protected constructor should not be called. Factory methods should be used instead.
        /// </summary>
        public ExceptionLog()
            : base()
        {
            CreatedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="concreteType"></param>
        /// <param name="customMessage"></param>
        public ExceptionLog(Exception exception, Type concreteType, string customMessage)
            : this()
        {            
            CurrentException = exception;
            CustomMessage = String.Format("Error in type: {0}. Message: {1}", concreteType.ToString(), customMessage);
        }
        
        /// <summary>
        /// Returns the typed string of the primary property.
        /// </summary>
        public override string ToString()
        {
            return ExceptionLogID.ToString();
        }
    }
}