//-----------------------------------------------------------------------
// <copyright file="IWorkerParameter.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Foundation.Session;

namespace Genesys.Foundation.Worker
{
    /// <summary>
    /// Parameter and data for any process
    /// </summary>
    /// <typeparam name="TDataIn">Type of input data for the process</typeparam>
    [CLSCompliant(true)]
    public interface IWorkerParameter<TDataIn>
    {
        /// <summary>
        /// App, User, Device context
        /// </summary>
        ISessionContext Context { get; set; }

        /// <summary>
        /// Input data for the process
        /// </summary>
        TDataIn DataIn { get; set; }
    }
}
