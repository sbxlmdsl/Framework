//-----------------------------------------------------------------------
// <copyright file="ICode.cs" company="Genesys Source">
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
    /// Code, short character-based identifier of a record. 
    ///  Not always ISO codes, sometimes custom or a legacy systems identifier
    /// </summary>    
    [CLSCompliant(true)]
    public interface ICode
    {
        /// <summary>
        /// Code
        /// </summary>
        string Code { get; set; }
    }
}
