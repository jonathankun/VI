using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

namespace VInspection.Clases.Tables
{
    [Activity(Label = "Pre-uso Vehicular", Theme = "@style/AppTheme.Normal")]
    public class TablesActivityView : BaseActivity, TablesFragmentCallbacks, DataConfirmationListener
    {
        public const string EXTRA_STRING_USER_NAME = "USER_NAME";

        private const string ESTADO_SECCION_ACTUAL = "SECCION_ACTUAL";

        public const int DATE_PICKER_SOAT = 0;
        public const int DATE_PICKER_REVTECNICA = 1;

        public const int CODE_DIALOG_DISCART = 1;
        public const int CODE_DIALOG_GET = 2;
        public const int CODE_DIALOG_SET = 3;
        public const int CODE_DIALOG_ALERT = 4;

        bool mResultFalg = false;

        TabLayout tabLayout;
        NonSwipableViewPager mViewPager;

        private View mProgreso;
        private View mTextoVacio;
        private View mBottomBar;

        private int mCurrentPage = 0;

        Button B_Cancel, B_Accept;

        Context context = Application.Context;

        private SectionPagerAdapter mFragmentAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.tab_tables_activity);

            mViewPager = FindViewById<NonSwipableViewPager>(Resource.Id.vp_fragmentos_tablas);//VIEWPAGE
            mViewPager.CurrentItem = mCurrentPage;
            
            mFragmentAdapter = new SectionPagerAdapter(SupportFragmentManager);
            mViewPager.Adapter = mFragmentAdapter;

            tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(mViewPager);
            tabLayout.TabMode = TabLayout.ModeScrollable;
            tabLayout.Enabled = false;
            tabLayout.Clickable = false;

            //Configuración de conplementos del fragmento
            mTextoVacio = FindViewById(Resource.Id.tv_vacio);
            mProgreso = FindViewById(Resource.Id.pb_progreso);
            mBottomBar = FindViewById(Resource.Id.fl_bottom_bar);

            B_Accept = FindViewById<Button>(Resource.Id.b_accept);
            B_Accept.Click += (o, e) =>
            {
                ((TablesFragmentView)mFragmentAdapter.getRegisteredFragment(mViewPager.CurrentItem)).validateDataForNext();
            };

            B_Cancel = FindViewById<Button>(Resource.Id.b_cancel);
            B_Cancel.Click += (o, e) =>
            {
                ((TablesFragmentView)mFragmentAdapter.getRegisteredFragment(mViewPager.CurrentItem)).validateDataForPrev();
            };

            mViewPager.PageScrolled += (sender, e) =>
            {
                B_Accept.Text = getPositiveText();
                B_Cancel.Text = getNegativeText();
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
            mViewPager.CurrentItem = mCurrentPage;
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt(ESTADO_SECCION_ACTUAL, mViewPager.CurrentItem);
            mCurrentPage = mViewPager.CurrentItem;
        }

        public void showProgress(bool show)
        {
            mProgreso.Visibility = show? ViewStates.Visible : ViewStates.Gone;
            mTextoVacio.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            mViewPager.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
            mBottomBar.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
        }

