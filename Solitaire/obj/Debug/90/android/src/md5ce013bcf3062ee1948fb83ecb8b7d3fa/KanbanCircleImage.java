package md5ce013bcf3062ee1948fb83ecb8b7d3fa;


public class KanbanCircleImage
	extends android.widget.ImageView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.SfKanban.Android.KanbanCircleImage, Syncfusion.SfKanban.Android", KanbanCircleImage.class, __md_methods);
	}


	public KanbanCircleImage (android.content.Context p0)
	{
		super (p0);
		if (getClass () == KanbanCircleImage.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanCircleImage, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public KanbanCircleImage (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == KanbanCircleImage.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanCircleImage, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public KanbanCircleImage (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == KanbanCircleImage.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanCircleImage, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public KanbanCircleImage (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == KanbanCircleImage.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanCircleImage, Syncfusion.SfKanban.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
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
