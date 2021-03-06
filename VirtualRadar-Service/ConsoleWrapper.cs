﻿// Copyright © 2017 onwards, Andrew Whewell
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceFactory;
using VirtualRadar.Interface;
using VirtualRadar.Interface.Settings;

namespace VirtualRadar
{
    class ConsoleWrapper : IConsole
    {
        private const string LogFileName = "ServiceMessages.txt";
        private static string LogFullPath;

        private static ConsoleWrapper _Singleton;
        public IConsole Singleton
        {
            get {
                if(_Singleton == null) {
                    _Singleton = new ConsoleWrapper();

                    var folder = Factory.Singleton.Resolve<IConfigurationStorage>().Singleton.Folder;
                    LogFullPath = Path.Combine(folder, LogFileName);

                    if(!Directory.Exists(folder)) {
                        Directory.CreateDirectory(folder);
                    }

                    File.WriteAllLines(LogFullPath, new string[] {
                        String.Format("Service started at {0:yyyy-MM-dd HH:mm:ss.fff} (UTC)", DateTime.UtcNow),
                    });
                }
                return _Singleton;
            }
        }

        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Gray;

        public bool KeyAvailable
        {
            get { return false; }
        }

        public void Beep()
        {
            ;
        }

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            return new ConsoleKeyInfo();
        }

        public void Write(string message)
        {
            File.AppendAllText(LogFullPath, message);
        }

        public void Write(char value)
        {
            Write(new string(value, 1));
        }

        public void WriteLine()
        {
            WriteLine("");
        }

        public void WriteLine(string message)
        {
            File.AppendAllLines(LogFullPath, new string[] { message });
        }

        public void WriteLine(string format, params object[] args)
        {
            WriteLine(String.Format(format, args));
        }
    }
}