        public void showDateDialog(int id)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    if (id == 0)
                    {
                        TablesFragmentView fragment = (TablesFragmentView)mFragmentAdapter.getRegisteredFragment(mViewPager.CurrentItem);
                        fragment.setdateSOAT(time);
                    }
                    else
                    {
                        TablesFragmentView fragment = (TablesFragmentView)mFragmentAdapter.getRegisteredFragment(mViewPager.CurrentItem);
                        fragment.setdateRevTecnica(time);
                    }
                });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        public Toolbar getToolbar()
        {
            return null;
        }

        public bool onOptionsItemSelected(IMenuItem menuItem)
        {
            switch (menuItem.ItemId)
            {
                case Resource.Id.nav_inicio:
                    //new DialogoConfirmacionDatos().show(getSupportFragmentManager(), "DialogoConfirmacion");
                    showDialog(CODE_DIALOG_DISCART, "¿Desea descartar los datos ingresados?", "Aceptar", "Cancelar");
                    return true;
            }
            return base.OnOptionsItemSelected(menuItem);
        }

        public override void Finish()
        {
            if (mResultFalg == true)
            {
                Log.Info(TAG, "Resultado OK");
                Intent intent = new Intent();
                intent.PutExtra(Home.HomeActivityView.BANDERA_RESPUESTA, 1);
                SetResult(Home.HomeActivityView.RESULT_OK, intent);
            }
            else
            {
                Log.Info(TAG, "Resultado Cancelado");
                Intent intent = new Intent();
                intent.PutExtra(Home.HomeActivityView.BANDERA_RESPUESTA, 0);
                SetResult(Home.HomeActivityView.RESULT_OK, intent);
            }
            base.Finish();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnBackPressed()
        {
            showDialog(CODE_DIALOG_DISCART, "¿Desea descartar los datos ingresados?", "Aceptar", "Cancelar");
            //base.OnBackPressed();
        }

        private string getPositiveText()
        {
            if (mViewPager.CurrentItem + 1 == 5)
            {
                return GetString(Resource.String.guardar);
            }
            else
            {
                return GetString(Resource.String.siguiente);
            }
        }

        private string getNegativeText()
        {
            if (mViewPager.CurrentItem + 1 == 1)
            {
                return GetString(Resource.String.cancelar);
            }
            else
            {
                return GetString(Resource.String.atras);
            }
        }

        public void goToNex()
        {
            mViewPager.CurrentItem = mViewPager.CurrentItem + 1;
        }

        public void goToPrev()
        {
            mViewPager.CurrentItem = mViewPager.CurrentItem - 1;
        }

        public void showDialog(int requestCode, string message, string okText, string cancelText)
        {
            DataConfirmationDialog frag = DataConfirmationDialog.NewInstance(requestCode , message, okText, cancelText, this);
            frag.Show(FragmentManager, message);
        }

        public void OnDataSetAccept(int requestCode)
        {
            switch (requestCode)
            {
                case CODE_DIALOG_DISCART:
                    Finish();
                    break;
                case CODE_DIALOG_SET:
                    //Guardar datos para cuando tengamos Wifi
                    break;
                case CODE_DIALOG_GET:
                    Finish();
                    break;
                case CODE_DIALOG_ALERT:
                    //Sin accion
                    break;
            }
        }

        public void OnDataSetCancel(int requestCode)
        {
            switch (requestCode)
            {
                case CODE_DIALOG_DISCART:
                    //Sin accion
                    break;
                case CODE_DIALOG_SET:
                    Finish();
                    break;
                case CODE_DIALOG_GET:
                    //No Existe opcion
                    break;
                case CODE_DIALOG_ALERT:
                    //No Existe opcion
                    break;
            }
        }

        public void raiseResultFlag(bool flag)
        {
            mResultFalg = flag;
        }

        public void finishActivity()
        {
            Log.Info(TAG, "Ingreso a finishActivity");
            Finish();
        }

        public class SectionPagerAdapter : FragmentPagerAdapter
        //FragmentStatePagerAdapter
        {
            public Context ApplicationContext { get; private set; }

            // Sparse array to keep track of registered fragments in memory
            private SparseArray<SupportFragment> registeredFragments = new SparseArray<SupportFragment>();

            public SectionPagerAdapter(SupportFragmentManager fm) : base(fm)
            {

            }

            public override SupportFragment GetItem(int position)
            {
                TablesFragmentView tablesFragmentView = TablesFragmentView.NewInstance(position);

                TablesInteractor tablesInteractor = new TablesInteractor(ApplicationContext);
                new TablesViewModel(position, tablesFragmentView, tablesInteractor);
                return tablesFragmentView;
            }

            public override int Count
            {
                get { return 5; }
            }

            public override ICharSequence GetPageTitleFormatted(int position)
            {
                string Title = Utils.UServices.getTitle(position);
                return new Java.Lang.String(Title);
            }

            // Register the fragment when the item is instantiated
            public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
            {
                SupportFragment fragment = (SupportFragment)base.InstantiateItem(container, position);
                registeredFragments.Put(position, fragment);
                return fragment;
            }

            // Unregister when the item is inactive
            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
            {
                registeredFragments.Remove(position);
                base.DestroyItem(container, position, @object);
            }

            public SupportFragment getRegisteredFragment(int position)
            {
                return registeredFragments.Get(position);
            }
        }
    }
}