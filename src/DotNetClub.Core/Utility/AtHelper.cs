using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotNetClub.Core.Utility
{
    public sealed class AtHelper
    {
        public static List<string> FetchUsers(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new List<string>();
            }

            string[] regexList = new string[]
            {
                "```.+?```/g", // 去除单行的 ```
                @"^```[\s\S] +?^```/gm", // ``` 里面的是 pre 标签内容
                @"`[\s\S]+?`/g", // 同一行中，`some code` 中内容也不该被解析
                @"^    .*/gm", // 4个空格也是 pre 标签，在这里 . 不会匹配换行
                @"\b\S*?@[^\s]*?\..+?\b/g", // somebody@gmail.com 会被去除
                @"\[@.+?\]\(\/.+?\)/g", // 已经被 link 的 username
            };

            foreach (var regexStr in regexList)
            {
                var regex = new Regex(regexStr);
                text = regex.Replace(text, "");
            }

            var resultRegex = new Regex(@"@[a-z0-9\-_]+\b");
            var matches = resultRegex.Matches(text);

            List<string> result = new List<string>();

            foreach (Match match in matches)
            {
                result.Add(match.Value.Substring(1));
            }

            return result.Distinct().ToList();
        }

        public static string LinkUsers(string text)
        {
            var users = FetchUsers(text);

            foreach(var user in users)
            {
                var regex = new Regex($"@{user}\\b(?!\\])");

                bool match = regex.IsMatch(text);

                text = regex.Replace(text, $"[@{user}](/user/{user})");
            }

            return text;
        }
    }
}
