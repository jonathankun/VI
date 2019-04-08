
using Android.App;
using Android.Content;
using Android.Net;
using Android.Util;
using static VInspection.Clases.Utils.UServices;

namespace VInspection.Clases.Utils
{
    public class UConnection
    {
        public const string TAG = "DEBUG LOG";

        public static bool conextadoRedInterna()
        {
            ConnectivityManager connectivity = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            if (connectivity != null)
            {
                NetworkInfo info = connectivity.ActiveNetworkInfo;
                if (info != null)
                {
                    if (Wifi_Name == info.ExtraInfo)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool conectadoWifi()
        {
            ConnectivityManager connectivity = (ConnectivityManager) Application.Context.GetSystemService(Context.ConnectivityService);
            if (connectivity != null)
            {
                NetworkInfo info = connectivity.ActiveNetworkInfo;
                if (info != null)
                {
                    Log.Info(TAG, "info.ToString: " + info.ToString());
                    Log.Info(TAG, "info.ExtraInfo: " + info.ExtraInfo);
                    if (info.TypeName == "WIFI")
                    {
                        if (info.IsConnected)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool conectadoRedMovil()
        {
            ConnectivityManager connectivity = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            if (connectivity != null)
            {
                //NetworkInfo info = connectivity.GetNetworkInfo(ConnectivityManager.TYPE_MOBILE);
                NetworkInfo info = connectivity.ActiveNetworkInfo;
                if (info.TypeName == "MOBILE")
                {
                    if (info != null)
                    {
                        if (info.IsConnected)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}