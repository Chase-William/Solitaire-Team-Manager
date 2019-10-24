package md5ce013bcf3062ee1948fb83ecb8b7d3fa;


public class KanbanRecyclerViewAdapter
	extends android.support.v7.widget.RecyclerView.Adapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getItemCount:()I:GetGetItemCountHandler\n" +
			"n_getItemViewType:(I)I:GetGetItemViewType_IHandler\n" +
			"n_onCreateViewHolder:(Landroid/view/ViewGroup;I)Landroid/support/v7/widget/RecyclerView$ViewHolder;:GetOnCreateViewHolder_Landroid_view_ViewGroup_IHandler\n" +
			"n_onBindViewHolder:(Landroid/support/v7/widget/RecyclerView$ViewHolder;I)V:GetOnBindViewHolder_Landroid_support_v7_widget_RecyclerView_ViewHolder_IHandler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.SfKanban.Android.KanbanRecyclerViewAdapter, Syncfusion.SfKanban.Android", KanbanRecyclerViewAdapter.class, __md_methods);
	}


	public KanbanRecyclerViewAdapter ()
	{
		super ();
		if (getClass () == KanbanRecyclerViewAdapter.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanRecyclerViewAdapter, Syncfusion.SfKanban.Android", "", this, new java.lang.Object[] {  });
	}

	public KanbanRecyclerViewAdapter (md5ce013bcf3062ee1948fb83ecb8b7d3fa.SfKanban p0, md5ce013bcf3062ee1948fb83ecb8b7d3fa.KanbanColumn p1)
	{
		super ();
		if (getClass () == KanbanRecyclerViewAdapter.class)
			mono.android.TypeManager.Activate ("Syncfusion.SfKanban.Android.KanbanRecyclerViewAdapter, Syncfusion.SfKanban.Android", "Syncfusion.SfKanban.Android.SfKanban, Syncfusion.SfKanban.Android:Syncfusion.SfKanban.Android.KanbanColumn, Syncfusion.SfKanban.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public int getItemCount ()
	{
		return n_getItemCount ();
	}

	private native int n_getItemCount ();


	public int getItemViewType (int p0)
	{
		return n_getItemViewType (p0);
	}

	private native int n_getItemViewType (int p0);


	public android.support.v7.widget.RecyclerView.ViewHolder onCreateViewHolder (android.view.ViewGroup p0, int p1)
	{
		return n_onCreateViewHolder (p0, p1);
	}

	private native android.support.v7.widget.RecyclerView.ViewHolder n_onCreateViewHolder (android.view.ViewGroup p0, int p1);


	public void onBindViewHolder (android.support.v7.widget.RecyclerView.ViewHolder p0, int p1)
	{
		n_onBindViewHolder (p0, p1);
	}

	private native void n_onBindViewHolder (android.support.v7.widget.RecyclerView.ViewHolder p0, int p1);

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
