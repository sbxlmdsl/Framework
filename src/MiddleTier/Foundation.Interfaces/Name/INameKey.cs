//-----------------------------------------------------------------------
// <copyright file="INameKey.cs" company="Genesys Source">
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
    /// Name and ID, used in all lookup records/classes
    /// </summary>
    [CLSCompliant(true)]
    public interface INameKey : IKey, IName
    {
    }
}
