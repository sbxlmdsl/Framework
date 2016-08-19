//-----------------------------------------------------------------------
// <copyright file="RecordStatus.cs" company="Genesys Source">
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

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// Status of the records current state. Can be multiple values to reduce collisions and maintain independent behavior on a per-value basis.
    /// Note: This is a [Flags] decorated enum. Values MUST be bitwise friendly (1, 2, 4, 8, 16, 32, etc.) 
    ///     None must be 0, and excluded from bitwise operations
    /// </summary>
    /// <remarks></remarks>
    [CLSCompliant(true)]
    [Flags]
    public enum RecordStatusValues
    {
        /// <summary>
        /// Normal behavior: Allows all querying and changes.
        /// </summary>
        Default = 0x0,

        /// <summary>
        /// ReadOnly: Do not allow to be changed. Ignore and log any change request. Alert calling app that record is read only (can be changed back to default to be altered later, not historical.)
        /// </summary>
        ReadOnly = 0x1,

        /// <summary>
        /// Record now historical. This record can never be updated, and will now be excluded out of all re-calculations (becomes a line item to feed historical counts.)
        /// </summary>
        Historical = 0x2,

        /// <summary>
        /// Deleted: This record is deleted and to be considered non-existent, even in historical re-calculations (will make historical counts shift.)
        /// </summary>
        Deleted = 0x2,
    }
}
