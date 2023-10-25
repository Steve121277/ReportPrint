using System;

namespace ReportPrint.Model
{
    internal interface IUserData
    {
        int UserId { get; set; }
        GameType GameType { get; set; }
        DateTime MeasureTime { get; set; }
        float GaneScore { get; set; }
        string GameTitle { get; }
        int LineNo { get; set; }

    }
}
