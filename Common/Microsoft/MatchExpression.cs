using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
// ReSharper disable IdentifierTypo

namespace Sphyrnidae.Common.Microsoft
{
    public class MatchExpression
    {
        public List<Regex> Regexes { get; set; }

        public Action<Match, object> Action { get; set; }
    }
}