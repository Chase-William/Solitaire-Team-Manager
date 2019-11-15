package md5991b3296029042025ad7970fd527090b;


public class ContributorsViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Solitaire.ContributorsViewHolder, Solitaire", ContributorsViewHolder.class, __md_methods);
	}


	public ContributorsViewHolder ()
	{
		super ();
		if (getClass () == ContributorsViewHolder.class)
			mono.android.TypeManager.Activate ("Solitaire.ContributorsViewHolder, Solitaire", "", this, new java.lang.Object[] {  });
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
