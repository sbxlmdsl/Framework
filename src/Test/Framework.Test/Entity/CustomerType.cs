//-----------------------------------------------------------------------
// <copyright file="CustomerType.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Framework.Data;
using Genesys.Framework.Entity;

namespace Genesys.Framework.Test
{
    /// <summary>
    /// Tests attributes        
    /// </summary>
    [ConnectionStringName("DefaultConnection")]
    public partial class CustomerType : CrudEntity<CustomerType>
    {
    }
}
