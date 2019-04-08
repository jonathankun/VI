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
using static VInspection.Clases.UsersEditor.UserEditorActivityView;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using VInspection.Clases.Utils;

namespace VInspection.Clases.Users
{
    [Activity(Label = "Usuarios", Icon = "@drawable/icon", Theme = "@style/AppTheme.Normal")]
    public class UserActivityView : BaseActivity, UserActivityViewInterface
    {
        public FloatingActionButton FAB_New_User;

        private RecyclerView mUser;
        private LinearLayout mEmptyStateContainer;
        RecyclerView.LayoutManager mLayoutManager;
        ListItemUserAdapter listItemUserAdapter;
        private Spinner mStatusFilterSpinner;
        ArrayAdapter<string> statusFilterAdapter;
        private ProgressBar Progreso;

        public const string EXTRA_USER_ID = "ID_USER";
        public const string EXTRA_FUNCTION = "FUNCTION";

        public const int IDENTIFICADOR_USER = 2;

        private const int CODE_DIALOG_REFRESH = 1;

        public static List<string> STATES_VALUES = new List<string> { "Todos", "Responsables", "Supervisores", "Vigilantes", "Conductores" };

        Context context = Application.Context;

        public UserInteractor mInteractor;
        public UserViewModel mViewModel { get; private set; }

        public void setViewModel(UserViewModel viewModel)
        {
            mViewModel = viewModel;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.user_activity);

            mInteractor = new UserInteractor(ApplicationContext);
            mViewModel = new UserViewModel(this, mInteractor);

            SupportToolbar toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "";

            mStatusFilterSpinner = FindViewById<Spinner>(Resource.Id.toolbar_spinner);

            statusFilterAdapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleSpinnerItem, STATES_VALUES);

            statusFilterAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            mStatusFilterSpinner.Adapter = statusFilterAdapter;

            mStatusFilterSpinner.ItemSelected += (o, e) =>
            {
                String status = mStatusFilterSpinner.GetItemAtPosition(e.Position).ToString();
                Log.Info(TAG, "Se seleccionó: " + status);
                cargarUsuarios(status);
            };

            mEmptyStateContainer = FindViewById<LinearLayout>(Resource.Id.empty_state_container);
            Progreso = FindViewById<ProgressBar>(Resource.Id.pb_progreso);

            mUser = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            // Plug in the linear layout manager:
            mLayoutManager = new LinearLayoutManager(this);
            mUser.SetLayoutManager(mLayoutManager);
            // Plug in my adapter:
            listItemUserAdapter = new ListItemUserAdapter(this, new List<User>());

            showUserList(mViewModel.ObtenerListaUsuariosBDI());

            listItemUserAdapter.ItemClick += OnItemClick;

            mUser.SetAdapter(listItemUserAdapter);

