package md5ce013bcf3062ee1948fb83ecb8b7d3fa;


public class KanbanItemView
	extends android.support.v7.widget.RecyclerView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Syncfusion.SfKanban.Android.KanbanItemView, Syncfusion.SfKanban.Android", KanbanItemView.class, __md_methods);
	}


	public KanbanItemView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == KanbanItemView.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanItemView, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public KanbanItemView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == KanbanItemView.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanItemView, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public KanbanItemView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == KanbanItemView.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanItemView, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
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
