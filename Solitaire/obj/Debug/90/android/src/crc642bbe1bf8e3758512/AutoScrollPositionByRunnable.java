package crc642bbe1bf8e3758512;


public class AutoScrollPositionByRunnable
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
		mono.android.Runtime.register ("Syncfusion.SfKanban.Android.AutoScrollPositionByRunnable, Syncfusion.SfKanban.Android", AutoScrollPositionByRunnable.class, __md_methods);
	}


	public AutoScrollPositionByRunnable ()
	{
		super ();
		if (getClass () == AutoScrollPositionByRunnable.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.AutoScrollPositionByRunnable, Syncfusion.SfKanban.Android", "", this, new java.lang.Object[] {  });
	}

	public AutoScrollPositionByRunnable (crc642bbe1bf8e3758512.SfKanban p0, int p1, int p2, crc642bbe1bf8e3758512.KanbanItemView p3)
	{
		super ();
		if (getClass () == AutoScrollPositionByRunnable.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.AutoScrollPositionByRunnable, Syncfusion.SfKanban.Android", "Syncfusion.SfKanban.Android.SfKanban, Syncfusion.SfKanban.Android:System.Int32, mscorlib:System.Int32, mscorlib:Syncfusion.SfKanban.Android.KanbanItemView, Syncfusion.SfKanban.Android", this, new java.lang.Object[] { p0, p1, p2, p3 });
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
