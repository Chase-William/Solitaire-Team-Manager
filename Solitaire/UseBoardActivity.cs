using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Syncfusion.SfKanban.Android;
using Solitaire.Lang;
using Android.Support.V7.App;

namespace Solitaire
{
    [Activity(Label = "UseBoardActivity")]
    public class UseBoardActivity : AppCompatActivity
    {
        Board thisBoard;
        SfKanban thisKanban;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Applying this SfKanban as the content view for the app
            SetContentView(Resource.Layout.use_board_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Your Board";
            SetSupportActionBar(toolbar);

            thisKanban = FindViewById<SfKanban>(Resource.Id.kanban);
            

            // Getting the extra "id" we passed which will enable use to reference our Board
            long id = this.Intent.GetLongExtra("Id", -1);
            
            // -1 will signify that this is a new board and that we need to generate it's default setup
            if (id == -1)
                InitDefaultBoard();
            // Otherwise load a pre-existing board
            else                            
                LoadBoard(id);

            
        }

        ///
        /// 
        ///     Initalizes & Applies our custom toolbar
        /// 
        ///
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.use_board_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        ///
        /// 
        ///     Determines which toolbar button was pressed
        /// 
        ///
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Comparing the title of the toolbar btns to a string to determine which was clicked
            switch (item.TitleFormatted.ToString())
            {
                // If Add Column is clicked we need to create a new default column
                case "Add Column":

                    CreateDeckDialog d = new CreateDeckDialog(this);
                    d.Show();

                    //var instructorDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                    //instructorDialog.SetMessage($"{InstructorData.instructors[e.Position].InstructorName}\n{InstructorData.instructors[e.Position].Proficiency}");
                    //instructorDialog.SetNeutralButton("Ok", delegate { });
                    //instructorDialog.Show();
                    AddColumn();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        /// 
        /// 
        ///     Will add a column with default values to the Sfkanban
        /// 
        ///
        private void AddColumn()
        {
            thisKanban.ColumnMappingPath = "Category";

            KanbanColumn todoColumn = new KanbanColumn(this)
            {
                Title = "Default Title",
                MinimumLimit = 0,
                MaximumLimit = 10,
                Categories = new List<object>() { "Default Categories" }
            };
            todoColumn.ErrorBarSettings.Color = Color.Green;
            todoColumn.ErrorBarSettings.MinValidationColor = Color.Orange;
            todoColumn.ErrorBarSettings.MaxValidationColor = Color.Red;
            todoColumn.ErrorBarSettings.Height = 4;
            thisKanban.Columns.Add(todoColumn);
        }

        /// 
        /// 
        ///     Creates default board & applies it to the UI
        ///
        ///
        private void InitDefaultBoard()
        {
            thisBoard = new Board("Default Name", "Default Description");
            
            thisKanban.ColumnMappingPath = "Category";

            KanbanColumn todoColumn = new KanbanColumn(this)
            {
                Title = "Default Title",
                MinimumLimit = 0,
                MaximumLimit = 10,
                Categories = new List<object>() { "Default Categories"}
            };
            todoColumn.ErrorBarSettings.Color = Color.Green;
            todoColumn.ErrorBarSettings.MinValidationColor = Color.Orange;
            todoColumn.ErrorBarSettings.MaxValidationColor = Color.Red;
            todoColumn.ErrorBarSettings.Height = 4;
            thisKanban.Columns.Add(todoColumn);

            // Assigning this kanban to the board it belongs to
            thisBoard.SfBoard = thisKanban;           
        }

        /// 
        /// 
        ///     Loads an pre-existing board & applies it to the UI
        /// 
        ///
        private void LoadBoard(long _id)
        {

            thisKanban.ColumnMappingPath = "Category";

            KanbanColumn todoColumn = new KanbanColumn(this);
            todoColumn.Title = "To Do";
            todoColumn.MinimumLimit = 5;
            todoColumn.MaximumLimit = 10;
            todoColumn.ErrorBarSettings.Color = Color.Green;
            todoColumn.ErrorBarSettings.MinValidationColor = Color.Orange;
            todoColumn.ErrorBarSettings.MaxValidationColor = Color.Red;
            todoColumn.ErrorBarSettings.Height = 4;
            todoColumn.Categories = new List<object>() { "Open" };
            thisKanban.Columns.Add(todoColumn);

            KanbanColumn progressColumn = new KanbanColumn(this);
            progressColumn.Title = "In Progress";
            progressColumn.Categories = new List<object>() { "In Progress" };
            thisKanban.Columns.Add(progressColumn);

            KanbanColumn codeColumn = new KanbanColumn(this);
            codeColumn.Title = "Code Review";
            codeColumn.Categories = new List<object>() { "Code Review" };
            thisKanban.Columns.Add(codeColumn);

            KanbanColumn doneColumn = new KanbanColumn(this);
            doneColumn.Title = "Done";
            doneColumn.Categories = new List<object>() { "Done" };
            thisKanban.Columns.Add(doneColumn);

            thisKanban.ItemsSource = ItemsSourceCards();

            List<KanbanWorkflow> workflows = new List<KanbanWorkflow>();

            KanbanWorkflow openWorkflow = new KanbanWorkflow();
            openWorkflow.Category = "Open";
            openWorkflow.AllowedTransitions = new List<object> { "In Progress" };

            KanbanWorkflow progressWorkflow = new KanbanWorkflow();
            progressWorkflow.Category = "In Progress";
            progressWorkflow.AllowedTransitions = new List<object> { "Open", "Code Review", "Closed-No Code Changes" };

            workflows.Add(openWorkflow);
            workflows.Add(progressWorkflow);
            thisKanban.Workflows = workflows;

            

            ObservableCollection<KanbanModel> ItemsSourceCards()
            {
                ObservableCollection<KanbanModel> cards = new ObservableCollection<KanbanModel>();

                cards.Add(
                    new KanbanModel()
                    {
                        ID = 1,
                        Title = "iOS - 1002",
                        ImageURL = "Image1.png",
                        Category = "Open",
                        Description = "Analyze customer requirements",
                        ColorKey = "Red",
                        Tags = new string[] { "Incident", "Customer" }
                    }
                );

                cards.Add(
                    new KanbanModel()
                    {
                        ID = 6,
                        Title = "Xamarin - 4576",
                        ImageURL = "Image2.png",
                        Category = "Open",
                        Description = "Show the retrieved data from the server in grid control",
                        ColorKey = "Green",
                        Tags = new string[] { "SfDataGrid", "Customer" }
                    }
                );

                cards.Add(
                    new KanbanModel()
                    {
                        ID = 13,
                        Title = "UWP - 13",
                        ImageURL = "Image4.png",
                        Category = "In Progress",
                        Description = "Add responsive support to application",
                        ColorKey = "Brown",
                        Tags = new string[] { "Story", "Kanban" }
                    }
                );

                cards.Add(
                    new KanbanModel()
                    {
                        ID = 2543,
                        Title = "Xamarin_iOS - 2543",
                        Category = "Code Review",
                        ImageURL = "Image12.png",
                        Description = "Provide swimlane support kanban",
                        ColorKey = "Brown",
                        Tags = new string[] { "Feature", "SfKanban" }
                    }
                );

                cards.Add(
                    new KanbanModel()
                    {
                        ID = 1975,
                        Title = "iOS - 1975",
                        Category = "Done",
                        ImageURL = "Image11.png",
                        Description = "Fix the issues reported by the customer",
                        ColorKey = "Purple",
                        Tags = new string[] { "Bug" }
                    }
                );

                return cards;
            }        
        }        
    }
}