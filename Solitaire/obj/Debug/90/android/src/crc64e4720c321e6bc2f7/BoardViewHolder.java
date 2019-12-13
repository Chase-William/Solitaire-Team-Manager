package crc64e4720c321e6bc2f7;


public class BoardViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Solitaire.BoardViewHolder, Solitaire", BoardViewHolder.class, __md_methods);
	}


	public BoardViewHolder ()
	{
		super ();
		if (getClass () == BoardViewHolder.class)
			mono.android.TypeManager.Activate ("Solitaire.BoardViewHolder, Solitaire", "", this, new java.lang.Object[] {  });
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
