package md5ce013bcf3062ee1948fb83ecb8b7d3fa;


public class IndicatorView
	extends android.view.View
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.SfKanban.Android.IndicatorView, Syncfusion.SfKanban.Android", IndicatorView.class, __md_methods);
	}


	public IndicatorView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == IndicatorView.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.IndicatorView, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public IndicatorView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == IndicatorView.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.IndicatorView, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public IndicatorView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == IndicatorView.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.IndicatorView, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public IndicatorView (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == IndicatorView.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.IndicatorView, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onDraw (android.graphics.Canvas p0)
	{
		n_onDraw (p0);
	}

	private native void n_onDraw (android.graphics.Canvas p0);

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
