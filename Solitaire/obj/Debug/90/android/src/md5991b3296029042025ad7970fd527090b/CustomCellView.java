package md5991b3296029042025ad7970fd527090b;


public class CustomCellView
	extends android.view.View
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getTag:()Ljava/lang/Object;:GetGetTagHandler\n" +
			"n_setTag:(Ljava/lang/Object;)V:GetSetTag_Ljava_lang_Object_Handler\n" +
			"n_setBackgroundResource:(I)V:GetSetBackgroundResource_IHandler\n" +
			"";
		mono.android.Runtime.register ("Solitaire.CustomCellView, Solitaire", CustomCellView.class, __md_methods);
	}


	public CustomCellView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomCellView.class)
			mono.android.TypeManager.Activate ("Solitaire.CustomCellView, Solitaire", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public CustomCellView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomCellView.class)
			mono.android.TypeManager.Activate ("Solitaire.CustomCellView, Solitaire", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomCellView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomCellView.class)
			mono.android.TypeManager.Activate ("Solitaire.CustomCellView, Solitaire", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public CustomCellView (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == CustomCellView.class)
			mono.android.TypeManager.Activate ("Solitaire.CustomCellView, Solitaire", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public java.lang.Object getTag ()
	{
		return n_getTag ();
	}

	private native java.lang.Object n_getTag ();


	public void setTag (java.lang.Object p0)
	{
		n_setTag (p0);
	}

	private native void n_setTag (java.lang.Object p0);


	public void setBackgroundResource (int p0)
	{
		n_setBackgroundResource (p0);
	}

	private native void n_setBackgroundResource (int p0);

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
