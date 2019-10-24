package md5ce013bcf3062ee1948fb83ecb8b7d3fa;


public class KanbanExpandView
	extends android.widget.ImageView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Syncfusion.SfKanban.Android.KanbanExpandView, Syncfusion.SfKanban.Android", KanbanExpandView.class, __md_methods);
	}


	public KanbanExpandView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == KanbanExpandView.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanExpandView, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public KanbanExpandView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == KanbanExpandView.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanExpandView, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public KanbanExpandView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == KanbanExpandView.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanExpandView, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public KanbanExpandView (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == KanbanExpandView.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanExpandView, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}

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
