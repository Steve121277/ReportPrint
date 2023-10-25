using System;

namespace ReportPrint.Model
{
    internal class UserDataAll : IUserData
    {
        public int UserId { get; set; }
        public GameType GameType { get; set; }
        public int LineNo { get; set; }
        public DateTime MeasureTime { get; set; }
        public float GaneScore { get; set; }
        public bool IsLeft { get; set; }

        public string GameTitle => GameType == GameType.All_ssfive ? Config.TtileOfAll_ssfive : Config.TtileOfAll_ashiage;
    }
}
