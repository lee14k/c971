using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;

namespace C971.Droid
{
    public class ExactAlarmPermissionService
    {
        public void RequestExactAlarmPermission()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(31)) // Android 12 (API 31) and above
            {
                // Use fully qualified name to avoid ambiguity
                var alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);

                // Check if the app already has the exact alarm permission
                if (!alarmManager.CanScheduleExactAlarms())
                {
                    // Launch system settings to request the exact alarm permission
                    Intent intent = new Intent(Settings.ActionRequestScheduleExactAlarm);
                    intent.SetFlags(ActivityFlags.NewTask); // Ensure it's run as a new task
                    Android.App.Application.Context.StartActivity(intent);
                }
            }
        }
    }
}