﻿//-----------------------------------------------------------------------
// <copyright file="ICreatedDate.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// Created date of the record
    /// </summary>    
    [CLSCompliant(true)]
    public interface ICreatedDate
    {
        /// <summary>
        /// CreatedDate
        /// </summary>
        DateTime CreatedDate { get; }        
    }
}
