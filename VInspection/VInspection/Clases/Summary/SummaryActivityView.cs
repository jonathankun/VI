using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using static VInspection.Clases.Home.HomeActivityView;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

namespace VInspection.Clases.Summary
{
    [Activity(Label = "Pre-uso Vehicular", Theme = "@style/AppTheme.Normal")]
    public class SummaryActivityView : BaseActivity, SummaryFragmentCallbacks, DataConfirmationListener
    {
        private const string ESTADO_SECCION_ACTUAL = "SECCION_ACTUAL";

        TabLayout tabLayout;
        private ViewPager mViewPager;
        private View mProgreso;
        private View mTextoVacio;
        private View mBottomBar;

        private int mCheckListId;
        private int mCurrentPage = 0;

        public const string TAG = "DEBUG LOG";

        private const int CODE_DIALOG_SET = 1;

        bool mResultFalg = false;

        private SummaryFragmentAdapter mFragmentAdapter;
        private Button mPositiveButton;
        private Button mNegativeButton;
        private Button mButtonCancel;

        public int mEstado;

        static Context context = Application.Context;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.tab_summary_activity);

            mCheckListId = Intent.GetIntExtra(EXTRA_CHECKLIST_ID, -1);
            Log.Info(TAG, "El ID es: " + mCheckListId);
            mEstado = Intent.GetIntExtra(EXTRA_CHECKLIST_STATE, -1);
            Log.Info(TAG, "El estado es: " + mEstado);


            mViewPager = FindViewById<ViewPager>(Resource.Id.vp_fragmentos_resumen);
            mViewPager.CurrentItem = mCurrentPage;
            mFragmentAdapter = new SummaryFragmentAdapter(SupportFragmentManager, mCheckListId);
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

            mPositiveButton = FindViewById<Button>(Resource.Id.b_accept);
            mNegativeButton = FindViewById<Button>(Resource.Id.b_cancel);
            mPositiveButton.Text = "VALIDAR";
            mNegativeButton.Text = "RECHAZAR";

            if (mEstado != 0)
            {
                mPositiveButton.Enabled = false;
                mNegativeButton.Enabled = false;
                //mPositiveButton.SetTextColor(GetColorStateList(Resource.Color.skp_textgrey));
                //mNegativeButton.SetTextColor(GetColorStateList(Resource.Color.skp_textgrey));
            }
            else
            {
                mPositiveButton.Enabled = true;
                mNegativeButton.Enabled = true;
                //mPositiveButton.SetTextColor(GetColorStateList(Resource.Color.skp_blue));
                //mNegativeButton.SetTextColor(GetColorStateList(Resource.Color.skp_darkblue));
            }

            mPositiveButton.Click += (o, e) =>
            {
                if (mViewPager.CurrentItem != 5)
                {   
                    mViewPager.CurrentItem = 5;
                }
                else
                {
                    ((SummaryFragmentView)mFragmentAdapter.getRegisteredFragment(mViewPager.CurrentItem)).validateCheckListSummary();
                }
            };

            mNegativeButton.Click += (o, e) =>
            {
                //Finish();
                if (mViewPager.CurrentItem != 5)
                {
                    mViewPager.CurrentItem = 5;
                }
                else
                {
                    ((SummaryFragmentView)mFragmentAdapter.getRegisteredFragment(mViewPager.CurrentItem)).refuseCheckListSummary();
                }
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

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt(ESTADO_SECCION_ACTUAL, mViewPager.CurrentItem);
            mCurrentPage = mViewPager.CurrentItem;
        }

        public void showProgress(bool show)
        {
            mProgreso.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            mTextoVacio.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            mViewPager.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
            mBottomBar.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
        }

        public void finishActivity()
        {
            Log.Info(TAG, "Ingreso a finishActivity");
            Finish();
        }

        public void raiseResultFlag(bool flag)
        {
            mResultFalg = flag;
        }

        public void showDialog(string message, string okText, string cancelText)
        {
            DataConfirmationDialog frag = DataConfirmationDialog.NewInstance(CODE_DIALOG_SET, message, okText, cancelText, this);
            frag.Show(FragmentManager, message);
        }

        public void OnDataSetAccept(int requestCode)
        {
            if (requestCode == CODE_DIALOG_SET)
            {
                //Sin Accion
            }
        }

        public void OnDataSetCancel(int requestCode)
        {
            if (requestCode == CODE_DIALOG_SET)
            {
                //No hay opcion
            }
        }


        public class SummaryFragmentAdapter : FragmentStatePagerAdapter
        {
            private SparseArray<SupportFragment> registeredFragments = new SparseArray<SupportFragment>();


            private int mCheckListId;

            public SummaryFragmentAdapter(SupportFragmentManager fm, int checkListId) : base(fm)
            {
                mCheckListId = checkListId;
            }

            public override SupportFragment GetItem(int position)
            {
                SummaryFragmentView summaryFragmentView = SummaryFragmentView.NewInstance(position, mCheckListId);

                SummaryInteractor sumaryInteractor = new SummaryInteractor(context);
                new SummaryViewModel(summaryFragmentView, sumaryInteractor);
                return summaryFragmentView;
            }

            public override int Count
            {
                get { return 6; }
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