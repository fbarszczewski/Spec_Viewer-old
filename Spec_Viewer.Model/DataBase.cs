using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Spec_Viewer.Model
{
    public class DataBase
    {
        private static readonly string _connString =
            "SERVER= remotemysql.com" +
            ";USERID= IalS35jGSf" +
            ";PASSWORD= lHxoGp4AQC" +
            ";DATABASE= IalS35jGSf" +
            ";Connection Timeout=10;";




        //WHERE Devices.model='P50' ORDER BY Devices.id DESC  




        public static DataTable GetSpec(string saveId,string serial)
        {
            MySqlConnection conn = new MySqlConnection(_connString);
            MySqlCommand cmd = new MySqlCommand(SpecCmd(saveId,serial),conn);
            DataTable dt = new DataTable();

            try
            {

                conn.Open();
                
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }

            return dt;
        }

        private static string SpecCmd(string saveId,string serial)
        {
            List<string> searchParams=new List<string>();

            string whereCmd = " ";

            if(!string.IsNullOrWhiteSpace(saveId))
            {
                searchParams.Add($"Devices.saveReference LIKE '%{saveId.Trim()}%'");
            }

            if(!string.IsNullOrWhiteSpace(serial))
            {
                searchParams.Add($"Devices.serial LIKE '%{serial.Trim()}%'");
            }

            if(searchParams.Count>0)
            {
                whereCmd=$"WHERE {string.Join(" AND ",searchParams)} ";
            }


            return "SELECT " +
                                                "Devices.id," +
                                                "Devices.saveReference as Reference, " +
                                                "Devices.serial," +

                                                "CONCAT(Devices.model,' '," +
                                                "Devices.cpu,'/'," +
                                                "Devices.ramSizeSum,'GB/'," +
                                                "Devices.hddSize,'/'," +
                                                "Devices.optical,'/'," +
                                                "Devices.diagonal," +
                                                "Devices.resolution,'/'," +
                                                "Devices.LicenseLabel) as Description," +

                                                "Devices.hddSN as HddSerial, " +
                                                "Licenses.newMAR as CMAR," +
                                                "Devices.Comments, " +
                                                "Devices.chassisType, " +
                                                "Devices.gpu," +
                                                "Devices.ramSize," +
                                                "Devices.ramPN," +
                                                "Devices.ramSN," +
                                                "Devices.hddSize," +
                                                "Devices.hddPN," +
                                                "Devices.hddSN," +
                                                "Devices.hddHealth," +
                                                "Devices.osName," +
                                                "Devices.osBuild," +
                                                "Devices.osLanguages," +
                                                "Devices.osSerial," +
                                                "Devices.batteryHealth," +
                                                "Devices.batterySerial," +
                                                "DATE_FORMAT(TIMESTAMP(date,'2:00:00'),'%d/%m/%y %T') as timeStamp," +
                                                "Devices.batteryCharge " +

                                                "FROM Devices " +
                                                $"LEFT JOIN Licenses ON Devices.serial=Licenses.serial {whereCmd} ORDER BY Devices.id DESC LIMIT 50";
        }
    }
}
