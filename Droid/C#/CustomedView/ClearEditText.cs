using System;
using Android.Widget;
using Android.Text;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Graphics;
using Android.Views;
using Java.Lang;
using Android.Views.Animations;

namespace CorpTraining.Droid
{
	public class ClearEditText:EditText,Android.Views.View.IOnFocusChangeListener,ITextWatcher
	{
		//删除按钮的引用
		private Drawable mClearDrawable;


		public ClearEditText (Android.Content.Context context, Android.Util.IAttributeSet attrs, int defStyleAttr) : base (context, attrs, defStyleAttr)
		{
			init ();
		}

		public ClearEditText (Android.Content.Context context) : this (context, null)
		{

		}


		public ClearEditText (Android.Content.Context context, Android.Util.IAttributeSet attrs) : this (context, attrs, Android.Resource.Attribute.EditTextStyle)
		{

		}

		public ClearEditText (IntPtr a, JniHandleOwnership b) : base (a, b)
		{
		}


		private void init ()
		{
			//获取EditText的DrawableRight,假如没有设置我们就使用默认的图片
			// 获取EditText的DrawableRight,假如没有设置我们就使用默认的图片,2是获得右边的图片  顺序是左上右下（0,1,2,3,）
			mClearDrawable = GetCompoundDrawables () [2];
			if (mClearDrawable == null) {
				mClearDrawable = Resources.GetDrawable (Resource.Mipmap.emotionstore_progresscancelbtn);
			}
			mClearDrawable.SetBounds (0, 0, mClearDrawable.IntrinsicWidth, mClearDrawable.IntrinsicHeight);
			// 默认设置隐藏图标
			setClearIconVisible (false);
			// 设置焦点改变的监听
			OnFocusChangeListener = this;
			// 设置输入框里面内容发生改变的监听
			AddTextChangedListener (this);
			SetTextColor (Color.Black);
			SetHintTextColor (Color.Gray);
		}

		public override bool OnTouchEvent (Android.Views.MotionEvent e)
		{
			if (GetCompoundDrawables () [2] != null) {
				if (e.Action == MotionEventActions.Up) {
					bool touchable = e.GetX () > (Width
					                 - PaddingRight - mClearDrawable.IntrinsicWidth)
					                 && (e.GetX () < ((Width - PaddingRight)));
					if (touchable) {
						this.Text = "";
					}
				}
			}
			return base.OnTouchEvent (e);
		}

		public void OnFocusChange (View v, bool hasFocus)
		{
			if (hasFocus) { 
				setClearIconVisible (Text.Length > 0); 
			} else { 
				setClearIconVisible (false); 
			} 
		}

		public void AfterTextChanged (IEditable s)
		{

		}

		public void BeforeTextChanged (Java.Lang.ICharSequence s, int start, int count, int after)
		{

		}

		public void OnTextChanged (ICharSequence text, int start, int lengthBefore, int lengthAfter)
		{
			setClearIconVisible (text.Length () > 0); 
		}

		/**
     * 设置清除图标的显示与隐藏，调用setCompoundDrawables为EditText绘制上去
     * @param visible
     */
		protected void setClearIconVisible (bool visible)
		{ 
			Drawable right = visible ? mClearDrawable : null; 
			SetCompoundDrawables (GetCompoundDrawables () [0], 
				GetCompoundDrawables () [1], right, GetCompoundDrawables () [3]); 
		}

		/**
     * 设置晃动动画
     */
		public void setShakeAnimation ()
		{
			this.Animation = shakeAnimation (5);
		}


		/**
     * 晃动动画
     * @param counts 1秒钟晃动多少下
     * @return
     */
		public static Animation shakeAnimation (int counts)
		{
			Animation translateAnimation = new TranslateAnimation (0, 10, 0, 0);
			translateAnimation.Interpolator = new CycleInterpolator (counts);
			translateAnimation.Duration = 1000;
			return translateAnimation;
		}

	}
}

