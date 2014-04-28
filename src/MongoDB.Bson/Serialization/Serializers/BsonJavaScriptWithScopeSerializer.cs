﻿/* Copyright 2010-2014 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.IO;
using MongoDB.Bson.IO;

namespace MongoDB.Bson.Serialization.Serializers
{
    /// <summary>
    /// Represents a serializer for BsonJavaScriptWithScopes.
    /// </summary>
    public class BsonJavaScriptWithScopeSerializer : BsonBaseSerializer<BsonJavaScriptWithScope>
    {
        // private static fields
        private static BsonJavaScriptWithScopeSerializer __instance = new BsonJavaScriptWithScopeSerializer();

        // constructors
        /// <summary>
        /// Initializes a new instance of the BsonJavaScriptWithScopeSerializer class.
        /// </summary>
        public BsonJavaScriptWithScopeSerializer()
        {
        }

        // public static properties
        /// <summary>
        /// Gets an instance of the BsonJavaScriptWithScopeSerializer class.
        /// </summary>
        public static BsonJavaScriptWithScopeSerializer Instance
        {
            get { return __instance; }
        }

        // public methods
        /// <summary>
        /// Deserializes a value.
        /// </summary>
        /// <param name="context">The deserialization context.</param>
        /// <returns>An object.</returns>
        public override BsonJavaScriptWithScope Deserialize(BsonDeserializationContext context)
        {
            var bsonReader = context.Reader;

            var bsonType = bsonReader.GetCurrentBsonType();
            switch (bsonType)
            {
                case BsonType.JavaScriptWithScope:
                    var code = bsonReader.ReadJavaScriptWithScope();
                    var scope = context.DeserializeWithChildContext(BsonDocumentSerializer.Instance);
                    return new BsonJavaScriptWithScope(code, scope);

                default:
                    var message = string.Format("Cannot deserialize BsonJavaScriptWithScope from BsonType {0}.", bsonType);
                    throw new FileFormatException(message);
            }
        }

        /// <summary>
        /// Serializes a value.
        /// </summary>
        /// <param name="context">The serialization context.</param>
        /// <param name="value">The object.</param>
        public override void Serialize(BsonSerializationContext context, BsonJavaScriptWithScope value)
        {
            var bsonWriter = context.Writer;

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var script = (BsonJavaScriptWithScope)value;
            bsonWriter.WriteJavaScriptWithScope(script.Code);
            context.SerializeWithChildContext(BsonDocumentSerializer.Instance, script.Scope);
        }
    }
}