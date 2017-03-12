//-----------------------------------------------------------------------
// <copyright file="ICrudEntity.cs" company="Genesys Source">
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
using System.Collections.Generic;
using System.Linq.Expressions;

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
        /// Deletes this object with Workflow-based tracking.
        /// More complicated than Delete(), but verbose logged info and ensures full-tiered architecture
        /// </summary>
        /// <param name="activity">Activity record that tracks this operation</param>
        bool Delete(IActivityContext activity);

        /// <summary>
        /// Worker method that deletes this object with automatic tracking.
        /// Less complicated than Delete(IActivity), but only minimal data logged
        /// </summary>
        bool Delete(bool forceDelete);

        /// <summary>
        /// Inserts or updates this object with flow-based tracking.
        /// More complicated than Save(), but verbose logged info and ensures full-tiered architecture
        /// </summary>
        /// <param name="activity">Activity record that tracks this operation</param>
        bool Save(IActivityContext activity);

        /// <summary>
        /// Worker method that inserts or updates this object with automatic tracking.
        /// Less complicated than Save(IActivity), but only minimal data logged
        /// </summary>
        bool Save(bool forceInsert);
    }
}
