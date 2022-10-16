using HtmlAgilityPack;
using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BilligKwhWebApp.Core.Toolbox
{
    public static class StringExtensions
    {
        public static int IntTryParse(this string input, int valueIfNotConverted)
        {
            int value;
            if (Int32.TryParse(input, out value))
            {
                return value;
            }
            return valueIfNotConverted;
        }

        public static string SafeReplace(this string input, string find, string replace, bool matchWholeWord)
        {
            string textToFind = matchWholeWord ? string.Format(@"\b{0}\b", find) : find;
            return Regex.Replace(input, textToFind, replace);
        }

        /// <summary>
        /// Modifies string by inserting spaces between each character
        /// </summary>
        public static string AddSpacesBetweenCharacters(this string input)
        {
            StringBuilder spacedString = new StringBuilder(input[0] + " ");
            for (int i = 1; i < input.Length; i++)
            {
                spacedString.Append(input[i] + " ");
            }
            return spacedString.ToString().Trim();
        }

        public static string NewLinesToBr(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.Replace(Environment.NewLine, "<br />", StringComparison.Ordinal);
        }

        /// <summary>
        /// Cuts away any HTML in a string. Handles both tabs and new lines by changing to spaces and empty lines, respectively.
        /// </summary>
        /// <returns>String without HTML.</returns>
        public static string HtmlToPlainText(this string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return "";
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var sb = new StringBuilder();
            foreach (var node in htmlDoc.DocumentNode.ChildNodes)
            {
                VisitHtmlNode(node, sb);
            }

            return System.Net.WebUtility.HtmlDecode(sb.Replace("&nbsp;", " ").ToString());
        }

        private static readonly string[] tagsThatCauseLineBreak = { "H1", "H2", "H3", "H4", "H5", "H6", "P", "DIV", "LI", "BR", "TR" };
        private static readonly string[] tagsToIgnoreContent = { "HEAD", "SCRIPT", "STYLE" };

        private static void VisitHtmlNode(HtmlNode node, StringBuilder output)
        {
            switch (node.NodeType)
            {
                case HtmlNodeType.Element:
                    if (tagsToIgnoreContent.Contains(node.Name.ToUpperInvariant()))
                    {
                        return;
                    }
                    foreach (var child in node.ChildNodes)
                    {
                        VisitHtmlNode(child, output);
                    }
                    if (tagsThatCauseLineBreak.Contains(node.Name.ToUpperInvariant()))
                    {
                        output.AppendLine();
                    }
                    break;
                case HtmlNodeType.Text:
                    output.Append(node.InnerText);
                    break;
            }
        }
    }

    public static class StringHelper
    {
        readonly static NumberFormatInfo dotDecimalFormat = new NumberFormatInfo { NumberDecimalSeparator = "." };

        public static string ToTitleCase(this string str)
        {
            var tokens = str.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = token.Substring(0, 1).ToUpper() + token.Substring(1).ToLower();
            }

            return string.Join(" ", tokens);
        }

        public static string PrepareFullTextSearch(string search)
        {
            string[] tmpSplit = search.Split(' ');

            string newSeach = "";

            foreach (string s in tmpSplit)
            {
                newSeach = newSeach + "\"" + s + "*\"&";
            }

            return newSeach.TrimEnd('&').Replace("&", " AND ");

        }

        public static string LatLngToString(double? lat, double? lng)
        {
            return string.Format(dotDecimalFormat, "{0}, {1}", lat, lng);
        }

        public static bool TryParseLatLng(string latlng, out double lat, out double lng)
        {
            lat = lng = 0.0;
            var sLatLng = latlng.Split(',');

            return (double.TryParse(sLatLng[0].Trim(), NumberStyles.Number, dotDecimalFormat, out lat) && double.TryParse(sLatLng[1].Trim(), NumberStyles.Number, dotDecimalFormat, out lng));
        }
    }

    public static class DateTimeHelper
    {

        /// <summary>
        ///  Converts a JavaScript Date object's time representation in ms into the correct DateTime in C#. 3 important things are taken into account: 
        ///  1. JavaScript's time origin starts at UNIX epoc being midnight 1st Jan 1970 while .NET's time origin starts at midnight 1st Jan 0001 
        ///  2. One .NET "tick" is NOT a millisecond but 100 nano seconds (= 10000 ms)
        ///  3. There could be an offset difference in time depending on the browser's location so this should be removed so it corresponds to the server's offset
        /// A bit more info here: https://stackoverflow.com/a/7966778 
        /// </summary>
        /// <param name="jsMs">The milliseconds returned by js Date object's getTime() function</param>
        /// <returns>.NET DateTime corresponding to the same date in javascript</returns>
        public static DateTime JsDateMilliSeondsToDateTime(long jsMs)
        {
            var dateWithOffset = new DateTime((jsMs * 10000) + 621355968000000000);
            var dof = new DateTimeOffset(dateWithOffset, new TimeSpan(0));

            return dof.ToLocalTime().DateTime;
        }

        public static DateTimeOffset SetTime(this DateTimeOffset dt, TimeSpan timeToSet)
        {
            return dt.Date + timeToSet;
        }

        /// <summary>
        /// Rounds datetime up, down or to nearest minutes and all smaller units to zero.
        /// COPIED FROM HERE: http://metadataconsulting.blogspot.com/2018/10/C-Round-Datetime-Extension-To-Nearest-Minute-And-Smaller-Units-Are-Rounded-To-Zero.html
        /// </summary>
        /// <param name="dt">static extension method</param>
        /// <param name="rndmin">mins to round to</param>
        /// <param name="directn">Up,Down,Nearest</param>
        /// <returns>rounded datetime with all smaller units than mins rounded off</returns>
        public static DateTime RoundToNearestMinute(this DateTime dt, int rndmin, RoundingDirection directn)
        {
            if (rndmin == 0) //can be > 60 mins
                return dt;

            TimeSpan d = TimeSpan.FromMinutes(rndmin); //this can be passed as a parameter, or use any timespan unit FromDays, FromHours, etc.		

            long delta = 0;
            Int64 modTicks = dt.Ticks % d.Ticks;

            switch (directn)
            {
                case RoundingDirection.Up:
                    delta = modTicks != 0 ? d.Ticks - modTicks : 0;
                    break;
                case RoundingDirection.Down:
                    delta = -modTicks;
                    break;
                case RoundingDirection.Nearest:
                    {
                        bool roundUp = modTicks > (d.Ticks / 2);
                        var offset = roundUp ? d.Ticks : 0;
                        delta = offset - modTicks;
                        break;
                    }

            }
            return new DateTime(dt.Ticks + delta, dt.Kind);
        }

        public enum RoundingDirection
        {
            Up,
            Down,
            Nearest
        }
    }
}
