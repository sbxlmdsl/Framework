//-----------------------------------------------------------------------
// <copyright file="ImageModel.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Genesys.Extensions;
using Genesys.Foundation.Name;

namespace Genesys.Foundation.Entity
{
	/// <summary>
	/// Common object across models and business entity
	/// </summary>
	/// <remarks></remarks>
	[CLSCompliant(true)]
	public class ImageModel : ModelEntity<ImageModel>, IBytesKey, IName
    {
        /// <summary>
        /// Name of this image
        /// </summary>
        public string Name { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// Bytes of BLOB image
        /// </summary>
        public byte[] Bytes { get; set; } = TypeExtension.DefaultBytes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public ImageModel() : base()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public ImageModel(byte[] bytes)
            : this()
        {
            Bytes = bytes;
        }
		
	}
}
