using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace SFO2O.Utility.Uitl
{
    public static class BanWordHelper
    {
        /// <summary>
        /// 获取敏感词列表的方法委托
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public delegate string[] ReadBanWord(object state);

        private static ReadBanWord _readBanWord;
        /// <summary>
        /// 字典更新
        /// </summary>
        private static Timer timer = null;


        /// <summary>
        /// 更新字典方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArgs"></param>
        private static void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_readBanWord != null)
            {
                var arr = _readBanWord(sender);
                InitDictionary(arr, Use_SkipCharList);
            }
        }
        /// <summary>
        /// 内部的缓冲字典
        /// </summary>
        private static Dictionary<char, IList<string>> keyDict = null;

        private static Dictionary<char, int> skipChars = null;
        /// <summary>
        /// 字典是否初始化
        /// </summary>
        public static bool InitDictionaryed
        {
            get { return keyDict != null; }
        }

        /// <summary>
        /// 是否已经载入
        /// </summary>
        public static bool DictionaryLoaded = false;

        /// <summary>
        /// 系统缺省的中止词表
        /// </summary>
        private const string Default_SkipCharList = "Xx*-_+()%#@$!&^[]\"':;.,|=~`/\\ ";
        /// <summary>
        /// 用户定义的中止词表
        /// </summary>
        private static string Use_SkipCharList = "Xx*-_+()%#@$!&^[]\"':;.,|=~`/\\ ";

        private static Dictionary<char, int> GetSkipChars(string skip_char_list)
        {
            Dictionary<char, int> dict_skip_char = new Dictionary<char, int>();
            if (string.IsNullOrWhiteSpace(skip_char_list))
                return dict_skip_char;

            char[] s_array = skip_char_list.ToCharArray();

            for (int i = 0; i < s_array.Length; i++)
            {
                char c = s_array[i];
                if (dict_skip_char.ContainsKey(c))
                    continue;
                else
                    dict_skip_char.Add(c, 0);
            }

            return dict_skip_char;
        }

        /// <summary>
        /// 匹配一个字符串,返回true表示包含关键字,false表示不匹配，只要匹配上任意一个关键字就返回.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool Match(string text)
        {
            return Match(text, null);
        }

        /// <summary>
        /// 匹配一个字符串，返回true表示包含关键字，false表示不包含，同时将匹配的关键字放在matched_keywords_list里
        /// </summary>
        /// <param name="text"></param>
        /// <param name="matched_keywords_list"></param>
        /// <returns></returns>
        public static bool Match(string text, List<string> matched_keywords_set)
        {
            return Match(text, matched_keywords_set, null);
        }


        /// <summary>
        /// 匹配一个字符串，返回true表示包含关键字，false表示不包含，同时将匹配的关键字放在matched_keywords_list里
        /// 同时将所有的关键词用*号替代
        /// </summary>
        /// <param name="text"></param>
        /// <param name="matched_keywords_list"></param>
        /// <returns></returns>
        public static bool Match(string text, List<string> matched_keywords_set, StringBuilder filtered_text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            int len = text.Length;

            bool matched_keyword = false;

            for (int i = 0; i < len; i++)
            {
                //不可能是关键字，那就向前走
                if (!keyDict.ContainsKey(text[i]))
                {
                    if (filtered_text != null)
                        filtered_text.Append(text[i]);

                    continue;
                }

                bool matched_this_keyword = false;

                //匹配上一个字，那就看看这个字能不能找到关键词
                foreach (string s in keyDict[text[i]])
                {
                    matched_this_keyword = true;

                    int j = i;
                    foreach (char c in s)
                    {
                        while (j < len)
                            if (skipChars.ContainsKey(text[j]))
                                j++;
                            else
                                break;

                        if (j >= len || c != text[j++])
                        {
                            matched_this_keyword = false;
                            break;
                        }
                    }

                    //没匹配上这个关键字.
                    if (!matched_this_keyword)
                        continue;

                    matched_keyword = true;

                    i += (j - i) - 1; //跳过这个词

                    if (filtered_text == null && matched_keywords_set == null)
                        return true;  //都是空？那直接返回false好了

                    if (filtered_text != null)
                        filtered_text.Append('*', s.Length);

                    if (matched_keywords_set != null)
                        matched_keywords_set.Add(s);

                    break;
                }

                if (!matched_this_keyword && filtered_text != null)
                    filtered_text.Append(text[i]);
            }

            return matched_keyword;
        }


        /// <summary>
        /// 载入一个字典，使用系统缺省的扰乱字符集
        /// </summary>
        /// <param name="readBanWord">更新敏感词字典的方法</param>
        /// <param name="interval">更新频率（单位：分钟）</param>
        /// <returns></returns>
        public static void InitDictionary(ReadBanWord readBanWord, int interval)
        {
            InitDictionary(readBanWord, Default_SkipCharList, interval);
        }

        /// <summary>
        /// 载入一个字典，使用自定义的扰乱字符集
        /// </summary>
        /// <param name="readBanWord">更新敏感词字典的方法</param>
        /// <param name="skip_chars">中止词表 如：“Xx*-_+()%#@$!&^[]\"':;.,|=~`/\\ ” </param>
        /// <param name="interval">更新频率（单位：分钟）</param>
        /// <returns></returns>
        public static void InitDictionary(ReadBanWord readBanWord, string skip_chars, int interval)
        {
            Use_SkipCharList = skip_chars;
            _readBanWord = readBanWord;
            TimerOnElapsed(null, null);
            timer = new Timer();
            timer.Interval = interval * 60000;
            timer.Elapsed += TimerOnElapsed;
            timer.Start();
        }



        /// <summary>
        /// 根据输入的字符串，生成一个字典表，返回字典的首字数目
        /// </summary>
        /// <param name="strList"></param>
        /// <returns></returns>
        private static int InitDictionary(string[] strList, string skip_chars)
        {
            skipChars = GetSkipChars(skip_chars);

            if (strList == null || strList.Length == 0)
                return 0;

            Dictionary<char, IList<string>> temp_dict = new Dictionary<char, IList<string>>(strList.Length / 4);

            int counter = 0;
            foreach (string s in strList)
                counter += InsertToDictionary(temp_dict, s);

            keyDict = temp_dict;

            DictionaryLoaded = true;

            return counter;
        }


        /// <summary>
        /// 向某个库里插入个词
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private static int InsertToDictionary(Dictionary<char, IList<string>> dict, string s)
        {
            if (s == null)
                return 0;

            string strim = s.Replace(" ", "");

            if (strim == "")
                return 0;

            if (dict.ContainsKey(strim[0]))
                dict[strim[0]].Add(strim);
            else
                dict.Add(strim[0], new List<string> { strim });

            return 1;
        }
    }




}
