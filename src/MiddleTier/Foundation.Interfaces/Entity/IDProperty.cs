//-----------------------------------------------------------------------
// <copyright file="IDProperty.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extensions;


namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// Container for ID data transport and polymorphism
    /// </summary>
    [CLSCompliant(true)]
    public class IDProperty : IID
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int ID { get; set; } = TypeExtension.DefaultInteger;
    }
}

