﻿#region License

//   Copyright 2018 Elton FAN (eltonfan@live.cn, http://elton.io)
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 

#endregion

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Elton.Nest.Models
{
    public class OrderedContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// JsonPropertyOrder(alphabetic = true)
        /// </summary>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
        }
        /// <summary>
        /// JsonIgnoreProperties(ignoreUnknown = true)
        /// </summary>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            property.NullValueHandling = NullValueHandling.Include;
            property.DefaultValueHandling = DefaultValueHandling.Include;
            property.ShouldSerialize = o => true;
            return property;
        }
    }

    /// <summary>
    /// Provides utilities methods for various common operations within this library.
    /// </summary>
    public static class Utils
    {
        static readonly Common.Logging.ILog log = Common.Logging.LogManager.GetLogger(typeof(Utils));

        static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new OrderedContractResolver(),
        };
        
        /// <summary>
        /// Returns the object in a JSON string representation if possible. If this fails, it will return
        /// the superclass' string representation of the object.
        /// </summary>
        /// <param name="obj">the object to convert.</param>
        /// <value">a string representation of the object.</value>
        public static string ToString(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, serializerSettings);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return obj?.ToString();
            }
        }

        /// <summary>
        /// Returns whether any of the provided Strings are empty (null or zero-length).
        /// </summary>
        /// <param name="args">strings to check for emptiness.</param>
        /// <value">true if any of the provided strings is null or is zero-length, false otherwise.</value>
        public static bool IsAnyEmpty(params string[] args)
        {
            if (args == null)
            {
                return false;
            }

            foreach (var s in args)
            {
                if (string.IsNullOrEmpty(s))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AreEqual<T>(T left, T right)
        {
            if (object.Equals(left, null) || object.Equals(right, null))
                return object.Equals(left, right);

            if (object.ReferenceEquals(left, right))
                return true;

            foreach (var property in typeof(T).GetProperties())
            {
                if (property.GetValue(left) != property.GetValue(right))
                    return false;
            }

            return true;
        }

        public static int GetHashCode(object obj)
        {
            if (object.Equals(obj, null))
                return 0;
            int hashCode = 41;
            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj);
                if (value != null)
                    hashCode = hashCode * 59 + value.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Builds a path incrementally.
        /// </summary>
        public class PathBuilder
        {
            readonly StringBuilder builder;

            /// <summary>
            /// Creates a new PathBuilder.
            /// </summary>
            public PathBuilder()
            {
                builder = new StringBuilder();
            }

            /// <summary>
            /// Appends a string to the path.
            /// </summary>
            /// <param name="entry">string to append to the path.</param>
            /// <value">the PathBuilder. Allows for chaining multiple appends.</value>
            public PathBuilder Append(string entry)
            {
                builder.Append("/").Append(entry);
                return this;
            }

            /// <summary>
            /// Returns the resulting path.
            /// </summary>
            /// <returns>the resulting path.</returns>
            public string Build()
            {
                return builder.ToString();
            }
        }
    }
}