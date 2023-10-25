using System;

namespace ReportPrint.Model
{
    internal class UserDataCarePitLog : IUserData
    {
        public int UserId { get; set; }
        public GameType GameType { get; set; }
        public int LineNo { get; set; }
        public DateTime MeasureTime { get; set; }
        public float GaneScore { get; set; }

        public string GameTitle => Config.TitleOfCarePitLog;
    }
}
