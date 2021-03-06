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

using Elton.Nest.Models;
using Elton.Nest.Rest.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elton.Nest
{
    public class ValueAddedEventArgs : EventArgs
    {
        public ValueAddedEventArgs(string path, string data)
        {
            Path = path;
            Data = data;
        }

        public string Path { get; private set; }
        public string Data { get; private set; }
    }

    public class ValueChangedEventArgs : EventArgs
    {
        public ValueChangedEventArgs(string path, string data, string oldData)
        {
            Path = path;
            Data = data;
            OldData = oldData;
        }

        public string Path { get; private set; }
        public string Data { get; private set; }

        public string OldData { get; private set; }
    }

    public class ValueRemovedEventArgs : EventArgs
    {
        public ValueRemovedEventArgs(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }
    }
}
