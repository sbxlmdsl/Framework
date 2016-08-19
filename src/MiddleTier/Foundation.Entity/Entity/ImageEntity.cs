//-----------------------------------------------------------------------
// <copyright file="ImageBase.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Genesys.Extensions;
using Genesys.Extras.Mathematics;
using Genesys.Foundation.Entity;

namespace Genesys.Foundation.Entity
{
    /// <summary>
    /// Base DAO class for tables containing image column
    /// Separates the heavy image column into its own object
    /// </summary>
    
    [CLSCompliant(true)]
    public abstract class ImageEntity<TEntity> : SaveableEntity<TEntity>, IBytesKey where TEntity : ImageEntity<TEntity>, new()
    {
        /// <summary>
        /// 1x1px transparent image
        /// </summary>
        public static Bitmap ImageEmpty { get; set; } = new Bitmap(0, 0);
        
        /// <summary>
        /// 1x1px transparent image
        /// </summary>
        public static Image NoImage
        {
            get { return Genesys.Foundation.Entity.Properties.Resources.EmptyTransparent; }
        }
        /// <summary>
        /// Byte array with data
        /// </summary>
        public virtual byte[] Bytes { get; set; }

        /// <summary>
        /// Image of the Bytes data
        /// </summary>
        public Image Image
        {
            get { return this.Bytes.ToImage(); }
        }

        /// <summary>
        /// Gets dynamic content type of the Bytes data
        /// </summary>
        public string ContentType
        {
            get
            {
                return this.Image.RawFormat.ToContentType();
            }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ImageEntity()
            : base()
        {
            this.Initialize<ImageEntity<TEntity>>();
            this.Bytes = ImageEntity<TEntity>.NoImage.ToBytes();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ImageBytes">Data to save</param>
        public ImageEntity(byte[] ImageBytes)
            : this()
        {
            this.Bytes = ImageBytes;
        }
        
        /// <summary>
        /// Sizes based on height only, will use multiplier to calculate proper width
        /// </summary>
        /// <param name="newHeight">New height (width is auto)</param>
        
        public TEntity ToSize(int newHeight)
        {            
            TEntity returnValue = new TEntity();
            int newWidth = TypeExtension.DefaultInteger;
            decimal multiplier = TypeExtension.DefaultDecimal;

            multiplier = Arithmetic.Divide(newHeight.ToDecimal(), this.Image.Height.ToDecimal());
            returnValue = this.ToSize(new Size(Arithmetic.Multiply(this.Image.Width, multiplier).ToInt(), newHeight));
            
            return returnValue;
        }

        /// <summary>
        /// Resizes an image and stretches if ratio is wrong
        /// </summary>
        /// <param name="newSize">New height and width</param>
        public TEntity ToSize(Size newSize)
        {
            TEntity returnValue = new TEntity();

            if ((this.Image == null == false) && (this.Image.Size.Width > 0 & this.Image.Size.Height > 0))
            {
                returnValue.Bytes = this.Image.Resize(newSize).ToBytes();
            }
            
            return returnValue;
        }
        
        /// <summary>
        /// Converts to a very lightweight thumbnail
        /// </summary>
        public TEntity ToThumbnail()
        {
            TEntity returnValue = new TEntity();
            Image newImage = this.Image;
            Image thumbnail = newImage.GetThumbnailImage(this.Image.Width, this.Image.Height, new Image.GetThumbnailImageAbort(EmptyCallBack), IntPtr.Zero);
            returnValue.Bytes = thumbnail.ToBytes();
            
            return returnValue;
        }

        /// <summary>
        /// Puts a thumbnail in upper left corner
        /// </summary>
        /// <param name="width">Width in px</param>
        /// <param name="height">Height in px</param>
        public TEntity ToThumbnailInUpperLeftCorner(int width, int height)
        {
            TEntity returnValue = new TEntity();
            Image thumbnail = new Bitmap(this.Image, this.Image.Width, this.Image.Height);
            Graphics graphicConvert = Graphics.FromImage(thumbnail);

            if ((this.Image == null == false) && (this.Image.Size.Width > 0 & this.Image.Size.Height > 0))
            {
                // Resize image
                graphicConvert.CompositingQuality = CompositingQuality.HighQuality;
                graphicConvert.SmoothingMode = SmoothingMode.HighQuality;
                graphicConvert.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Rectangle rectangleConvert = new Rectangle(0, 0, width, height);
                graphicConvert.DrawImage(this.Image, rectangleConvert);
                returnValue.Bytes = thumbnail.ToBytes();
            }
            
            return returnValue;
        }

        /// <summary>
        /// Required for ToThumbnail
        /// </summary>
        private bool EmptyCallBack()
        {
            return false;
        }
        
        /// <summary>
        /// Crops an x,y for the area of width, height
        /// </summary>
        /// <param name="width">Width in px</param>
        /// <param name="height">Height in px</param>
        /// <param name="x">Starting X</param>
        /// <param name="y">Starting Y</param>
        public Byte[] Crop(int width, int height, int x, int y)
        {
            return this.Image.Crop(width, height, x, y).ToBytes();
        }
    }
}
