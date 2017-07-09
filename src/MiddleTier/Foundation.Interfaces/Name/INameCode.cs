//-----------------------------------------------------------------------
// <copyright file="INameCode.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Foundation.Entity;

namespace Genesys.Foundation.Name
{
    /// <summary>
    /// Name and code
    ///  Such as an ISO code and a friendly name combination
    /// </summary>
    [CLSCompliant(true)]
    public interface INameCode : ICode, IName
    {
    }
}
