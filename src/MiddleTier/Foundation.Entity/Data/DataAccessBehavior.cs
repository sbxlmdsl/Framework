//-----------------------------------------------------------------------
// <copyright file="DataAccessBehaviorValues.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extensions;

namespace Genesys.Foundation.Data
{
    /// <summary>
    /// enumeration to allow the attribute to use strongly-typed ID
    /// </summary>
    [CLSCompliant(true)]
    public enum DataAccessBehaviors
    {
        /// <summary>
        /// All Select, Insert, Update and Delete functionality
        /// </summary>
        AllAccess = 0,

        /// <summary>
        /// Insert functionality
        /// </summary>
        InsertOnly = 1,

        /// <summary>
        /// Select functionality
        /// </summary>
        SelectOnly = 2,

        /// <summary>
        /// Select, Insert and Delete functionality
        /// </summary>
        NoUpdate = 3
    }

    /// <summary>
    /// Connection string Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class), CLSCompliant(true)]
    public class DataAccessBehavior : Attribute, IAttributeValue<DataAccessBehaviors>
    {
        /// <summary>
        /// Value of attribute
        /// </summary>
        public DataAccessBehaviors Value { get; set; } = DataAccessBehaviors.AllAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value to hydrate</param>
        public DataAccessBehavior(DataAccessBehaviors value)
        {
            Value = value;
        }
    }
}
