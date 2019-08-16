package md5917d25482e5da87ec1c39af0d7d5bb9b;


public class TrainingActivity
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onSupportNavigateUp:()Z:GetOnSupportNavigateUpHandler\n" +
			"";
		mono.android.Runtime.register ("Train___Android.TrainingActivity, Train - Android", TrainingActivity.class, __md_methods);
	}


	public TrainingActivity ()
	{
		super ();
		if (getClass () == TrainingActivity.class)
			mono.android.TypeManager.Activate ("Train___Android.TrainingActivity, Train - Android", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public boolean onSupportNavigateUp ()
	{
		return n_onSupportNavigateUp ();
	}

	private native boolean n_onSupportNavigateUp ();

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
