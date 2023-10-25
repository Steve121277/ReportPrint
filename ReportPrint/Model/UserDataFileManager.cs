using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ReportPrint.Model
{
    internal class UserDataFileManager
    {
        internal static IEnumerable<IUserData> GetUserDataFromAllCSV(int UserID)
        {
            List<IUserData> userDatas = new List<IUserData>();
            int csvLineNo = 0; //Line no in CSV
            DateTime? measureTime = null;
            float? score;

            if (!File.Exists(Config.CSVUserDataAllFilePath))
                return userDatas;

            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true
            };

            using (var streamReader = File.OpenText(Config.CSVUserDataAllFilePath))
            using (var csvReader = new CsvReader(streamReader, csvConfig))
            {
                while (csvReader.Read())
                {
                    csvReader.TryGetField<string>(0, out string value);
                    csvLineNo++;

                    if (!int.TryParse(value, out int userID))
                    {
                        continue;
                    }

                    if (userID != UserID)
                    {
                        continue;
                    }

                    if (csvReader.TryGetField<string>(1, out string Date))
                    {
                        measureTime = ParseDate(Date);

                        if (measureTime == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    if (!csvReader.TryGetField<string>(4, out string GameName))
                    {
                        if (GameName != "ssfive" &&
                            GameName != "ashiage")
                            continue;
                    }

                    if (!csvReader.TryGetField<string>(5, out string GameOption))
                    {
                        if (GameName != "ssfive" &&
                            GameName != "ashiage")
                            continue;
                    }

                    if (csvReader.TryGetField<string>(7, out string Score))
                    {
                        score = ParseScore(Score);
                        if (score == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    UserDataAll userData = new UserDataAll()
                    {
                        UserId = userID,
                        GameType = GameName == "ssfive" ? GameType.All_ssfive : GameType.All_ashiage,
                        LineNo = csvLineNo - 1,
                        MeasureTime = measureTime.Value,
                        GaneScore = score.Value,
                        IsLeft = GameOption == Config.TtileOfAll_ashiage_left
                    };

                    userDatas.Add(userData);
                }
            }

            return userDatas;
        }

        internal static IEnumerable<IUserData> GetUserDataFromLogCSV(int UserID)
        {
            List<IUserData> userDatas = new List<IUserData>();
            int csvLineNo = 0; //Line no in CSV
            DateTime? measureTime = null;
            float? score;

            if (!File.Exists(Config.CSVUserDataCarePitLogFilePath))
                return userDatas;

            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = "\t",
                HasHeaderRecord = true
            };

            using (var streamReader = File.OpenText(Config.CSVUserDataCarePitLogFilePath))
            using (var csvReader = new CsvReader(streamReader, csvConfig))
            {
                while (csvReader.Read())
                {
                    csvLineNo++;
                    csvReader.TryGetField<string>(2, out string value);

                    if (!int.TryParse(value, out int userID))
                    {
                        continue;
                    }

                    if (userID != UserID)
                    {
                        continue;
                    }

                    if (csvReader.TryGetField<string>(1, out string Date))
                    {
                        measureTime = ParseDate(Date);

                        if (measureTime == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    if (csvReader.TryGetField<string>(4, out string Score))
                    {
                        score = ParseScore(Score);
                        if (score == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    UserDataCarePitLog userData = new UserDataCarePitLog()
                    {
                        UserId = userID,
                        GameType = GameType.CarePitLog,
                        LineNo = csvLineNo - 1,
                        MeasureTime = measureTime.Value,
                        GaneScore = score.Value
                    };

                    userDatas.Add(userData);
                }
            }

            return userDatas;
        }

        public static IEnumerable<IUserData> GetUserDataFromTUGCSV(int UserID)
        {
            List<IUserData> userDatas = new List<IUserData>();
            int csvLineNo = 0; //Line no in CSV
            DateTime? measureTime = null;
            float? score;

            if (!File.Exists(Config.CSVUserDataTUGFilePath))
                return userDatas;

            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true
            };

            using (var streamReader = File.OpenText(Config.CSVUserDataTUGFilePath))
            using (var csvReader = new CsvReader(streamReader, csvConfig))
            {
                while (csvReader.Read())
                {
                    csvLineNo++;
                    csvReader.TryGetField<string>(0, out string value);

                    if (!int.TryParse(value, out int userID))
                    {
                        continue;
                    }

                    if (userID != UserID)
                    {
                        continue;
                    }

                    if (csvReader.TryGetField<string>(1, out string Date))
                    {
                        measureTime = ParseDate(Date);

                        if (measureTime == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    if (csvReader.TryGetField<string>(2, out string Score))
                    {
                        score = ParseScore(Score);
                        if (score == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    UserDataCarePitLog userData = new UserDataCarePitLog()
                    {
                        UserId = userID,
                        GameType = GameType.TUG,
                        LineNo = csvLineNo - 1,
                        MeasureTime = measureTime.Value,
                        GaneScore = score.Value
                    };

                    userDatas.Add(userData);
                }
            }

            return userDatas;
        }

        public static bool DeleteUserData(IUserData userData)
        {
            string pathName = string.Empty;

            if (userData.GameType == GameType.All_ssfive_left ||
                userData.GameType == GameType.All_ssfive_right ||
                userData.GameType == GameType.All_ashiage)
                pathName = Config.CSVUserDataAllFilePath;
            else if (userData.GameType == GameType.TUG)
                pathName = Config.CSVUserDataTUGFilePath;
            else if (userData.GameType == GameType.CarePitLog)
                pathName = Config.CSVUserDataCarePitLogFilePath;

            if (!File.Exists(pathName))
                return false;

            List<String> lines = new List<String>();
            int lineNo = 0;

            using (StreamReader reader = new StreamReader(pathName))
            {
                String line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (lineNo != userData.LineNo)
                        lines.Add(line);
                    lineNo++;
                }
            }

            //Set file size 0
            File.WriteAllBytes(pathName, new byte[0]);
            using (StreamWriter writer = new StreamWriter(pathName, true))
            {
                foreach (String line in lines)
                    writer.WriteLine(line);
            }

            return true;
        }

        public static bool ModifyUserData(IUserData userData, float value)
        {
            if (userData.GameType == GameType.All_ssfive_left ||
                userData.GameType == GameType.All_ssfive_right ||
                userData.GameType == GameType.All_ashiage)
                return ModifyUserDataFromAllCSV(userData, value);
            else if (userData.GameType == GameType.TUG)
                return ModifyUserDataFromTUGCSV(userData, value);
            else if (userData.GameType == GameType.CarePitLog)
                return ModifyUserDataFromLogCSV(userData, value);

            return false;
        }

        static bool ModifyUserDataFromAllCSV(IUserData userData, float value)
        {
            string pathName = Config.CSVUserDataAllFilePath;

            if (!File.Exists(pathName))
                return false;

            string[] lines = File.ReadAllLines(pathName);

            string line = lines[userData.LineNo];

            var split = line.Split(',');

            if (split.Length < 8)
                return false;

            split[7] = value.ToString();
            lines[userData.LineNo] = string.Join(",", split);

            File.WriteAllLines(pathName, lines);

            return true;
        }

        static bool ModifyUserDataFromLogCSV(IUserData userData, float value)
        {
            string pathName = Config.CSVUserDataCarePitLogFilePath;

            if (!File.Exists(pathName))
                return false;

            string[] lines = File.ReadAllLines(pathName);

            string line = lines[userData.LineNo];

            var split = line.Split('\t');

            if (split.Length < 5)
                return false;

            split[4] = value.ToString();
            lines[userData.LineNo] = string.Join("\t", split);

            File.WriteAllLines(pathName, lines);

            return true;
        }

        static bool ModifyUserDataFromTUGCSV(IUserData userData, float value)
        {
            string pathName = Config.CSVUserDataTUGFilePath;

            if (!File.Exists(pathName))
                return false;

            string[] lines = File.ReadAllLines(pathName);

            string line = lines[userData.LineNo];

            var split = line.Split(',');

            if (split.Length < 3)
                return false;

            split[2] = value.ToString();
            lines[userData.LineNo] = string.Join(",", split);

            File.WriteAllLines(pathName, lines);

            return true;
        }

        static bool MakeTUGSkelton()
        {
            if (!Directory.Exists(Config.CSVUserDataTUGDirectoryPath))
            {
                Directory.CreateDirectory(Config.CSVUserDataTUGDirectoryPath);
            }

            File.WriteAllLines(Config.CSVUserDataTUGFilePath, new string[] { "UserID,DATE,Score" });

            return true;
        }

        internal static bool SaveTUGData(int UserID, DateTime MeasureTime, float GameScore)
        {
            string pathName = Config.CSVUserDataTUGFilePath;

            if (!File.Exists(pathName))
            {
                MakeTUGSkelton();
            }

            if (!File.Exists(pathName))
                return false;

            using (StreamWriter writer = new StreamWriter(pathName, true))
            {
                writer.WriteLine($"{UserID},{MeasureTime.ToString("yyyy/M/d")},{GameScore}");
            }

            return true;
        }

        static DateTime? ParseDate(string MeasureDateTime)
        {
            string[] formats = {
                "yyyy/M/d",
                "yyyy/M/d HH:mm",
                "yyyy/M/d h:mm",
                "yyyy/M/d HH:mm:s",
                "yyyy/M/d HH:mm:ss",
                "yyyy/M/d HH:mm:ss",
            };

            if (DateTime.TryParseExact(MeasureDateTime, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue))
            {
                return dateValue;
            }

            return null;
        }

        static float? ParseScore(string GameScore)
        {
            if (!float.TryParse(GameScore, out float score))
            {
                return null;
            }

            return score;
        }
    }
}
