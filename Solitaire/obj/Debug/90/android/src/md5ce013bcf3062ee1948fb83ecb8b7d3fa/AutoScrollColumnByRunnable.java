package md5ce013bcf3062ee1948fb83ecb8b7d3fa;


public class AutoScrollColumnByRunnable
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		java.lang.Runnable
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_run:()V:GetRunHandler:Java.Lang.IRunnableInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.SfKanban.Android.AutoScrollColumnByRunnable, Syncfusion.SfKanban.Android", AutoScrollColumnByRunnable.class, __md_methods);
	}


	public AutoScrollColumnByRunnable ()
	{
		super ();
		if (getClass () == AutoScrollColumnByRunnable.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.AutoScrollColumnByRunnable, Syncfusion.SfKanban.Android", "", this, new java.lang.Object[] {  });
	}

	public AutoScrollColumnByRunnable (md5ce013bcf3062ee1948fb83ecb8b7d3fa.SfKanban p0, md5ce013bcf3062ee1948fb83ecb8b7d3fa.KanbanItemView p1, int p2)
	{
		super ();
		if (getClass () == AutoScrollColumnByRunnable.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.AutoScrollColumnByRunnable, Syncfusion.SfKanban.Android", "Syncfusion.SfKanban.Android.SfKanban, Syncfusion.SfKanban.Android:Syncfusion.SfKanban.Android.KanbanItemView, Syncfusion.SfKanban.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void run ()
	{
		n_run ();
	}

	private native void n_run ();

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
