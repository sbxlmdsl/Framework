﻿//-----------------------------------------------------------------------
// <copyright file="ProcessManager.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Threading.Tasks;
using Genesys.Extensions;
using Genesys.Extras.Net;
using Genesys.Extras.Serialization;
using Genesys.Foundation.Session;

namespace Genesys.Foundation.Process
{
    /// <summary>
    /// Manages sending data to a process, and retrieving the result of that process
    /// </summary>
    /// <typeparam name="TDataIn">Data sent to service</typeparam>
    /// <typeparam name="TDataOut">Data returned from service</typeparam>
    [CLSCompliant(true)]
    public class ProcessManager<TDataIn, TDataOut>
    {
        private string ProcessServiceURL = TypeExtension.DefaultString;
        private ProcessParameter<TDataIn> DataIn = new ProcessParameter<TDataIn>();

        /// <summary>
        /// Serializer of entire parameter
        /// </summary>
        public ISerializer<ProcessParameter<TDataIn>> Serializer { get; } = new JsonSerializer<ProcessParameter<TDataIn>>();

        /// <summary>
        /// De-serializer of entire result
        /// </summary>
        public ISerializer<ProcessResult> Deserializer { get; } = new JsonSerializer<ProcessResult>();
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processServiceURL">Url</param>
        /// <param name="context">Context of user, device and app</param>
        /// <param name="dataIn">Data to send</param>
        public ProcessManager(string processServiceURL, SessionContext context, TDataIn dataIn) : base()
        {
            ProcessServiceURL = processServiceURL;
            DataIn = new ProcessParameter<TDataIn>(context, dataIn);
            Serializer.KnownTypes.Add(dataIn.GetType());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processServiceURL">Url</param>
        /// <param name="dataIn">Data to send</param>
        public ProcessManager(string processServiceURL, IProcessParameter<TDataIn> dataIn) : base()
        {            
            ProcessServiceURL = processServiceURL;
            DataIn = dataIn.DirectCastSafe<ProcessParameter<TDataIn>>();
            Serializer.KnownTypes.Add(dataIn.GetType());
        }
        
        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the Flow
        /// </summary>
        
        public ProcessResult Send()
        {
            ProcessResult returnValue = new ProcessResult();
            var result = TypeExtension.DefaultString;

            // Call middle tier to process
            HttpRequestPostString request = new HttpRequestPostString(this.ProcessServiceURL, this.Serializer.Serialize(this.DataIn));
            result = request.Send();
            returnValue = this.Deserializer.Deserialize(result);
            
            return returnValue;
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the Flow
        /// </summary>
        
        public async Task<ProcessResult> SendAsync()
        {
            ProcessResult returnValue = new ProcessResult();
            var result = TypeExtension.DefaultString;

            HttpRequestPostString request = new HttpRequestPostString(this.ProcessServiceURL, this.Serializer.Serialize(this.DataIn));
            result = await request.SendAsync();
            returnValue = this.Deserializer.Deserialize(result);

            return returnValue;
        }
    }
}