using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Model
{
    class DataMapper
    {
        private ApplicationContext context = new ApplicationContext();

        private List<Zone> zones = new List<Zone>();
        
        private string _config;

        public DataMapper(string config)
        {
            context.Config = config;
            zones.Add(new Zone("Зона штатного появления 1", 5100, 5397, 10, 20));
            zones.Add(new Zone("Зона штатного появления 2", 4863, 5013, 10, 20));
            zones.Add(new Zone("Спуск конвейера", 2729, 2978, 10, 10));
            zones.Add(new Zone("Весы", 2008, 2214, 10, 10));
            zones.Add(new Zone("Брак по весу", 1688, 1996, 10, 10));
            zones.Add(new Zone("Металлодетектор", 1203, 1660, 20, 20));
            zones.Add(new Zone("Брак по металлу", 878, 1186, 10, 10));
            zones.Add(new Zone("Пленкообертка", 325, 725, 10, 10));
            zones.Add(new Zone("Штатный уход", 30, 290, 20, 20));
        }


        /// <summary>
        /// Количество брикетов, проходящих зону штатного появления 1 и  зону штатного появления 2
        /// </summary>
        /// <returns></returns>
        public int CountPassingZoneRegularAppearance(DateTime start, DateTime finish)
        {
            var zonaFirst = zones.FirstOrDefault(x => x.ZoneName == "Зона штатного появления 1");
            var zonaSecond = zones.FirstOrDefault(x => x.ZoneName == "Зона штатного появления 2");
            var left = zonaSecond.Left - zonaSecond.LeftFault;
            var right = zonaFirst.Right + zonaFirst.RightFault;
            var sql = @"SELECT   
                          COUNT(DISTINCT object_track.object_id) 
                        FROM
                          main.object_track
                          WHERE object_track.x >= "+left+" AND object_track.x <= "+right+
                          " AND object_track.crt_date >= '" + start +
                          "' AND object_track.crt_date <= '" + finish + "'";
            return context.GetData(sql);
        }

        /// <summary>
        /// Количество брикетов, приходящих на спуске ленточного конвейера
        /// </summary>
        /// <returns></returns>
        public int CountComingDownConveyor(DateTime start, DateTime finish)
        {
            var zonaComing= zones.FirstOrDefault(x => x.ZoneName == "Спуск конвейера");
            return GetCount(zonaComing, start, finish);
        }

        /// <summary>
        /// Kоличество брикетов, проходящих систему взвешивания (весы)
        /// </summary>
        /// <returns></returns>
        public int CountPassingWeighingSystem(DateTime start, DateTime finish)
        {
            var zonaWeight = zones.FirstOrDefault(x => x.ZoneName == "Весы");
            return GetCount(zonaWeight, start, finish);
        }

        /// <summary>
        /// Kоличество брикетов, отбракованных по весу (умирают в пределах зоны брака по весу);
        /// </summary>
        /// <returns></returns>
        public int CountCulledByWeight(DateTime start, DateTime finish)
        {
            var zonaWeight = zones.FirstOrDefault(x => x.ZoneName == "Брак по весу");
            var left = zonaWeight.Left - zonaWeight.LeftFault;
            var right = zonaWeight.Right + zonaWeight.RightFault;


            var sql = @"SELECT   
                          MIN(o.crt_date ),MAX(o.crt_date ),o.object_id
                        FROM
                          main.object_track AS o
                          WHERE o.x >= " + left + " AND o.x <= " + right +
                          " AND o.crt_date >= '" + start +
                          "' AND o.crt_date <= '" + finish +
                        "' GROUP BY o.object_id";
            List<DateData> dateList = context.GetDateData(sql);

            return context.GetCountCulled(dateList);
        }

        /// <summary>
        /// Kоличество брикетов, проходящих проверку на наличие металла (не умирают в пределах или сразу после зоны "Брак по металлу")
        /// </summary>
        /// <returns></returns>
        public int CountTestedPresenceOfMetal(DateTime start, DateTime finish)
        {
            var zonaMetal = zones.FirstOrDefault(x => x.ZoneName == "Брак по металлу");
            var left = zonaMetal.Left - zonaMetal.LeftFault;
            var right = zonaMetal.Right + zonaMetal.RightFault;


            var sql = @"SELECT   
                          MIN(o.crt_date ),MAX(o.crt_date ),o.object_id
                        FROM
                          main.object_track AS o
                          WHERE o.x >= " + left + " AND o.x <= " + right +
                          " AND o.crt_date >= '" + start +
                          "' AND o.crt_date <= '" + finish +
                        "' GROUP BY o.object_id";
            List<DateData> dateList = context.GetDateData(sql);

            return context.GetCountMetal(dateList);
        }

        /// <summary>
        /// Kоличество брикетов, отбракованных на металлодетекторе (умирают в зоне "Брак по металлу")
        /// </summary>
        /// <returns></returns>
        public int CountCulledMetalDetector(DateTime start, DateTime finish)
        {
            var zonaMetal = zones.FirstOrDefault(x => x.ZoneName == "Брак по металлу");
            var left = zonaMetal.Left - zonaMetal.LeftFault;
            var right = zonaMetal.Right + zonaMetal.RightFault;


            var sql = @"SELECT   
                          MIN(o.crt_date ),MAX(o.crt_date ),o.object_id
                        FROM
                          main.object_track AS o
                          WHERE o.x >= " + left + " AND o.x <= " + right +
                          " AND o.crt_date >= '" + start +
                          "' AND o.crt_date <= '" + finish +
                        "' GROUP BY o.object_id";
            List<DateData> dateList = context.GetDateData(sql);

            return context.GetCountDeathMetal(dateList);
        }

        /// <summary>
        /// Kоличество брикетов, проходящих через пленкообертку;
        /// </summary>
        /// <returns></returns>
        public int CountPassingFilmWrapper(DateTime start, DateTime finish)
        {
            var zonaFilmWrapper = zones.FirstOrDefault(x => x.ZoneName == "Пленкообертка");
            return GetCount(zonaFilmWrapper, start, finish);
        }

        /// <summary>
        /// Kоличество брикетов, отправленных на склад ("проходящих через зону " Штатный уход")
        /// </summary>
        /// <returns></returns>
        public int CountSentToWarehouse(DateTime start, DateTime finish)
        {
            var zonaWarehouse = zones.FirstOrDefault(x => x.ZoneName == "Штатный уход");
            return GetCount(zonaWarehouse, start,finish);
        }

        private int GetCount(Zone zona, DateTime start, DateTime finish)
        {
            var left = zona.Left - zona.LeftFault;
            var right = zona.Right + zona.RightFault;
            var sql = @"SELECT   
                          COUNT(DISTINCT object_track.object_id) 
                        FROM
                          main.object_track
                          WHERE object_track.x >= " + left + " AND object_track.x <= " + right +
                          " AND object_track.crt_date >= '" + start +
                          "' AND object_track.crt_date <= '" + finish+"'";
            return context.GetData(sql);
        }
    }

    class Zone
    {
        public Zone(string zoneName, int left, int right, int leftFault, int rightFault)
        {
            ZoneName = zoneName;
            Left = left;
            Right = right;
            LeftFault = leftFault;
            RightFault = rightFault;
        }

        /// <summary>
        /// имя зоны.
        /// </summary>
        public string ZoneName { get; set; }

        /// <summary>
        /// левая координата зоны.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// правая координата зоны.
        /// </summary>
        public int Right { get; set; }

        /// <summary>
        /// погрешность левой координаты зоны.
        /// </summary>
        public int LeftFault { get; set; }

        /// <summary>
        /// погрешность правой координаты зоны.
        /// </summary>
        public int RightFault { get; set; }
    }
}
