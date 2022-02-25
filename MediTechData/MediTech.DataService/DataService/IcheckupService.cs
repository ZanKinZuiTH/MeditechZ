using MediTech.Model;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediTech.DataService
{
    public class IcheckupService 
    {

        public string ichecktest()
        {
            string requestApi = string.Format("/WeatherForecast/ichecktest");
            string data = IcheckupHelper.Get<string>(requestApi);
            return data;
        }



    }

}