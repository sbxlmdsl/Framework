//-----------------------------------------------------------------------
// <copyright file="DataConcurrencyValues.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Genesys.Foundation.Data
{
    /// <summary>
    /// enumeration to allow the attribute to use strongly-typed ID
    /// </summary>
    [CLSCompliant(true)]
    public enum DataConcurrencies
    {
        /// <summary>
        /// Forces clean read of committed data, will not read uncommitted data
        /// Will wait for commits to finish, can be blocked
        /// This entity will re-pull itself from the database after a save, insert then fully reselect to ensure data integrity
        /// </summary>
        Pessimistic = 0,

        /// <summary>
        /// allows dirty reads of uncommitted data
        /// Can not be blocked by other processes commits
        /// This entity will not re-pull itself from the database after a save call.
        /// After a save, any data changes in the data tier will not be reflected in the object
        /// </summary>
        Optimistic = 1,
    }

    /// <summary>
    /// Connection string Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class), CLSCompliant(true)]
    public class DataConcurrency : System.Attribute
    {
        /// <summary>
        /// Value of attribute
        /// </summary>
        public DataConcurrencies Value { get; set; } = DataConcurrencies.Pessimistic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value to hydrate</param>
        public DataConcurrency(DataConcurrencies value)
        {
            Value = value;
        }
    }
}
