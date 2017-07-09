//-----------------------------------------------------------------------
// <copyright file="TakeRows.cs" company="Genesys Source">
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
    /// Take Top Rows Attribute
    ///  Default: 100 rows
    /// </summary>
    [AttributeUsage(AttributeTargets.Class), CLSCompliant(true)]
    public class TakeRows : Attribute, IAttributeValue<int>
    {
        /// <summary>
        /// Value of attribute
        /// </summary>
        public int Value { get; set; } = 100;

        /// <summary>
        /// Number of rows to take top from query
        ///  Default: 100 rows
        /// </summary>
        /// <param name="value">number of rows to take</param>
        public TakeRows(int value)
        {
            Value = value;
        }
    }
}
