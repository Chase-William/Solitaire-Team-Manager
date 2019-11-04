using Android.Gms.Common.Apis;
using Java.Lang;

namespace Solitaire
{
    public class LoggoutResultCallback : Object, IResultCallback
    {
        public GoogleLoginActivity Activity { get; set; }

        public void OnResult(Object result)
        {
            Activity.UpdateUI(false);
        }
    }
}
