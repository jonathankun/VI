using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using VInspection.Clases.Summary;
using VInspection.Clases.Tables;
using static VInspection.Clases.Utils.UTime;
using static VInspection.Clases.Utils.UVInspectionModels;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace VInspection.Clases.Home
{
    [Activity(Label = "V Inspection", Icon = "@drawable/icon", Theme = "@style/AppTheme.Normal")]
    public class HomeActivityView : BaseActivity, HomeActivityViewInterface, DataConfirmationListener
    {
        public const string TAG = "DEBUG LOG";

        public FloatingActionButton FAB_New_CheckList;

        public Button B_Actualizar, B_Guardar;

        private RecyclerView mResumenPreUsos;
        private LinearLayout mEmptyStateContainer;
        RecyclerView.LayoutManager mLayoutManager;
        ListItemChechkListSummaryAdapter listItemChechkListSummaryAdapter;
        private Spinner mStatusFilterSpinner;
        ArrayAdapter<string> statusFilterAdapter;
        private ProgressBar Progreso;

        private ISharedPreferences mUserPreferences = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        private ISharedPreferences mActivityPreferences = Application.Context.GetSharedPreferences("HomeActivityState", FileCreationMode.Private);

        public const string EXTRA_CHECKLIST_ID = "ID_CHECKLIST";
        public const string EXTRA_CHECKLIST_STATE = "STATE_CHECKLIST";
        public const int IDENTIFICADOR_HOME = 1;
        public const string BANDERA_RESPUESTA = "Bandera";
        public static Result RESULT_OK;
        public static Result RESULT_CANCELLED;

        private const int CODE_DIALOG_REFRESH = 1;

        private const int STATUS_FILTER_DEFAULT_VALUE = 0;

        private Android.Widget.SearchView searchView;

        // Other Framework Elements
        private ReceptorActualizacionUI receptorUI;

        // FCM Elements
        //private FirebaseMessaging mFirebaseMessaging = FirebaseMessaging.getInstance();

        public static List<string> STATES_VALUES = new List<string> { "Todas", "Validados", "No Validados" };

        User user;

        public static Context context = Application.Context;

        public HomeInteractor mInteractor;
        public HomeViewModel mViewModel { get; private set; }

        public void setViewModel(HomeViewModel viewModel)
        {
            mViewModel = viewModel;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetTitle(Resource.String.title_home_activity);

            user = new User()
            {
                IdUsuario = mUserPreferences.GetInt("IdUsuario", 0),
                Nombre = mUserPreferences.GetString("Nombre", String.Empty),
                Cuenta = mUserPreferences.GetString("Cuenta", String.Empty),
                Contrasena = mUserPreferences.GetString("Contrasena", String.Empty),
                Perfil = mUserPreferences.GetString("Perfil", String.Empty)
            };

            mInteractor = new HomeInteractor(ApplicationContext);
            mViewModel = new HomeViewModel(this, mInteractor);

            if (user.Nombre != String.Empty && user.Perfil == PERFIL_DESARROLLADOR)
            {
                SetContentView(Resource.Layout.home_supervisor_activity);

                // Entonces suscribir a FCM
                //String topic = FirebaseConfig.GUARD_TOPIC;
                // Enviar petición de suscripción
                //mFirebaseMessaging.subscribeToTopic(topic);

                /*IntentFilter filtroGCM = new IntentFilter(IFirebaseMessagingService.EVITAR_CREAR_NOTIFICACION);
                receptorUI = new ReceptorActualizacionUI();
                LocalBroadcastManager.getInstance(this).registerReceiver(receptorUI, filtroGCM);*/


                PrepararLista_CheckList();

                Preparar_FloatingActionButton();
            }
            else if (user.Nombre != String.Empty && (user.Perfil == PERFIL_SUPERVISOR || user.Perfil == PERFIL_RESPONSABLE))
            {
                SetContentView(Resource.Layout.home_supervisor_activity);

                PrepararLista_CheckList();

                Preparar_FloatingActionButton();
            }
            else if (user.Nombre != String.Empty && user.Perfil == PERFIL_VIGILANTE)
            {
                SetContentView(Resource.Layout.home_guard_activity);

                PrepararLista_CheckList();
            }
            else if (user.Nombre != String.Empty && user.Perfil == PERFIL_CONDUCTOR)
            {
                SetContentView(Resource.Layout.home_driver_activity);

                Preparar_FloatingActionButton();
            }
            else
            {
                ISharedPreferencesEditor editUser = mUserPreferences.Edit();
                editUser.PutInt("IdUsuario", 0);
                editUser.PutString("Nombre", String.Empty);
                editUser.PutString("Cuenta", String.Empty);
                editUser.PutString("Contrasena", String.Empty);
                editUser.PutString("Perfil", String.Empty);
                editUser.Apply();
                showLoginScreen();
                Finish();
            }
        }

        public void PrepararLista_CheckList()
        {
            SupportToolbar toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            // Remover título de la action bar
            SupportActionBar.Title = "";

            mStatusFilterSpinner = FindViewById<Spinner>(Resource.Id.toolbar_spinner);

            statusFilterAdapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleSpinnerItem, STATES_VALUES);

            statusFilterAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            mStatusFilterSpinner.Adapter = statusFilterAdapter;

            mStatusFilterSpinner.ItemSelected += (o, e) =>
            {
                String status = mStatusFilterSpinner.GetItemAtPosition(e.Position).ToString();
                Log.Info(TAG, "Se seleccionó: " + status);
                cargarPreUsos(status);
            };

            mEmptyStateContainer = FindViewById<LinearLayout>(Resource.Id.empty_state_container);
            Progreso = FindViewById<ProgressBar>(Resource.Id.pb_progreso);

            mResumenPreUsos = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            // Plug in the linear layout manager:
            mLayoutManager = new LinearLayoutManager(this);
            mResumenPreUsos.SetLayoutManager(mLayoutManager);
            // Plug in my adapter:
            listItemChechkListSummaryAdapter = new ListItemChechkListSummaryAdapter(this, new List<CheckListSummary>(), mViewModel.ObtenerListaVehiculosBDI());

            listItemChechkListSummaryAdapter.ItemClick += OnItemClick;

            mResumenPreUsos.SetAdapter(listItemChechkListSummaryAdapter);
        }

        void OnItemClick(object sender, int position)
        {
            List<CheckListSummary> CheckListSummary = mViewModel.ObtenerListaPreUsosBDI();

            CheckListSummary clickedCheckListSummary = CheckListSummary[position];

            int ID = clickedCheckListSummary.IdResumen;
            int STATE = clickedCheckListSummary.Estado;

            Log.Info(TAG, "Se selecciono el PreUso de ID: " + ID);

            Intent intent = new Intent(context, typeof(SummaryActivityView));
            intent.PutExtra(EXTRA_CHECKLIST_ID, ID);
            intent.PutExtra(EXTRA_CHECKLIST_STATE, STATE);
            StartActivityForResult(intent, IDENTIFICADOR_HOME);
        }

        public void Preparar_FloatingActionButton()
        {
            FAB_New_CheckList = FindViewById<FloatingActionButton>(Resource.Id.FAB_new_checklist);

            FAB_New_CheckList.Click += (o, e) =>
            {
                Intent intent = new Intent(context, typeof(TablesActivityView));
                if (user.Perfil == PERFIL_DESARROLLADOR || user.Perfil == PERFIL_SUPERVISOR || user.Perfil == PERFIL_RESPONSABLE)
                {
                    Log.Info(TAG, "Entro a StartActivityForResult");
                    StartActivityForResult(intent, IDENTIFICADOR_HOME);
                }
                else
                {
                    Log.Info(TAG, "Entro a StartActivity");
                    StartActivity(intent);
                }
            };
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == IDENTIFICADOR_HOME)
            {
                if (resultCode == RESULT_OK)
                {
                    int respuesta = data.GetIntExtra(BANDERA_RESPUESTA, -1);
                    Log.Info(TAG, "El valor de respuesta es: " + respuesta);
                    if (respuesta == 1)
                    {
                        Log.Info(TAG, "respuesta");
                        refreshCheckListSummariesList();
                    }
                }
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();

            ISharedPreferencesEditor editActivity = mActivityPreferences.Edit();
            editActivity.PutBoolean("state", true);
            editActivity.Apply();
        }

        protected override void OnPause()
        {
            base.OnPause();
            LocalBroadcastManager.GetInstance(BaseContext).UnregisterReceiver(receptorUI);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            if (PERFIL_DESARROLLADOR == user.Perfil)
            {
                MenuInflater.Inflate(Resource.Menu.menu_toolbar_developer, menu);
            }
            if (PERFIL_VIGILANTE == user.Perfil || PERFIL_SUPERVISOR == user.Perfil || PERFIL_RESPONSABLE == user.Perfil)
            {
                MenuInflater.Inflate(Resource.Menu.menu_toolbar_sync, menu);
            }
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            switch (id)
            {
                case Resource.Id.action_sync:
                    refreshCheckListSummariesList();
                    return true;
                case Resource.Id.action_test_ok:
                    mViewModel.GenerarPreusoTestOKBDE();
                    return true;
                case Resource.Id.action_test_bad:
                    mViewModel.GenerarPreusoTestBadBDE();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void cargarPreUsos(string rawStatus)
        {
            Log.Info(TAG, "Entro en cargarPreUsos con estado:" + rawStatus);

            int status;

            switch (rawStatus)
            {
                case "Validados":
                    status = 1;
                    break;
                case "No Validados":
                    status = 0;
                    break;
                default:
                    status = 100;
                    break;
            }

            List<int> ListaIdCheckList = new List<int>();

            int NumFilas = 0;

            List<CheckListSummary> PreUsosBDI = mViewModel.ObtenerListaPreUsosBDI();

            try
            {
                NumFilas = PreUsosBDI.Count;
                Log.Info(TAG, "El numero de filas en la BDI es: " + NumFilas);
            }
            catch (Exception e)
            {
                Log.Info(TAG, e.Message);
            }

            for (int a = 0; a < NumFilas; a++)
            {
                if (status == 100)
                {
                    Log.Info(TAG, "Fila a agregar: " + a);
                    Log.Info(TAG, "El ID de la fila es: " + PreUsosBDI[a].IdResumen);
                    ListaIdCheckList.Add(PreUsosBDI[a].IdResumen);
                }
                else
                {
                    Log.Info(TAG, "Fila a Evaluar: " + a);
                    CheckListSummary VICLS = PreUsosBDI[a];
                    Log.Info(TAG, "El estado de la fila es: " + VICLS.Estado);
                    if (status == VICLS.Estado)
                    {
                        Log.Info(TAG, "Se agrega a la lista el Pre-Uso de ID: " + VICLS.IdResumen);
                        ListaIdCheckList.Add(VICLS.IdResumen);
                    }
                }
            }

            Log.Info(TAG, "Los IDs de Items a agregar son: " + ListaIdCheckList);

            if (ListaIdCheckList.Count > 0)
            {
                Log.Info(TAG, "Se actualizara la lista de CheckList");
                List<CheckListSummary> Lista_CheckListSummary = new List<CheckListSummary>();
                for (int a = 0; a < ListaIdCheckList.Count; a++)
                {
                    Log.Info(TAG, "Agregando Pre-Usos de fila: " + ListaIdCheckList[a]);
                    CheckListSummary VICLS = PreUsosBDI.Find(x => x.IdResumen == ListaIdCheckList[a]);
                    Lista_CheckListSummary.Add(VICLS);
                }
                showCheckListSummaries(Lista_CheckListSummary);
            }
            else
            {
                Log.Info(TAG, "No hay Pre-Usos del estado seleccionado");
                showNoCheckListSummaries();
            }
        }

        public void refreshCheckListSummariesList()
        {
            Log.Info(TAG, "refreshCheckListSummariesList");
            mViewModel.ObtenerListaPreUsosBDE();
            
        }

        public void errorRefreshnig()
        {
            Log.Info(TAG, "errorRefreshnig");
            Toast.MakeText(this, "Hubo un error al descargar la lista de Pre-Usos", ToastLength.Long).Show();
        }

        public void showProgress(bool mostrar)
        {
            mResumenPreUsos.Visibility = mostrar ? ViewStates.Gone : ViewStates.Visible;
            mEmptyStateContainer.Visibility = mostrar ? ViewStates.Gone : ViewStates.Visible;
            Progreso.Visibility = mostrar ? ViewStates.Visible : ViewStates.Gone;
        }

        public void showCheckListSummaries(List<CheckListSummary> listaPreUsos)
        {
            listItemChechkListSummaryAdapter.swapItems(listaPreUsos);

            mResumenPreUsos.Visibility = ViewStates.Visible;
            mEmptyStateContainer.Visibility = ViewStates.Gone;
            Progreso.Visibility = ViewStates.Gone;
        }

        public void showNoCheckListSummaries()
        {
            mResumenPreUsos.Visibility = ViewStates.Gone;
            mEmptyStateContainer.Visibility = ViewStates.Visible;
            Progreso.Visibility = ViewStates.Gone;
        }

        public void ShowMessage(string message)
        {
            Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
        }

        public void ShowDialog(string message, string okText, string cancelText)
        {
            DataConfirmationDialog frag = DataConfirmationDialog.NewInstance(CODE_DIALOG_REFRESH, message, okText, cancelText, this);
            frag.Show(FragmentManager, message);
        }

        public void OnDataSetAccept(int requestCode)
        {
            if (requestCode == CODE_DIALOG_REFRESH)
            {
                //Sin Accion
            }
        }

        public void OnDataSetCancel(int requestCode)
        {
            if (requestCode == CODE_DIALOG_REFRESH)
            {
                //Sin Accion
            }
        }


        public class ReceptorActualizacionUI : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                Log.Info(TAG, "ENTRÓ A ONRECEIVER");
                //mHomePresenter.refreshCheckListDelDia();
                //mostrarProgreso(true);
            }
        }

       public class ListItemChechkListSummaryAdapter : RecyclerView.Adapter
        {
            private List<CheckListSummary> mItems;
            private List<Vehicle> mVehicles;
            private Context mContext;
            public event EventHandler<int> ItemClick;

            public ListItemChechkListSummaryAdapter(Context context, List<CheckListSummary> items, List<Vehicle> vehicles)
            {
                mContext = context;
                mItems = items;
                mVehicles = vehicles;
            }

            void OnClick(int position)
            {
                if (ItemClick != null) ItemClick(this, position);
            }

            public void swapItems(List<CheckListSummary> checkListSummary)
            {
                if (checkListSummary == null)
                {
                    mItems = new List<CheckListSummary>();
                }
                else
                {
                    mItems = checkListSummary;
                    Log.Info(TAG, "Los items nuevos son: " + mItems);
                }
                NotifyDataSetChanged();
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                Log.Info(TAG, "Preparando posicion: " + position);

                CheckListSummary mCheckListSummary = mItems[position];

                Log.Info(TAG, "El el id del item de la posicion " + position + " es: " + mCheckListSummary.IdResumen);

                SimpleViewHolder simpleViewHolder = holder as SimpleViewHolder;

                switch (mCheckListSummary.Estado)
                {
                    case 0:
                        // ocultar botón
                        //simpleHolder.cancelButton.Visibility = ViewStates.Gone);
                        simpleViewHolder.statusIndicator.SetBackgroundResource(Resource.Color.color_estado_2);
                        break;
                    case 1:
                        // mostrar botón
                        //simpleHolder.cancelButton.Visibility = ViewStates.Visible;
                        simpleViewHolder.statusIndicator.SetBackgroundResource(Resource.Color.color_estado_1);
                        break;
                    case 2:
                        // ocultar botón
                        //simpleHolder.cancelButton.Visibility = ViewStates.Gone);
                        simpleViewHolder.statusIndicator.SetBackgroundResource(Resource.Color.color_estado_3);
                        break;
                }

                simpleViewHolder.date.Text = DateTimeToString(mCheckListSummary.Fecha);

                simpleViewHolder.item1.Text = mCheckListSummary.Vehiculo;
                Vehicle mVehicle = mVehicles.Find(x => x.Placa == mCheckListSummary.Vehiculo);
                simpleViewHolder.item2.Text = mVehicle.Modelo;
                simpleViewHolder.item3.Text = mVehicle.Area;
                simpleViewHolder.item4.Text = mCheckListSummary.Conductor;

                if (mCheckListSummary.BanderaPrincipal == 0)
                {
                    simpleViewHolder.flagIndicator.SetBackgroundResource(Resource.Color.color_estado_1);
                }
                else
                {
                    simpleViewHolder.flagIndicator.SetBackgroundResource(Resource.Color.color_estado_2);
                }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(mContext);
                View view = layoutInflater.Inflate(Resource.Layout.home_activity_include_listitem, parent, false);


                return new SimpleViewHolder(view, OnClick);
            }

            public override int ItemCount
            {
                get
                {
                    return mItems.Count;
                }
            }
        }

        public class SimpleViewHolder : RecyclerView.ViewHolder
        {
            public View statusIndicator;
            public TextView date;
            public TextView item1;
            public TextView item2;
            public TextView item3;
            public TextView item4;
            public View flagIndicator;
            //public Button cancelButton { get; private set; }

            public SimpleViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                statusIndicator = itemView.FindViewById(Resource.Id.V_status_indicator);
                date = itemView.FindViewById<TextView>(Resource.Id.TV_List_Date);
                item1 = itemView.FindViewById<TextView>(Resource.Id.TV_List_Item1);
                item2 = itemView.FindViewById<TextView>(Resource.Id.TV_List_Item2);
                item3 = itemView.FindViewById<TextView>(Resource.Id.TV_List_Item3);
                item4 = itemView.FindViewById<TextView>(Resource.Id.TV_List_Item4);
                flagIndicator = itemView.FindViewById(Resource.Id.V_flag_indicator);
                //cancelButton = (Button) itemView.findViewById(Resource.Id.button_cancel_appointment);

                itemView.Click += (sender, e) => listener(base.Position);

                /*itemView.Click += (sender, e) =>
                {
                    int p
                    listener(base.Position)
                };*/

                /*cancelButton.Click += (o, e) =>
                {
                    int position = AdapterPosition;
                    if (position != RecyclerView.NoPosition) {
                        mOnItemClickListener.onCancelAppointment(mItems.get(position));
                    }
                };*/

            }
        }
    }

    public interface HomeActivityViewInterface
    {
        void setViewModel(HomeViewModel viewModel);
        void refreshCheckListSummariesList();
        void errorRefreshnig();
        void showProgress(bool mostrar);
        void showCheckListSummaries(List<CheckListSummary> listaPreUsos);
        void showNoCheckListSummaries();
        void ShowMessage(string message);
        void ShowDialog(string message, string okText, string cancelText);
    }
}