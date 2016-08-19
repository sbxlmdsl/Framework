//-----------------------------------------------------------------------
// <copyright file="ISaveableEntity.cs" company="Genesys Source">
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
using Genesys.Foundation.Activity;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// Data access entity that can be saved
    /// </summary>
    [CLSCompliant(true)]
    public interface ISaveableEntity : IEntity
    {
        /// <summary>
        /// Is a new object, and most likely not yet committed to the database
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        /// ActivityFlowID
        /// </summary>
        int ActivityID { get; set; }

        /// <summary>
        /// Saves this object with automatic tracking.
        /// Less complicated than Save(IFlowClass), but minimal data logged and free-form architecture
        /// </summary>
        int Save();

        /// <summary>
        /// Saves this object with Workflow-based tracking.
        /// More complicated than Save(), but verbose logged info and ensures full-tiered architecture
        /// </summary>
        /// <param name="activity">Activity record that tracks this operation</param>
        int Save(IActivity activity);

        /// <summary>
        /// Deletes this object with automatic tracking.
        /// Less complicated than Delete(IFlowClass), but minimal data logged and free-form architecture
        /// </summary>
        void Delete();

        /// <summary>
        /// Deletes this object with Workflow-based tracking.
        /// More complicated than Delete(), but verbose logged info and ensures full-tiered architecture
        /// </summary>
        /// <param name="activity">Activity record that tracks this operation</param>
        void Delete(IActivity activity);
    }
}
