using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportPrint.Model.Statistics
{
    /// <summary>
    /// Class <c>StatisticItem</c> models the statistic item from data.
    /// </summary>
    internal class StatisticItem
    {
        //Measured user information.
        internal User UserInfo { get; set; }
        //Previous 12 month's value.
        internal float[,] Values { get; set; } = new float[5, 12];
        //Month to be calculated.
        //uUsed for table caption and graph month axiss  drawing.
        internal int CalcMonth { get; set; }
        //Statistic Notes
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

            sitem.CalcMonth = month;

            while (cnt < 12)
            {
                DateTime EndTime = BegTime.AddMonths(1);
                //Initialze value to Single.NaN.
                for (int i = 0; i <= (int)GameType.CarePitLog; i++)
                {
                    sitem.Values[i, cnt] = Single.NaN;
                }

                //Find user datas in one months.
                IEnumerable<IUserData> findUserDatas = UserDatas.Where(u => u.MeasureTime >= BegTime && u.MeasureTime < EndTime).OrderBy(d => d.MeasureTime);

                //Search data and set to Values
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
                        sitem.Values[index, cnt] = userData.GameScore;
                    }
                }

                cnt++;
                BegTime = BegTime.AddMonths(-1);
            }

            return sitem;
        }
    }
}
