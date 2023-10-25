using System.Collections.Generic;
using System.Linq;

namespace ReportPrint.Model
{
    internal class ModelManager
    {
        public static IEnumerable<User> Users
        {
            get
            {
                return ExcelReader.GetUsersFromExcel();
            }
        }

        public static IEnumerable<IUserData> GetUserDatas(int UserID)
        {
            List<IUserData> userDatas = new List<IUserData>();

            userDatas.AddRange(UserDataFileManager.GetUserDataFromAllCSV(UserID));
            userDatas.AddRange(UserDataFileManager.GetUserDataFromTUGCSV(UserID));
            userDatas.AddRange(UserDataFileManager.GetUserDataFromLogCSV(UserID));

            return userDatas.OrderByDescending(d => d.MeasureTime);
        }
    }
}
