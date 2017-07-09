//-----------------------------------------------------------------------
// <copyright file="RecordState.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using System;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// Status of the entity current state. Can be multiple values to reduce collisions and maintain independent behavior on a per-value basis.
    /// Note: This is a [Flags] decorated enum. Values must be bitwise friendly (1, 2, 4, 8, 16, 32, etc.) 
    ///     None must be 0, and excluded from bitwise operations
    /// </summary>
    /// <remarks></remarks>
    [CLSCompliant(true)]
    [Flags]
    public enum EntityStates
    {
        /// <summary>
        /// Normal behavior: Allows all querying and changes.
        /// </summary>
        Default = 0x0,

        /// <summary>
        /// ReadOnly/Locked: Do not allow to be changed. Ignore and log any change request. Alert calling app that record is read only (can be changed back to default to be altered later, not historical.)
        /// </summary>
        ReadOnly = 0x1,

        /// <summary>
        /// Record now historical. This record can never be updated, and will now be excluded out of all re-calculations (becomes a line item to feed historical counts.)
        /// </summary>
        Historical = 0x2,

        /// <summary>
        /// Deleted: This record is deleted and to be considered non-existent, even in historical re-calculations (will make historical counts shift.)
        /// </summary>
        Deleted = 0x4,
    }

    /// <summary>
    /// Entity State attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class), CLSCompliant(true)]
    public class EntityState : Attribute, IAttributeValue<EntityStates>
    {
        /// <summary>
        /// Value of attribute
        /// </summary>
        public EntityStates Value { get; set; } = EntityStates.Default;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value to hydrate</param>
        public EntityState(EntityStates value)
        {
            Value = value;
        }
    }
}