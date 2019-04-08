using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using static VInspection.Clases.Utils.UVInspectionModels;
using static VInspection.Clases.VehiclesEditor.VehicleEditorActivityView;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace VInspection.Clases.Vehicles
{
    [Activity(Label = "Vehiculos", Icon = "@drawable/icon", Theme = "@style/AppTheme.Normal")]
    public class VehicleActivityView : BaseActivity, VehicleActivityViewInterface, DataConfirmationListener
    {
        public FloatingActionButton FAB_New_Vehicle;

        private RecyclerView mVehiculo;
        private LinearLayout mEmptyStateContainer;
        RecyclerView.LayoutManager mLayoutManager;
        ListItemVehicleAdapter listItemVehicleAdapter;
        private Spinner mStatusFilterSpinner;
        ArrayAdapter<string> statusFilterAdapter;
        private ProgressBar Progreso;

        private ISharedPreferences mActivityPreferences = Application.Context.GetSharedPreferences("VehicleActivityState", FileCreationMode.Private);

        public const string EXTRA_VEHICLE_ID = "ID_VEHICLE";
        public const string EXTRA_FUNCTION = "FUNCTION";
        public const int IDENTIFICADOR_VEHICLE = 2;

        private const int CODE_DIALOG_REFRESH = 1;

        public static List<string> STATES_VALUES = new List<string> {"Todos", "Estado 1", "Estado 2", "Estado 3"};

        Context context = Application.Context;

        public VehicleInteractor mInteractor;
        public VehicleViewModel mViewModel { get; private set; }

        public void setViewModel(VehicleViewModel viewModel)
        {
            mViewModel = viewModel;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.vehicle_activity);

            mInteractor = new VehicleInteractor(ApplicationContext);
            mViewModel = new VehicleViewModel(this, mInteractor);


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
                cargarVehiculos(status);
            };

            mEmptyStateContainer = FindViewById<LinearLayout>(Resource.Id.empty_state_container);
            Progreso = FindViewById<ProgressBar>(Resource.Id.pb_progreso);

            mVehiculo = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            // Plug in the linear layout manager:
            mLayoutManager = new LinearLayoutManager(this);
            mVehiculo.SetLayoutManager(mLayoutManager);
            // Plug in my adapter:
            listItemVehicleAdapter = new ListItemVehicleAdapter(this, new List<Vehicle>());

            showVehicleList(mViewModel.ObtenerListaVehiculosBDI());

            listItemVehicleAdapter.ItemClick += OnItemClick;

            mVehiculo.SetAdapter(listItemVehicleAdapter);

            Preparar_FloatingActionButton();
        }

        void OnItemClick(object sender, int position)
        {
            List<Vehicle> vehicle = mViewModel.ObtenerListaVehiculosBDI();

            Vehicle clickedVehicle = vehicle[position];

            int ID = clickedVehicle.IdVehiculo;

            Log.Info(TAG, "Se selecciono el Vehiculo de ID: " + ID);

            Intent intent = new Intent(context, typeof(VehiclesEditor.VehicleEditorActivityView));
            intent.PutExtra(EXTRA_VEHICLE_ID, ID);
            intent.PutExtra(EXTRA_FUNCTION, 2); //Funciones: "1": Editar; "2": Ver;
            StartActivityForResult(intent, IDENTIFICADOR_VEHICLE);
        }

        public void Preparar_FloatingActionButton()
        {
            FAB_New_Vehicle = FindViewById<FloatingActionButton>(Resource.Id.FAB_new_vehicle);

            FAB_New_Vehicle.Click += (o, e) =>
            {
                Intent intent = new Intent(context, typeof(VehiclesEditor.VehicleEditorActivityView));
                intent.PutExtra(EXTRA_VEHICLE_ID, 0);
                intent.PutExtra(EXTRA_FUNCTION, 1); //Funciones: "1": Editar; "2": Ver;
                StartActivityForResult(intent, IDENTIFICADOR_VEHICLE);
            };
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == IDENTIFICADOR_VEHICLE)
            {
                if (resultCode == RESULT_OK)
                {
                    int respuesta = data.GetIntExtra(BANDERA_RESPUESTA, -1);
                    Log.Info(TAG, "El valor de respuesta es: " + respuesta);
                    if (respuesta == 1)
                    {
                        refreshVehiclesList();
                    }
                }
            }
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
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar_sync, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            switch (id)
            {
                case Resource.Id.action_sync:
                    refreshVehiclesList();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void cargarVehiculos(string rawStatus)
        {
            Log.Info(TAG, "Entro en cargarVehiculos con estado:" + rawStatus);

            int status;

            switch (rawStatus)
            {
                case "Estado 1":
                    status = 0;
                    break;
                case "Estado 2":
                    status = 1;
                    break;
                case "Estado 3":
                    status = 2;
                    break;
                default:
                    status = 100;
                    break;
            }

            List<int> ListaIdVehiculos = new List<int>();           

            int NumFilas = 0;

            List<Vehicle> VehiculosBDI = mViewModel.ObtenerListaVehiculosBDI();

            try
            {
                NumFilas = VehiculosBDI.Count;
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
                    Log.Info(TAG, "El ID de la fila es: " + VehiculosBDI[a].IdVehiculo);
                    ListaIdVehiculos.Add(VehiculosBDI[a].IdVehiculo);
                }
                else
                {
                    Log.Info(TAG, "Fila a Evaluar: " + a);
                    Vehicle VIV = VehiculosBDI[a];
                    Log.Info(TAG, "El estado de la fila es: " + VIV.Estado);
                    if (status == VIV.Estado)
                    {
                        Log.Info(TAG, "Se agrega a la lista el vehiculos de ID: " + VIV.IdVehiculo);
                        ListaIdVehiculos.Add(VIV.IdVehiculo);
                    }
                }
            }

            Log.Info(TAG, "Los IDs de Items a agregar son: " + ListaIdVehiculos);

            if (ListaIdVehiculos.Count > 0)
            {
                Log.Info(TAG, "Se actualizara la lista de Vehiculos");
                List<Vehicle> Lista_Vehiculos = new List<Vehicle>();
                for (int a = 0; a < ListaIdVehiculos.Count; a++)
                {
                    Log.Info(TAG, "Agregando Vehiculo de fila: " + ListaIdVehiculos[a]);
                    Vehicle VIV = VehiculosBDI.Find(x => x.IdVehiculo == ListaIdVehiculos[a]);
                    Lista_Vehiculos.Add(VIV);
                }
                showVehicleList(Lista_Vehiculos);
            }
            else
            {
                Log.Info(TAG, "No hay Vehiculo del estado seleccionado");
                showNoVehicle();
            }
        }

        public void refreshVehiclesList()
        {
            Log.Info(TAG, "refreshVehiclesList");
            mViewModel.ObtenerListaVehiculosBDE();
        }

        public void errorRefreshnig()
        {
            Log.Info(TAG, "errorRefreshnig");
            Toast.MakeText(this, "Hubo un error al descargar la lista de vehiculos", ToastLength.Long).Show();
        }

        public void showProgress(bool mostrar)
        {
            mVehiculo.Visibility = mostrar ? ViewStates.Gone : ViewStates.Visible;
            mEmptyStateContainer.Visibility = mostrar ? ViewStates.Gone : ViewStates.Visible;
            Progreso.Visibility = mostrar ? ViewStates.Visible : ViewStates.Gone;
        }

        public void showVehicleList(List<Vehicle> listaVehiculos)
        {
            listItemVehicleAdapter.swapItems(listaVehiculos);

            mVehiculo.Visibility = ViewStates.Visible;
            mEmptyStateContainer.Visibility = ViewStates.Gone;
            Progreso.Visibility = ViewStates.Gone;
        }

        public void showNoVehicle()
        {
            mVehiculo.Visibility = ViewStates.Gone;
            mEmptyStateContainer.Visibility = ViewStates.Visible;
            Progreso.Visibility = ViewStates.Gone;
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



        public class ListItemVehicleAdapter : RecyclerView.Adapter
        {
            private List<Vehicle> mItems;
            private Context mContext;
            public event EventHandler<int> ItemClick;

            public ListItemVehicleAdapter(Context context, List<Vehicle> items)
            {
                mContext = context;
                mItems = items;
            }

            void OnClick(int position)
            {
                if (ItemClick != null) ItemClick(this, position);
            }

            public void swapItems(List<Vehicle> vehicle)
            {
                if (vehicle == null)
                {
                    mItems = new List<Vehicle>();
                }
                else
                {
                    mItems = vehicle;
                    Log.Info(TAG, "Los items nuevos son: " + mItems);
                }
                NotifyDataSetChanged();
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                Log.Info(TAG, "Preparando posicion: " + position);

                Vehicle mVehicle = mItems[position];

                Log.Info(TAG, "El el id del item de la posicion " + position + " es: " + mVehicle.IdVehiculo );

                SimpleViewHolder simpleViewHolder = holder as SimpleViewHolder;

                switch (mVehicle.Estado)
                {
                    case 0:
                        simpleViewHolder.statusIndicator.SetBackgroundResource(Resource.Color.color_estado_1);
                        break;
                    case 1:
                        simpleViewHolder.statusIndicator.SetBackgroundResource(Resource.Color.color_estado_2);
                        break;
                    case 2:
                        simpleViewHolder.statusIndicator.SetBackgroundResource(Resource.Color.color_estado_3);
                        break;
                }

                simpleViewHolder.item1.Text = mVehicle.Placa;
                simpleViewHolder.item2.Text = mVehicle.Area;
                simpleViewHolder.item3.Text = mVehicle.Central;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(mContext);
                View view = layoutInflater.Inflate(Resource.Layout.vehicle_activity_include_listitem, parent, false);


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
            public TextView item1;
            public TextView item2;
            public TextView item3;

            public SimpleViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                statusIndicator = itemView.FindViewById(Resource.Id.V_status_indicator);
                item1 = itemView.FindViewById<TextView>(Resource.Id.TV_List_Item1);
                item2 = itemView.FindViewById<TextView>(Resource.Id.TV_List_Item2);
                item3 = itemView.FindViewById<TextView>(Resource.Id.TV_List_Item3);

                itemView.Click += (sender, e) => listener(base.Position);
            }
        }
    }

    public interface VehicleActivityViewInterface
    {
        void setViewModel(VehicleViewModel viewModel);
        void refreshVehiclesList();
        void errorRefreshnig();
        void showProgress(bool mostrar);
        void showVehicleList(List<Vehicle> listavehiculos);
        void showNoVehicle();
        void ShowDialog(string message, string okText, string cancelText);
    }
}