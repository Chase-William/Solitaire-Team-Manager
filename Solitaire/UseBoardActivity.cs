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
        public Board thisBoard;
        private static List<object> AllSupportedCategories = new List<object>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Applying this SfKanban as the content view for the app
            SetContentView(Resource.Layout.use_board_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Your Board";
            SetSupportActionBar(toolbar);                        
            
            

            // Getting the extra "id" we passed which will enable use to reference our Board
            long id = this.Intent.GetLongExtra("Id", -1);
            bool needsInit = this.Intent.GetBooleanExtra("NeedInit", true);

            // If the board needs initalization run:
            if (needsInit)
                InitDefaultBoard();
            // Otherwise load a pre-existing board:
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
                case "Add Deck":

                    new CreateDeckDialog(this);
                    
                    break;
                case "Add Card":

                    new CreateCardDialog(this);

                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        /// 
        /// 
        ///     Will add a card to a specified column within the current working board
        /// 
        /// 
        public void AddCard(string _nameCard, string _descriptionCard, string _category)
        {
            // Needed to create a new collection because just appeneding to the original collection as not working
            ObservableCollection<KanbanModel> tempList = new ObservableCollection<KanbanModel>();            

            foreach (var card in thisBoard.Kanban.ItemsSource)
            {
                tempList.Add((KanbanModel)card);
            }

            tempList.Add(new KanbanModel()
            {
                ID = 1,
                Title = _nameCard,
                // Category is where this card is going to determine which deck this card will be inside of on the GUI
                Category = _category
            });

            thisBoard.Kanban.ItemsSource = tempList;            
        }

        /// 
        /// 
        ///     Will add a column with default values to the current working board
        ///  
        ///
        public void AddDeck(string _nameColumn, string _descriptionColumn)
        {
            AllSupportedCategories.Add(_nameColumn);

            KanbanColumn newDeck = new KanbanColumn(this)
            {
                Title = _nameColumn,
                MinimumLimit = 0,
                MaximumLimit = 10,
                // Categories is a list of all the "categories" this deck supports
                Categories = new List<object>() {  _nameColumn }
            };



            // TODO: Need to add a description area or something




            // Some pretty stuff
            newDeck.ContentDescription = _descriptionColumn;
            newDeck.ErrorBarSettings.Color = Color.Green;
            newDeck.ErrorBarSettings.MinValidationColor = Color.Orange;
            newDeck.ErrorBarSettings.MaxValidationColor = Color.Red;
            newDeck.ErrorBarSettings.Height = 4;


            // We need to add this kanbanWorkflow because it will be used when moving and deciding where cards shall be placed 
            thisBoard.Kanban.Workflows.Add(new KanbanWorkflow()
            {
                Category = _nameColumn,
                AllowedTransitions = AllSupportedCategories
            });
            
           


            thisBoard.Kanban.Columns.Add(newDeck);
        }

        /// 
        /// 
        ///     Creates default board & applies it to the UI
        ///
        ///
        private void InitDefaultBoard()
        {
            // First we need to create a default board
            thisBoard = new Board("Default Name", "Default Description");
            // Then we can assign the kanban instance to ti
            thisBoard.Kanban = FindViewById<SfKanban>(Resource.Id.kanban);

            //thisBoard.Kanban.ColumnMappingPath = "Category";

            // When a card is clicked it will launch a dialog asking for user input on what should be done next
            thisBoard.Kanban.ItemTapped += (e, a) =>
            {
                new ClickedCardOptionsDialog(this);              
            };

            thisBoard.Kanban.ColumnMappingPath = "Category";
            // Initalizing our Workflows collection
            thisBoard.Kanban.Workflows = new List<KanbanWorkflow>();
            // Initalizing our ItemSource collection 
            thisBoard.Kanban.ItemsSource = new ObservableCollection<KanbanModel>();                    
        }

        /// 
        /// 
        ///     Loads an pre-existing board & applies it to the UI
        /// 
        ///
        private void LoadBoard(long _id)
        {
            SfKanban workingKanban = FindViewById<SfKanban>(Resource.Id.kanban);

            workingKanban = thisBoard.Kanban;

            // Need to test this and stuff laters

            thisBoard.Kanban.ColumnMappingPath = "Category";

            KanbanColumn todoColumn = new KanbanColumn(this);
            todoColumn.Title = "To Do";
            todoColumn.MinimumLimit = 5;
            todoColumn.MaximumLimit = 10;
            todoColumn.ErrorBarSettings.Color = Color.Green;
            todoColumn.ErrorBarSettings.MinValidationColor = Color.Orange;
            todoColumn.ErrorBarSettings.MaxValidationColor = Color.Red;
            todoColumn.ErrorBarSettings.Height = 4;
            todoColumn.Categories = new List<object>() { "Open" };
            thisBoard.Kanban.Columns.Add(todoColumn);

            KanbanColumn progressColumn = new KanbanColumn(this);
            progressColumn.Title = "In Progress";
            progressColumn.Categories = new List<object>() { "In Progress" };
            thisBoard.Kanban.Columns.Add(progressColumn);

            KanbanColumn codeColumn = new KanbanColumn(this);
            codeColumn.Title = "Code Review";
            codeColumn.Categories = new List<object>() { "Code Review" };
            thisBoard.Kanban.Columns.Add(codeColumn);

            KanbanColumn doneColumn = new KanbanColumn(this);
            doneColumn.Title = "Done";
            doneColumn.Categories = new List<object>() { "Done" };
            thisBoard.Kanban.Columns.Add(doneColumn);

            thisBoard.Kanban.ItemsSource = ItemsSourceCards();

            List<KanbanWorkflow> workflows = new List<KanbanWorkflow>();

            KanbanWorkflow openWorkflow = new KanbanWorkflow();
            openWorkflow.Category = "Open";
            openWorkflow.AllowedTransitions = new List<object> { "In Progress" };

            KanbanWorkflow progressWorkflow = new KanbanWorkflow();
            progressWorkflow.Category = "In Progress";
            progressWorkflow.AllowedTransitions = new List<object> { "Open", "Code Review", "Closed-No Code Changes" };

            workflows.Add(openWorkflow);
            workflows.Add(progressWorkflow);
            thisBoard.Kanban.Workflows = workflows;

            

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