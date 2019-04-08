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

namespace VInspection.Clases
{
    public class DataConfirmationDialog : DialogFragment
    {
        DataConfirmationListener _DataConfirmationListener;
        int _RequestCode;
        string _Mensaje;
        string _AcceptText;
        string _CancelText;

        public static DataConfirmationDialog NewInstance(int requestCode, string mensaje, string acceptText, string cancelText, DataConfirmationListener listener)
        {
            DataConfirmationDialog frag = new DataConfirmationDialog();
            frag._RequestCode = requestCode;
            frag._Mensaje = mensaje;
            frag._AcceptText = acceptText;
            frag._CancelText = cancelText;
            frag._DataConfirmationListener = listener;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return CreateSimpleDialog();
        }

        public AlertDialog CreateSimpleDialog()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);

            builder.SetMessage(_Mensaje);
            builder.SetPositiveButton(_AcceptText, PositiveButton);
            if (_CancelText != "False")
            {
                builder.SetNegativeButton(_CancelText, NegativeButton);
            }

            return builder.Create();
        }

        private void PositiveButton(object sender, DialogClickEventArgs e)
        {
            _DataConfirmationListener.OnDataSetAccept(_RequestCode);
        }

        private void NegativeButton(object sender, DialogClickEventArgs e)
        {
            _DataConfirmationListener.OnDataSetCancel(_RequestCode);
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);

            _DataConfirmationListener = (DataConfirmationListener)activity;
        }
    }

    public interface DataConfirmationListener
    {
        void OnDataSetAccept(int requestCode);
        void OnDataSetCancel(int requestCode);
    }
}