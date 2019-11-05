using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Syncfusion.SfKanban.Android;

namespace Solitaire.CustomGestures
{
    public class DoubleClickGesture 
    {
        private const double WAIT_FOR_DOUBLE_CLICK_THRESHOLD = 150;
        public Timer timer = new Timer(WAIT_FOR_DOUBLE_CLICK_THRESHOLD);
        public UseBoardActivity callerInstance;
        static DoubleClickGesture() { }

        public void InitDoubleClickGesture(UseBoardActivity _context)
        {
            callerInstance = _context;

            // This will fire if the user hasn't signaled a double click in a specific amount of time
            // Idk if this is the right way to do double clicks but it's what I thought of
            // To improve this, apon first click we could use the 150ms to init the activity for a single click
            // That would be done on a seperate thread but its unlikey I will do that for the mvp
            timer.Elapsed += delegate
            {
                // If the timer has reached its interval then we are going to start the single click activity which is details..
                // Therefore we need to stop the timer
                timer.Stop();
                // Reseting the callerInstance boolean that tracks which click is active (first or second)
                callerInstance.clickIdentifier = true;
                // For this intent we only pass the kanbanModel Id because we dont want to edit the board, only the Sfkanban
                Intent showDetailsActivity = new Intent(callerInstance, typeof(DetailsCardActivity));
                showDetailsActivity.PutExtra("kanbanModelId", callerInstance.clickedKanbanModelId);
                callerInstance.StartActivityForResult(showDetailsActivity, callerInstance.DETAILS_ACTIVITY_CODE);
            };
        }     
    }
}