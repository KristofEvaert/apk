using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;

namespace AddHardware.Models
{
   public  class Server
    {
        public static int? nummer;
        public static string melding;
        public static void PostToDb(Object newObject, string destination)
        {
            try
            {
                //string destination = "https://localhost:5001/api/v1/hardwareinfo/PC";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destination);
                var pcJson = new JavaScriptSerializer().Serialize(newObject);
                request.Method = "POST";
                var dataPc = Encoding.UTF8.GetBytes(pcJson);
                request.Headers.Add("apiKey", "50ffbdb7-18ca-455e-afcd-a3e7e36ef725");
                request.ContentType = "application/json";
                request.ContentLength = dataPc.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(dataPc, 0, dataPc.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    Feedback user = jss.Deserialize<Feedback>(responseStr);
                    nummer = Convert.ToInt32(user.code);
                    melding = user.description;
                    if (nummer != null)
                    {
                        MessageBox.Show($"{melding} nr:{nummer}");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"unable to write hardware to database {e.Message}");

            }


        }
        public static List<Locatie> locatieLijst;
        public static List<Locatie> ServerGetLocaties()
        {  
            try
            {
                string destination = "https://api.apkgroup.eu/api/v1/seat";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destination);
                request.Method = "GET";
                request.Headers.Add("apiKey", "50ffbdb7-18ca-455e-afcd-a3e7e36ef725");
                request.ContentType = "application/json";
                // Stream requestStream = request.GetRequestStream();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    locatieLijst = jss.Deserialize<List<Locatie>>(responseStr);
                }
                return locatieLijst;


            }
            catch (Exception e)
            {

                MessageBox.Show($"server connection failed unable to get locations from database {e.Message}");
            }

            return locatieLijst;
        }
        public static int GetVolgNummer(string destination)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destination);
                request.Method = "GET";
                request.Headers.Add("apiKey", "50ffbdb7-18ca-455e-afcd-a3e7e36ef725");
                request.ContentType = "application/json";
                // Stream requestStream = request.GetRequestStream();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    Feedback user = jss.Deserialize<Feedback>(responseStr);
                    nummer = Convert.ToInt32(user.code);
                    melding = user.description;
                }
                return (int)nummer;
            }
            catch (Exception e)
            {

                MessageBox.Show($"server connection failed {e.Message} er is een fout met het ophalen van de volgnummer van het scherm");
            }
            return (int)nummer;
        }

    }
}