            Preparar_FloatingActionButton();

        }

        public void OnItemClick(object sender, int position)
        {
            List<User> users = mViewModel.ObtenerListaUsuariosBDI();

            User clickedUser = users[position];

            int ID = clickedUser.IdUsuario;

            Log.Info(TAG, "Se selecciono el Vehiculo de ID: " + ID);

            Intent intent = new Intent(context, typeof(UsersEditor.UserEditorActivityView));
            intent.PutExtra(EXTRA_USER_ID, ID);
            intent.PutExtra(EXTRA_FUNCTION, 2); //Funciones: "1": Editar; "2": Ver;
            StartActivityForResult(intent, IDENTIFICADOR_USER);
        }

        public void Preparar_FloatingActionButton()
        {
            FAB_New_User = FindViewById<FloatingActionButton>(Resource.Id.FAB_new_user);

            FAB_New_User.Click += (o, e) =>
            {
                Intent intent = new Intent(context, typeof(UsersEditor.UserEditorActivityView));
                intent.PutExtra(EXTRA_USER_ID, 0);
                intent.PutExtra(EXTRA_FUNCTION, 1); //Funciones: "1": Editar; "2": Ver;
                StartActivityForResult(intent, IDENTIFICADOR_USER);
            };
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == IDENTIFICADOR_USER)
            {
                if (resultCode == RESULT_OK)
                {
                    int respuesta = data.GetIntExtra(BANDERA_RESPUESTA, -1);
                    Log.Info(TAG, "El valor de respuesta es: " + respuesta);
                    if (respuesta == 1)
                    {
                        refreshUsersList();
                    }
                }
            }
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
                    refreshUsersList();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void cargarUsuarios(string rawProfile)
        {
            Log.Info(TAG, "Entro en cargarUsuarios con estado:" + rawProfile);

            string profile;

            //"Todos", "Responsables", "Supervisores", "Vigilantes", "Conductores"

            switch (rawProfile)
            {
                case "Responsables":
                    profile = "Responsable";
                    break;
                case "Supervisores":
                    profile = "Supervisor";
                    break;
                case "Vigilantes":
                    profile = "Vigilante";
                    break;
                case "Conductores":
                    profile = "Conductor";
                    break;
                default:
                    profile = "Todos";
                    break;
            }

            List<int> ListaIdUsuarios = new List<int>();

            int NumFilas = 0;

            List<User> UsuariosBDI = mViewModel.ObtenerListaUsuariosBDI();

            try
            {
                NumFilas = UsuariosBDI.Count;
                Log.Info(TAG, "El numero de filas en la BDI es: " + NumFilas);
            }
            catch (Exception e)
            {
                Log.Info(TAG, e.Message);
            }

            for (int a = 0; a < NumFilas; a++)
            {
                if (profile == "Todos")
                {
                    Log.Info(TAG, "Fila a agregar: " + a);
                    Log.Info(TAG, "El ID de la fila es: " + UsuariosBDI[a].IdUsuario);
                    ListaIdUsuarios.Add(UsuariosBDI[a].IdUsuario);
                }
                else
                {
                    Log.Info(TAG, "Fila a Evaluar: " + a);

                    User VIU = UsuariosBDI[a];
                    Log.Info(TAG, "El estado de la fila es: " + VIU.Perfil);
                    if (profile == VIU.Perfil)
                    {
                        Log.Info(TAG, "Se agrega a la lista el vehiculos de ID: " + VIU.Perfil);
                        ListaIdUsuarios.Add(VIU.IdUsuario);
                    }
                }
            }

            Log.Info(TAG, "Los IDs de Items a agregar son: " + ListaIdUsuarios);

            if (ListaIdUsuarios.Count > 0)
            {
                Log.Info(TAG, "Se actualizara la lista de Vehiculos");
                List<User> Lista_Usuarios = new List<User>();
                for (int a = 0; a < ListaIdUsuarios.Count; a++)
                {
                    Log.Info(TAG, "Agregando Vehiculo de fila: " + ListaIdUsuarios[a]);
                    User VIU = UsuariosBDI.Find(x => x.IdUsuario == ListaIdUsuarios[a]);
                    Lista_Usuarios.Add(VIU);
                }
                showUserList(Lista_Usuarios);
            }
            else
            {
                Log.Info(TAG, "No hay Vehiculo del estado seleccionado");
                showNoUser();
            }
        }

        public void refreshUsersList()
        {
            Log.Info(TAG, "refreshUsersList");
            //mViewModel.ObtenerListaUsuariosBDE();
        }

        public void errorRefreshnig()
        {
            Log.Info(TAG, "errorRefreshnig");
            Toast.MakeText(this, "Hubo un error al descargar la lista de vehiculos", ToastLength.Long).Show();
        }

        public void showProgress(bool mostrar)
        {
            mUser.Visibility = mostrar ? ViewStates.Gone : ViewStates.Visible;
            mEmptyStateContainer.Visibility = mostrar ? ViewStates.Gone : ViewStates.Visible;
            Progreso.Visibility = mostrar ? ViewStates.Visible : ViewStates.Gone;
        }

        public void showUserList(List<User> listausuarios)
        {
            listItemUserAdapter.swapItems(listausuarios);

            mUser.Visibility = ViewStates.Visible;
            mEmptyStateContainer.Visibility = ViewStates.Gone;
            Progreso.Visibility = ViewStates.Gone;
        }

        public void showNoUser()
        {
            mUser.Visibility = ViewStates.Gone;
            mEmptyStateContainer.Visibility = ViewStates.Visible;
            Progreso.Visibility = ViewStates.Gone;
        }

        public void ShowDialog(string message, string okText, string cancelText)
        {
            throw new NotImplementedException();
        }




        public class ListItemUserAdapter : RecyclerView.Adapter
        {
            private List<User> mItems;
            private Context mContext;
            public event EventHandler<int> ItemClick;

            public ListItemUserAdapter(Context context, List<User> items)
            {
                mContext = context;
                mItems = items;
            }

            void OnClick(int position)
            {
                if (ItemClick != null) ItemClick(this, position);
            }

            public void swapItems(List<User> user)
            {
                if (user == null)
                {
                    mItems = new List<User>();
                }
                else
                {
                    mItems = user;
                    Log.Info(TAG, "Los items nuevos son: " + mItems);
                }
                NotifyDataSetChanged();
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                Log.Info(TAG, "Preparando posicion: " + position);

                User mUser = mItems[position];

                Log.Info(TAG, "El el id del item de la posicion " + position + " es: " + mUser.IdUsuario);

                SimpleViewHolder simpleViewHolder = holder as SimpleViewHolder;

                simpleViewHolder.item1.Text = mUser.Nombre;
                simpleViewHolder.item2.Text = mUser.Area;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(mContext);
                View view = layoutInflater.Inflate(Resource.Layout.user_activity_include_listitem, parent, false);


                return new SimpleViewHolder(view, OnClick);
            }

            public override int ItemCount
            {
                get
                {
                    return mItems.Count;
                }
            }

            public string TAG { get; private set; }
        }

        public class SimpleViewHolder : RecyclerView.ViewHolder
        {
            public View image;
            public TextView item1, item2;

            public SimpleViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                image = itemView.FindViewById(Resource.Id.U_CIV_Image);
                item1 = itemView.FindViewById<TextView>(Resource.Id.U_ET_T1);
                item2 = itemView.FindViewById<TextView>(Resource.Id.U_ET_T2);

                itemView.Click += (sender, e) => listener(base.Position);
            }
        }
    }

    public interface UserActivityViewInterface
    {
        void setViewModel(UserViewModel viewModel);
        void refreshUsersList();
        void errorRefreshnig();
        void showProgress(bool mostrar);
        void showUserList(List<User> listausuarios);
        void showNoUser();
        void ShowDialog(string message, string okText, string cancelText);
    }
}