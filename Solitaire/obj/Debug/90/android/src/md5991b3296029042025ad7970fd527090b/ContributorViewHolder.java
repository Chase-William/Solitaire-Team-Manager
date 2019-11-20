package md5991b3296029042025ad7970fd527090b;


public class ContributorViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Solitaire.ContributorViewHolder, Solitaire", ContributorViewHolder.class, __md_methods);
	}


	public ContributorViewHolder ()
	{
		super ();
		if (getClass () == ContributorViewHolder.class)
			mono.android.TypeManager.Activate ("Solitaire.ContributorViewHolder, Solitaire", "", this, new java.lang.Object[] {  });
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
