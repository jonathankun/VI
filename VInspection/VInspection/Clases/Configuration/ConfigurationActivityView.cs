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

namespace VInspection.Clases.Configuration
{
    [Activity(Label = "Configuración", Icon = "@drawable/icon", Theme = "@style/AppTheme.Normal")]
    public class ConfigurationActivityView : BaseActivity
    {
        EditText Ip;

        Button SaverEditor;

        bool BanderaBoton;

        private ISharedPreferences mUserPreferences = Application.Context.GetSharedPreferences("ConfigurationInfo", FileCreationMode.Private);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.configuration_activity);

            Ip = FindViewById<EditText>(Resource.Id.ET_IP);
            Ip.Text = mUserPreferences.GetString("ConfigurationIp", String.Empty);
            Ip.Enabled = false;
            SaverEditor = FindViewById<Button>(Resource.Id.B_SaverEditor);
            SaverEditor.Text = "Editar";

            SaverEditor.Click += (o, e) =>
            {
                if (BanderaBoton)
                {
                    BanderaBoton = false;
                    Ip.Enabled = false;
                    SaverEditor.Text = "Editar";
                    ISharedPreferencesEditor editUser = mUserPreferences.Edit();
                    editUser.PutString("ConfigurationIp", Ip.Text.ToString());
                    editUser.Apply();
                }
                else
                {
                    BanderaBoton = true;
                    Ip.Enabled = true;
                    SaverEditor.Text = "Guardar";
                }
            };
        }
    }
}