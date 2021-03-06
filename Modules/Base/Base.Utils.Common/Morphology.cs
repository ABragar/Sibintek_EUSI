﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Utils.Common
{
    public class MorphologyHelper
    {
        private const string STOP_WORDS = "ах:без:бож:бол:больш:брат:буд:будет:будеш:будт:будут:будьт:буедт:бфть:бы:быв:был:быстр:быт:вам:вас:ваш:вдол:вдруг:вед:верн:ве:весьм:взял:видел:видн:виж:вмест:вне:вниз:внутр:во:вовс:вокруг:вот:вперед:впроч:времен:врем:все:всг:всегд:всег:всем:всех:всю:вся:вчер:вы:вышел:где:глаз:го:говор:да:дава:давн:даж:два:две:двер:двух:дел:дела:денег:ден::деньг:для:дни:дня:дням:до:довольн:долг:долж:должн::дом:достаточн:дык:дяд:дял:ев:ег:едв:ем:есл:ест:ещ:же:жизн:жит:ах:без:бож:бол:больш:брат:буд:будет:будеш:будт:будут:будьт:буедт:бфть:бы:быв:был:быстр:быт:вам:вас:ваш:вдол:вдруг:вед:верн:ве:весьм:взял:видел:видн:виж:вмест:вне:вниз:внутр:во:вовс:вокруг:вот:вперед:впроч:времен:врем:все:всг:всегд:всег:всем:всех:всю:вся:вчер:вы:вышел:где:глаз:го:говор:да:дава:давн:даж:два:две:двер:двух:дел:дела:денег:ден:деньг:для:дни:дня:дням:до:довольн:долг:долж:должн:дом:достаточн:дык:дяд:дял:ев:ег:едв:ем:есл:ест:ещ:же:жизн:жит:за:завтр:замет:зач:зде:знает:знаеш:знал:знат:знач:зна:иб:ива:иванович:иваныч:из:ил:им:имен:имет:имх:иногд:их:ка:кажд:кажет:каза:как:какж:кем:княз:ко:когд:ког:ком:комнат:конечн:коотр:котор:которй:крайн:кром:кто:куд:лет:ли:либ:лиц:лиш:лучш:любв:любл:любов:люд:мал:мат:мегалол:межд:мен:мер:мест:минут:мит:мля:мне:мног:мно:мо:мог:могл:могут:мож:может:можеш:можн:молод:моч:мы:мысл:нем:на:навсегд:над:назад:наконец:нам:например:нарочн:нас:нах:нача:наш:не:нег:нельз:немн:непремен:нескольк:несмотр:нет:неужел:неч:ни:нибуд:никак:никогд:никт:ним:них:нич:но:ног:ноч:ну:нужн:нэ:об:облом:образ:один:одн:однак:окол:омч:он:опя:особен:от:ответ:отвеча:отец:отч:очен:перв:перед:петр:письм:по:под:подума:пожал:позвольт:пок:помн:понима:пор:посл:пот:поч:почт:пошел:пр:правд:прав:пред:прежд:при:пришел:про:проговор:продолжа:прост:прям:пуст:пят:раз:разв:разумеет:рубл:рук:сам:свет:свйо:сво:сдела:себ:сегодн:сейчас:сердц:сих:скаж:сказа:скольк:скор:слишк:слов:случа:смотрел:снов:со:соб:совершен:совс:спрос:стал:старик:сто:сторон:стоя:сут:сюд:та:тагд:так:такж:такй:там:те:теб:тем:тепер:тех:тих:то:тоб:тогд:тог:тож:тольк:том:тот:тотчас:точн:тоьлк:три:ту:тут:ты:уж:ужасн:ум:упс:ух:хорош:хот:хотел:хоч:чем:час:част:чег:че:через:чт:что:чтоб:чут:чье:чья:шта:ыбт:э:эт:этот";

        public static string[] SearchString(string str)
        {
            if (String.IsNullOrEmpty(str)) return new string[] { };

            List<string> list = new List<string>();

            var stopWords = STOP_WORDS.Split(':');

            string gl = "ауоыиэяюёеьъ";

            foreach (string word in str.Trim().Split(' '))
            {
                string resW = word;

                char[] arrCh = word.ToCharArray();

                int length = 0;

                for (int i = arrCh.Length - 1; i >= 0; i--)
                {
                    char ch = arrCh[i];

                    if (gl.Contains(ch))
                    {
                        length = i;
                    }
                    else
                    {
                        break;
                    }
                }

                if (length > 0)
                {
                    resW = word.Substring(0, length);
                }

                if (stopWords != null)
                {
                    if (stopWords.Contains(resW))
                    {
                        continue;
                    }
                }

                list.Add(resW);
            }

            return list.ToArray();
        }
    }
}
