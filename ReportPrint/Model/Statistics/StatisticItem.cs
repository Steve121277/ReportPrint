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

        //Calculate statistics
        internal static StatisticItem Calc(User User, DateTime CalcDate)
        {
            StatisticItem sitem = new StatisticItem() { UserInfo = User };
            IEnumerable<IUserData> UserDatas = Model.ModelManager.GetUserDatas(User.ID);

            int year = CalcDate.Year;
            int month = CalcDate.Month;
            int cnt = 0;

            //set 12 months
            if (month == 12)
            {
                sitem.FirstMonth = month == 12 ? 1 : (month + 1);
            }
            else
            {
                year--;
                month++;
            }

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
                        case GameType.All_ssfive:
                            {
                                UserDataAll userDataAll = (UserDataAll)userData;

                                index = (int)(userDataAll.IsLeft ? GameType.All_ssfive_left : GameType.All_ssfive_right);
                            }
                            break;
                        case GameType.All_ashiage:
                            index = (int)GameType.All_ashiage;
                            break;
                        case GameType.CarePitLog:
                            index = (int)GameType.CarePitLog;
                            break;
                        case GameType.TUG:
                            index = (int)GameType.TUG;
                            break;
                    }

                    if (index >= 0)
                        sitem.Values[index, cnt] = userData.GaneScore;
                }

                cnt++;
                BegTime = EndTime;
            }

            return sitem;
        }
    }
}
