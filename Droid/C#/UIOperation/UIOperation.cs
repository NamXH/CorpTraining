using System;

namespace CorpTraining.Droid
{
	public interface UIOperation
	{
		/// <summary>
		/// Initiate the listner
		/// </summary>
		void initListner ();

		/// <summary>
		/// initiate data
		/// </summary>
		void initData ();

		/// <summary>
		/// Inits the view.
		/// </summary>
		void initView ();

		/// <summary>
		/// Gets the layout resource.
		/// </summary>
		/// <returns>The layout resource.</returns>
		int getLayoutResource ();
	}
}

