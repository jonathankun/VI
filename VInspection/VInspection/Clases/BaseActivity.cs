using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UVInspectionModels;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace VInspection.Clases
{
    [Activity(Label = "BaseActivity", Theme = "@style/AppTheme.Normal")]
    public class BaseActivity : AppCompatActivity
    {
        public const string PERFIL_DESARROLLADOR = "Desarrollador";
        public const string PERFIL_SUPERVISOR = "Supervisor";
        public const string PERFIL_RESPONSABLE = "Responsable";
        public const string PERFIL_CONDUCTOR = "Conductor";
        public const string PERFIL_VIGILANTE = "Vigilante";
        public const string PROJECT_ID = "v-inspection";
        public const string EXTRA_NAV_DRAWER_ID = "NAV_DRAWER_ID";

        protected SupportToolbar mToolbar;
        protected NavigationView mNavigationView;
        protected DrawerLayout mDrawerLayout;

        private BroadcastReceiver mPushMessagesReceiver;

        private ISharedPreferences mUserPreferences = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

        User user;

        public const string TAG = "DEBUG LOG";

        Context mContext = Application.Context;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            user = new User()
            {
                IdUsuario = mUserPreferences.GetInt("IdUsuario", 0),
                Nombre = mUserPreferences.GetString("Nombre", String.Empty),
                Cuenta = mUserPreferences.GetString("Cuenta", String.Empty),
                Contrasena = mUserPreferences.GetString("Contrasena", String.Empty),
                Perfil = mUserPreferences.GetString("Perfil", String.Empty)
            };

            if (user.Cuenta == String.Empty || user.Contrasena == String.Empty)
            {
                Log.Info(TAG, "No hay cuenta Activa");
                ISharedPreferencesEditor editUser = mUserPreferences.Edit();
                editUser.PutInt("IdUsuario", 0);
                editUser.PutString("Nombre", String.Empty);
                editUser.PutString("Cuenta", String.Empty);
                editUser.PutString("Contrasena", String.Empty);
                editUser.PutString("Perfil", String.Empty);
                editUser.Apply();
                showLoginScreen();
            }
            else
            {
                Log.Info(TAG, "Hay cuenta Activa");
                Log.Info(TAG, "El usuario es: " + mUserPreferences.GetString("Nombre", String.Empty));
                Log.Info(TAG, "El perfil es: " + mUserPreferences.GetString("Perfil", String.Empty));
            }

            /*ActionBar ab = getSupportActionBar();
            if (ab != null)
            {
                ab.setDisplayHomeAsUpEnabled(true);
            }
            overridePendingTransition(0, 0);*/
        }


        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mNavigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            if (mNavigationView != null)
            {
                SetUpSlideMenu(mNavigationView);
            }
        }


        protected override void OnResume()
        {
            base.OnResume();
            LocalBroadcastManager.GetInstance(this).RegisterReceiver(mPushMessagesReceiver, new IntentFilter("registroCompleto"));
        }

        protected override void OnPause()
        {
            LocalBroadcastManager.GetInstance(this).UnregisterReceiver(mPushMessagesReceiver);
            base.OnPause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            /*if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }*/
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return true;
        }

        public void showLoginScreen()
        {
            Intent intent = new Intent(this, typeof(Login.LoginActivityView));
            this.StartActivity(intent);
            Finish();
        }

        public override void SetContentView(int layoutResID)
        {
            base.SetContentView(layoutResID);
            setUpToolbar();
        }

        protected SupportToolbar setUpToolbar()
        {
            if (mToolbar == null)
            {
                mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
                if (mToolbar != null)
                {
                    SetSupportActionBar(mToolbar);
                }
            }
            return mToolbar;
        }

        private void SetUpSlideMenu(NavigationView mNavigationView)
        {
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, mDrawerLayout, setUpToolbar(), Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            mDrawerLayout.SetDrawerListener(toggle);
            toggle.SyncState();

            mNavigationView.NavigationItemSelected += (object sender, NavigationView.NavigationItemSelectedEventArgs e) =>
            {
                e.MenuItem.SetChecked(true);
                onOptionsItemSelected(e.MenuItem);
                mDrawerLayout.CloseDrawers();
            };

            // Poner título de la cabecera
            View headerView = mNavigationView.InflateHeaderView(Resource.Layout.cabecera_nav);
            headerView.FindViewById<TextView>(Resource.Id.nav_drawer_texto_nombre).Text = user.Nombre;
            headerView.FindViewById<TextView>(Resource.Id.nav_drawer_texto_perfil).Text = user.Perfil;
            headerView.FindViewById<TextView>(Resource.Id.nav_drawer_texto_cuenta).Text = user.Cuenta;

            if (user.Perfil != "")
            {
                switch (user.Perfil)
                {
                    case PERFIL_DESARROLLADOR:
                        Log.Info(TAG, "Menu de perfil de supervisor");
                        mNavigationView.InflateMenu(Resource.Menu.menu_nav_drawer_developer);
                        break;
                    case PERFIL_SUPERVISOR:
                        Log.Info(TAG, "Menu de perfil de supervisor");
                        mNavigationView.InflateMenu(Resource.Menu.menu_nav_drawer_supervisor);
                        break;
                    case PERFIL_RESPONSABLE:
                        Log.Info(TAG, "Menu de perfil de supervisor");
                        mNavigationView.InflateMenu(Resource.Menu.menu_nav_drawer_supervisor);
                        break;
                    case PERFIL_CONDUCTOR:
                        Log.Info(TAG, "Menu de perfil de conductor");
                        mNavigationView.InflateMenu(Resource.Menu.menu_nav_drawer_driver);
                        break;
                    case PERFIL_VIGILANTE:
                        Log.Info(TAG, "Menu de perfil de vigilante");
                        mNavigationView.InflateMenu(Resource.Menu.menu_nav_drawer_guard);
                        break;
                    default:
                        Log.Info(TAG, "No Hay Menu");
                        throw new Java.Lang.RuntimeException("Perfil de usuario no encontrado.");
                }
            }
        }

        private bool onOptionsItemSelected(IMenuItem menuItem)
        {
            int navDrawerId = menuItem.ItemId;
            /*switch (navDrawerId)
            {
                case Resource.Id.home:
                    return true;
            }
            return base.OnOptionsItemSelected(menuItem);*/

            Intent intent = new Intent();
            intent.PutExtra(EXTRA_NAV_DRAWER_ID, navDrawerId);

            switch (navDrawerId)
            {
                case Resource.Id.nav_inicio:
                    intent = new Intent(this, typeof(Home.HomeActivityView));
                    this.StartActivity(intent);
                    Finish();
                    return true;

                case Resource.Id.nav_supervision:

                    return true;

                case Resource.Id.nav_vehiculos:
                    intent = new Intent(this, typeof(Vehicles.VehicleActivityView));
                    this.StartActivity(intent);
                    Finish();
                    return true;

                case Resource.Id.nav_usuarios:
                    intent = new Intent(this, typeof(Users.UserActivityView));
                    this.StartActivity(intent);
                    Finish();
                    return true;

                case Resource.Id.nav_sync_db:
                    DeleteSummary();
                    intent = new Intent(this, typeof(Home.HomeActivityView));
                    this.StartActivity(intent);
                    Finish();
                    return true;

                case Resource.Id.nav_usuario:
                    intent = new Intent(this, typeof(UsersEditor.UserEditorActivityView));
                    intent.PutExtra(Users.UserActivityView.EXTRA_USER_ID, -1);
                    intent.PutExtra(Users.UserActivityView.EXTRA_FUNCTION, 2); //Funciones: "1": Editar; "2": Ver;
                    this.StartActivity(intent);
                    Finish();
                    return true;

                case Resource.Id.nav_configurar:
                    intent = new Intent(this, typeof(Configuration.ConfigurationActivityView));
                    this.StartActivity(intent);
                    Finish();
                    return true;

                case Resource.Id.nav_cerrar_sesion:
                    CloseSession();
                    return true;

                default:
                    throw new Java.Lang.RuntimeException("Un equipo con ese id del menu drawer debe existir para seguir la ejecución");
            }
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
        }

        /*public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            menuItem.SetChecked(true);
            int id = menuItem.ItemId;

            ShowNavDrawerOption(id);
            return true;
        }

        private void ShowNavDrawerOption(int navDrawerId)
        {
            Intent intent = new Intent();
            intent.PutExtra(EXTRA_NAV_DRAWER_ID, navDrawerId);
            
            switch (navDrawerId)
            {
                case Resource.Id.nav_inicio:
                    intent = new Intent(this, typeof(Home.HomeActivityView));
                    this.StartActivity(intent);
                    Finish();
                    break;

                case Resource.Id.nav_export_db:
                    ExportSummary();
                    break;

                case Resource.Id.nav_sync_db:
                    DeleteSummary();
                    intent = new Intent(this, typeof(Home.HomeActivityView));
                    this.StartActivity(intent);
                    Finish();
                    break;


                case Resource.Id.nav_cerrar_sesion:
                    CloseSession();
                    break;

                default:
                    throw new Java.Lang.RuntimeException("Un equipo con ese id del menu drawer debe existir para seguir la ejecución");
            }
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
        }*/

        private void ExportSummary()
        {

        }

        private void DeleteSummary()
        {

        }

        private void CloseSession()
        {
            ISharedPreferencesEditor editUser = mUserPreferences.Edit();
            editUser.PutInt("IdUsuario", 0);
            editUser.PutString("Nombre", String.Empty);
            editUser.PutString("Cuenta", String.Empty);
            editUser.PutString("Contrasena", String.Empty);
            editUser.PutString("Perfil", String.Empty);
            editUser.Apply();

            // Además se cancelan los registros de tópicos
            //unsubsTopics();

            // Y finalmente se muestra la pantalla de procesos
            showLoginScreen();
        }
        
    }
}