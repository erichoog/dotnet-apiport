﻿using Microsoft.Fx.Portability.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiPortVS
{
    public class TargetPlatform : EqualityComparer<TargetPlatform>, IComparable<TargetPlatform>
    {
        private readonly ICollection<string> _alternativeNames = new HashSet<string>(StringComparer.Ordinal);

        public string Name { get; set; }

        public IEnumerable<TargetPlatformVersion> Versions { get; set; }

        public string DisplayName
        {
            get
            {
                if (AlternativeNames.Count > 0)
                {
                    return String.Format("{0} ({1})", Name, String.Join(", ", AlternativeNames));
                }
                else
                {
                    return Name;
                }
            }
        }

        public ICollection<string> AlternativeNames { get { return _alternativeNames; } }

        public TargetPlatform(IGrouping<string, AvailableTarget> targetInfo)
        {
            Name = targetInfo.Key;

            Versions = targetInfo
                .OrderBy(v => v.Version)
                .Select(v => new TargetPlatformVersion(this) { Version = v.Version, IsSelected = v.IsSet })
                .ToList();
        }

        public TargetPlatform() { }

        public TargetPlatform(TargetPlatform platform)
        {
            Name = platform.Name;
            Versions = platform.Versions.Select(v => new TargetPlatformVersion(v)).ToList();
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public override bool Equals(TargetPlatform x, TargetPlatform y)
        {
            return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)
                   && x.Versions.SequenceEqual(y.Versions);
        }

        public override int GetHashCode(TargetPlatform obj)
        {
            return obj.Name.GetHashCode();
        }

        public int CompareTo(TargetPlatform other)
        {
            if (other == null) return -1;

            return String.CompareOrdinal(Name, other.Name);
        }
    }
}