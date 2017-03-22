﻿// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TrakHound.Api.v2;

namespace mod_rest_programs
{
    class Program
    {
        // SHA1 Hash of Name+Start+Stop
        public string Id { get { return GenerateId(); } }

        public string Name { get; set; }

        public bool Completed { get; set; }

        public DateTime Start { get; set; }

        public DateTime Stop { get; set; }

        public TimeSpan Duration
        {
            get
            {
                if (Start > DateTime.MinValue && Stop > DateTime.MinValue)
                {
                    return Stop - Start;
                }
                else return TimeSpan.Zero;
            }
        }

        public List<ProgramEvent> Events { get; set; }


        public Program()
        {
            Events = new List<ProgramEvent>();
        }

        private string GenerateId()
        {
            // Create Identifier input
            string s = string.Format("{0}|{1}|{2}", Name, Start.ToUnixTime(), Stop.ToUnixTime());
            s = Uri.EscapeDataString(s);

            // Create Hash
            var b = Encoding.UTF8.GetBytes(s);
            var h = SHA1.Create();
            b = h.ComputeHash(b);
            var l = b.ToList();
            l.Reverse();
            b = l.ToArray();

            // Convert to Base64 string
            s = Convert.ToBase64String(b);

            // Remove non alphanumeric characters
            var regex = new Regex("[^a-zA-Z0-9 -]");
            s = regex.Replace(s, "");
            s = s.ToUpper();

            return s;
        }
    }
}
