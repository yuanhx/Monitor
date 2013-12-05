using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace SDP.Util
{
    public class PatternMap
    {
        public string p1;
        public string p2;

        public PatternMap(string p1, string p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }

    public class RegexUtil
    {
        private static IList<PatternMap> m_patterns = new List<PatternMap>();

        private static void InitPatternMap()
        {
            m_patterns.Clear();

            m_patterns.Add(new PatternMap(" to_char\\((.*?),( *)'yyyy-mm-dd hh24:mi:ss'\\) ", " convert\\(varchar\\(19\\),$1,120\\) "));
            m_patterns.Add(new PatternMap(" to_char\\((.*?),( *)'yyyy-mm-dd'\\) ", " convert\\(varchar\\(10\\),$1,120\\) "));
            m_patterns.Add(new PatternMap(" to_char\\((.*?)\\) ", " $1 "));
            m_patterns.Add(new PatternMap("\\|\\|", "\\+"));
            m_patterns.Add(new PatternMap("sysdate", "getdate()"));
            m_patterns.Add(new PatternMap("from dual", ""));
            m_patterns.Add(new PatternMap("'", "''"));
        }

        public static string OracleToSqlServerReplaceForSql(string data)
        {
            if (m_patterns.Count<=0)
                InitPatternMap();

            PatternMap pm;
            IEnumerator<PatternMap> it = m_patterns.GetEnumerator();
            while (it.MoveNext())
            {
                pm = it.Current;
                data = data.Replace(pm.p1, pm.p2);
            }
            return data;
        }

        public static string[] GetMatchingGroup(string patternstr, string data)
        {
            Regex regex = new Regex(patternstr);
            MatchCollection ms = regex.Matches(data);

            IList<string> list = new List<string>();

            IEnumerator it_match = ms.GetEnumerator();
            IEnumerator it_group;
            Match match;
            Group group;
            while (it_match.MoveNext())
            {
                match = it_match.Current as Match;
                if (match != null)
                {
                    it_group = match.Groups.GetEnumerator();
                    while (it_group.MoveNext())
                    {
                        group = it_group.Current as Group;
                        if (group != null)
                        {
                            list.Add(group.Value);
                        }
                    }
                }
            }

            return StrUtil.ToArray(list);
        }

        public static string[] GetMatchingStr(string patternstr, string data)
        {
            Regex regex = new Regex(patternstr);

            MatchCollection ms = regex.Matches(data);

            IList<string> list = new List<string>();

            IEnumerator it_match = ms.GetEnumerator();
            Match match;
            while (it_match.MoveNext())
            {
                match = it_match.Current as Match;
                if (match != null)
                {
                    list.Add(match.Value);
                }
            }

            return StrUtil.ToArray(list);
        }

        public static bool IsInteger(string data)
        {
            string[] flag = GetMatchingStr("^[-|+]?\\d+$", data);
            if (flag != null && flag.Length == 1)
                return true;
            else return false;
        }

        public static bool IsNumber(string data)
        {
            Regex regex = new Regex("^[-|+]?[\\d]+[\\.\\d]?$");
            return regex.IsMatch(data);

            //string[] flag = GetMatchingStr("^[-|+]?[\\d]+[\\.\\d]?$", data);
            //if (flag != null && flag.Length == 1)
            //    return true;
            //else return false;
        }
    }
}
