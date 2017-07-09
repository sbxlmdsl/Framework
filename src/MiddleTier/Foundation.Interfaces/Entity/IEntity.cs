//-----------------------------------------------------------------------
// <copyright file="IEntity.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extras.Serialization;
using System;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// Base used for all entity classes
    /// </summary>    
    [CLSCompliant(true)]
    public interface IEntity : IID, IKey, ICreatedDate, IModifiedDate, ISerialize<IEntity>
    {
        /// <summary>
        /// Is a new object, and most likely not yet committed to the database
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        /// ActivityFlowID
        /// </summary>
        int ActivityContextID { get; set; }

        /// <summary>
        /// Status of this record with static values: 0x0 - Default, 0x1 - ReadOnly, 0x2 - Historical, 0x4 - Deleted
        /// </summary>
        EntityStates Status { get; set; }
    }
}
