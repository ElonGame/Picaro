﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LibraryProject
{
    public abstract class WorldObject : ContentObject
    {
        #region Description

        /// <summary>
        /// The name of the object.
        /// </summary>
        private string name;

        /// <summary>
        /// The name of the object.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        //#region Content Type Reader

        ///// <summary>
        ///// Read a WorldObject object from the content pipeline.
        ///// </summary>
        //public class WorldObjectReader : ContentTypeReader<WorldObject>
        //{
        //    /// <summary>
        //    /// Read a WorldObject object from the content pipeline.
        //    /// </summary>
        //    protected override WorldObject Read(ContentReader input, WorldObject existingInstance)
        //    {
        //        // we cannot create this object, so there must be an existing instance
        //        if (existingInstance == null)
        //        {
        //            throw new ArgumentNullException("existingInstance");
        //        }

        //        existingInstance.AssetName = input.AssetName;
        //        existingInstance.Name = input.ReadString();

        //        return existingInstance;
        //    }
        //}

        //#endregion
    }
}
