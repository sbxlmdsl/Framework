//-----------------------------------------------------------------------
// <copyright file="WorkerParameter.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extensions;
using Genesys.Foundation.Security;
using Genesys.Foundation.Session;

namespace Genesys.Foundation.Worker
{
    /// <summary>
    /// Result that passes back failed rules, and return data
    /// </summary>
    /// <typeparam name="TDataIn">Type of data to pass</typeparam>
    [CLSCompliant(true)]
    public class WorkerParameter<TDataIn> : IWorkerParameter<TDataIn>
    {
        private SessionContext context = new SessionContext();
        
        /// <summary>
        /// Identity of user initiating this process
        /// </summary>
        public SessionContext Context { get { return context; } set { context = value as SessionContext; } }

        // Insist any interface types have a concrete equivalent, especially for serialization
        ISessionContext IWorkerParameter<TDataIn>.Context { get { return context; } set { context = value as SessionContext; } }

        /// <summary>
        /// Data to be returned
        /// </summary>
        public TDataIn DataIn { get; set; } = TypeExtension.InvokeConstructorOrDefault<TDataIn>();
        
        /// <summary>
        /// Force hydration on constructor
        /// </summary>
        public WorkerParameter() : base()
        {
        }

        /// <summary>
        /// Constructor that partially hydrates
        /// </summary>
        public WorkerParameter(TDataIn inputData) : this()
        {
            DataIn = inputData;
        }

        /// <summary>
        /// Constructor that fully hydrates
        /// </summary>
        public WorkerParameter(UserPrincipal principalIdentity, TDataIn inputData) : this(inputData)
        {
            Context.Fill(principalIdentity);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">User, device and app context</param>
        /// <param name="data">Data to send</param>
        public WorkerParameter(ISessionContext context, TDataIn data) : this(data)
        {
            Context.Fill(context);
        }       
    }
}
