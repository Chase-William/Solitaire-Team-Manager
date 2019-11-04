package md5991b3296029042025ad7970fd527090b;


public class ListBoardsFragmentActivity
	extends android.support.v4.app.Fragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"";
		mono.android.Runtime.register ("Solitaire.ListBoardsFragmentActivity, Solitaire", ListBoardsFragmentActivity.class, __md_methods);
	}


	public ListBoardsFragmentActivity ()
	{
		super ();
		if (getClass () == ListBoardsFragmentActivity.class)
			mono.android.TypeManager.Activate ("Solitaire.ListBoardsFragmentActivity, Solitaire", "", this, new java.lang.Object[] {  });
	}

	public ListBoardsFragmentActivity (md5991b3296029042025ad7970fd527090b.MainActivity p0)
	{
		super ();
		if (getClass () == ListBoardsFragmentActivity.class)
			mono.android.TypeManager.Activate ("Solitaire.ListBoardsFragmentActivity, Solitaire", "Solitaire.MainActivity, Solitaire", this, new java.lang.Object[] { p0 });
	}


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);


	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
