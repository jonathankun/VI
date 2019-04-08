using Android.Content;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;

namespace VInspection.Clases
{
    public class NonSwipableViewPager : ViewPager
    {
        public NonSwipableViewPager(Context context) : base(context) { }

        public NonSwipableViewPager(Context context, IAttributeSet attrs) : base(context, attrs) { }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return false;
        }
    }
}