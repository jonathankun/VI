using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Icu.Util;
using System.Globalization;

namespace VInspection.Clases.Utils
{
    public class UTime
    {
        public static string obtenerFecha()
        {
            DateTime date = DateTime.Now.Date;
            Log.Info("DATETIME", "La fecha actual es: " + date);
            return DateToString(date);
        }

        public static int compararFechas(string a, string b)
        {
            DateTime fechaA = StringToDate(a);
            DateTime fechaB = StringToDate(b);
            return fechaA.CompareTo(fechaB);
        }

        public static string DateTimeToString(DateTime date)
        {
            return (date.Day + "/" + date.Month + "/" + date.Year + "\n" + date.Hour + ":" + date.Minute + ":" + date.Second );
        }

        public static string DateToString(DateTime date)
        {
            string dia = date.Day.ToString();
            string mes = date.Month.ToString();
            if (date.Day<10)
            {
                dia = "0" + date.Day;
            }
            if (date.Month<10)
            {
                mes = "0" + date.Month;
            }


            return (dia+ "/" + mes + "/" + date.Year);
        }

        public static DateTime StringToDate(string date)
        {
            //DateTime fecha = DateTime.Parse(date, new CultureInfo("en-US", true));

            DateTime fecha = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            Log.Info("FECHAS", "fecha: " + fecha);

            return fecha;
        }

        public static string DateToLongDateString(DateTime date)
        {
            string fecha = date.ToLongDateString();
            char[] caracteres = fecha.ToCharArray();
            int i;

            for (i = 0; i< caracteres.Length; i++)
            {
                if (caracteres[i] == ',')
                {
                    i++;
                    break;
                }
            }


            fecha = fecha.Substring(i, caracteres.Length - i);
            fecha = fecha.Replace(" ", ""); ;

            return fecha;
        }

        public static string PrepareDateToBrowser(DateTime date)
        {
            return date.Day + "-" + date.Month + "-" + date.Year;
        }
    }
}