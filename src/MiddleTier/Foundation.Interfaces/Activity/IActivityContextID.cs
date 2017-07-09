//-----------------------------------------------------------------------
// <copyright file="IActivityContextID.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Genesys.Foundation.Activity
{
    /// <summary>
    /// Activity that tracks any interaction with the framework
    /// Particularly CRUD and Workflow operations.
    /// </summary>
	[CLSCompliant(true)]
    public interface IActivityContextID 
    {
        /// <summary>
        /// ID of the activity that tracks a transaction type process, typically querying or committing data
        /// </summary>
        int ActivityContextID { get; set; }
	}
}
