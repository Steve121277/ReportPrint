using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportPrint.Model.Statistics
{
    internal class StatisticItem
    {
        internal User UserInfo { get; set; }
        internal float[,] Values { get; set; } = new float[5, 12];
        internal int[] CaptionNo = new int[12];
        internal int FirstMonth { get; set; }
        internal string Notes;

        //Calculate statistics for print
        internal static StatisticItem Calc(User user, DateTime calcDate, string notes)
        {
            StatisticItem sitem = new StatisticItem() { UserInfo = user, Notes = notes };
            IEnumerable<IUserData> UserDatas = Model.ModelManager.GetUserDatas(user.ID);

            int year = calcDate.Year;
            int month = calcDate.Month;
            int cnt = 0;

            DateTime BegTime = new DateTime(year, month, 1);

            sitem.FirstMonth = month;

            while (cnt < 12)
            {
                DateTime EndTime = BegTime.AddMonths(1);

                for (int i = 0; i <= (int)GameType.CarePitLog; i++)
                {
                    sitem.Values[i, cnt] = Single.NaN;
                }

                IEnumerable<IUserData> findUserDatas = UserDatas.Where(u => u.MeasureTime >= BegTime && u.MeasureTime < EndTime).OrderBy(d => d.MeasureTime);

                foreach (IUserData userData in findUserDatas)
                {
                    int index = -1;

                    switch (userData.GameType)
                    {
                        case GameType.All_ashiage:
                            {
                                UserDataAll userDataAll = (UserDataAll)userData;

                                index = (int)(userDataAll.IsLeft ? GameType.All_ashiage_left : GameType.All_ashiage_right);
                            }
                            break;
                        case GameType.All_ssfive:
                            index = (int)GameType.All_ssfive;
                            break;
                        case GameType.CarePitLog:
                            index = (int)GameType.CarePitLog;
                            break;
                        case GameType.TUG:
                            index = (int)GameType.TUG;
                            break;
                    }

                    if (index >= 0)
                    {
                        sitem.Values[index, cnt] = userData.GaneScore;
                    }
                }

                cnt++;
                BegTime = BegTime.AddMonths(-1);
            }

            return sitem;
        }
    }
}
