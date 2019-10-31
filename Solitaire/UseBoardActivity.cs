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
using Android.Views.Animations;
using Android.Animation;

namespace Solitaire
{
    [Activity(Label = "UseBoardActivity")]
    public unsafe class UseBoardActivity : AppCompatActivity
    {
        // The board acts as a *pointer to the working board, therefore all changes will occur to the original - NOT A COPY
        public Board thisBoard;
        // The SfKanban is merly used as a way for the user to interact with their board and change its data
        public SfKanban thisKanban;
        // Contains a list off all the categories so we can keep track of all the categories each board needs to support
        private List<object> allSupportedCategories = new List<object>();
        // When we click on a card, we will save which card was clicked
        private long cachedId;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Applying this SfKanban as the content view for the app
            SetContentView(Resource.Layout.use_board_activity);

            // Setting our custom toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Your Board";
            SetSupportActionBar(toolbar);
           
            // Getting the extra "id" we passed which will enable use to reference our Board
            long id = this.Intent.GetLongExtra("Id", -1);
            
            // If the board needs initalization run:
            // We dont need to get the data, if that
            if (this.Intent.HasExtra("NeedInit"))
                InitDefaultBoard(id);
            // Otherwise load a pre-existing board:
            else                            
                LoadBoardIntoKanban(id);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 1 && resultCode == Result.Ok)
            {
                // Querying the correct kanban instance by ID
                KanbanModel kanbanptr = thisKanban.ItemsSource.Cast<KanbanModel>().Single(kanban => kanban.ID == cachedId);

                // Assiging the new values
                kanbanptr.Title = data.GetStringExtra("Name");
                kanbanptr.Description = data.GetStringExtra("Description");
            }
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
        public void AddCard(string _nameCard, string _descriptionCard, string _parentDeck)
        {
            // Needed to create a new collection because just appeneding to the original collection does not work
            // Actually upsets me because I was comparing a string to a string with EXACTLY the same data and it wouldn't add it
            // But if I hard coded the category into program as a literal it would work...
            // Even tryied to making a public string variable that I only used when interacting with the category's incase it was 
            // something todo with the actual pointer or WHATEVER!
            var cardList = new ObservableCollection<KanbanModel>();            
            foreach (var card in thisKanban.ItemsSource) { cardList.Add((KanbanModel)card); }
            cardList.Add(new KanbanModel()
            {
                // Gives all objects it is called on a unique ID
                ID = IdManager.GenerateId(),
                Title = _nameCard,
                // Category determines which workflow inside of a deck this will be placed to start
                Category = _parentDeck,
                // DOCUMENTATION DOESNT STATE WHERE THE DIR STARTS - maybe i need to find where the default image is in the module and dir from there
                ImageURL = "Assets/card_task.png"
            });
            thisKanban.ItemsSource = cardList;            
        }

        /// 
        /// 
        ///     Will add a column with the user's provided values
        ///  
        ///
        public void AddDeck(string _nameColumn, string _descriptionColumn)
        {
            allSupportedCategories.Add(_nameColumn);

            KanbanColumn newDeck = new KanbanColumn(this)
            {
                Title = _nameColumn,
                MinimumLimit = 0,
                MaximumLimit = 10,
                // Categories is a list of all the "categories" this deck supports
                Categories = new List<object>() {  _nameColumn }
            };

            // Some pretty stuff
            newDeck.ContentDescription = _descriptionColumn;
            newDeck.ErrorBarSettings.Color = Color.Green;
            newDeck.ErrorBarSettings.MinValidationColor = Color.Orange;
            newDeck.ErrorBarSettings.MaxValidationColor = Color.Red;
            newDeck.ErrorBarSettings.Height = 4;

            // We need to add this kanbanWorkflow because it will be used when moving and deciding where cards shall be placed 
            thisKanban.Workflows.Add(new KanbanWorkflow()
            {
                Category = _nameColumn,
                AllowedTransitions = allSupportedCategories
            });                       
            thisKanban.Columns.Add(newDeck);
        }

        /// 
        /// 
        ///     Creates default board & applies it to the UI
        ///
        ///
        private void InitDefaultBoard(long _id)
        {
            // Assigning the board to our (Boardptr*) basically which will be the board we will be modifing 
            thisBoard = TestData.boards.Single(board => board.Id == _id);

            // We then get the SfKanban which will be how the user interacts with the board's data
            thisKanban = FindViewById<SfKanban>(Resource.Id.kanban);

            //thisBoard.Kanban.ColumnMappingPath = "Category";

            // When card is clicked it will prompt within a dialog "Details" OR "Edit"
            thisKanban.ItemTapped += (e, a) =>
            {
                // Casting once instead of 3 timers for performance, and basically making a pointer to it
                KanbanModel kanbanModelptr = (KanbanModel)a.Data;
                // caching the id for a lookup that can occur later depending on if the user clicks "edit" or "details" in our dialog we are instanciating
                cachedId = (long)kanbanModelptr.ID;
                // data is the kanban but as a object so we cast it so we can provide the information needed
                new ClickedCardOptionsDialog(this, kanbanModelptr.Title, kanbanModelptr.Description);
            };

            // Initalizing our Workflows collection
            thisKanban.Workflows = new List<KanbanWorkflow>();
            // Initalizing our ItemSource collection 
            thisKanban.ItemsSource = new ObservableCollection<KanbanModel>();                    
        }

        /// 
        /// 
        ///     Loads an pre-existing board & applies it to the UI <>
        /// 
        ///
        private void LoadBoardIntoKanban(long _id)
        {
            // Assign the board instance we desire to our *pointer
            thisBoard = TestData.boards.Single(board => board.Id == _id);

            // Then we can assign the kanban instance to ti
            thisKanban = FindViewById<SfKanban>(Resource.Id.kanban);


            /*
             
                First we init the Decks and their workflows

                Then we init the cards
                
            */

            // Initializing all KanbanColumns with data from the list of decks in board
            foreach (Deck deck in thisBoard.Decks)
            {
                var newDeck = new KanbanColumn(this)
                {
                    Title = deck.Name,
                    ContentDescription = deck.Description,
                    MinimumLimit = 0,
                    MaximumLimit = 5,
                    Categories = new List<object>() { deck.Name }
                };
                newDeck.ErrorBarSettings.Color = Color.Green;
                newDeck.ErrorBarSettings.MinValidationColor = Color.Orange;
                newDeck.ErrorBarSettings.MaxValidationColor = Color.Red;
                newDeck.ErrorBarSettings.Height = 4;
                
                thisKanban.Columns.Add(newDeck);               

                // Initializing the kanban's workflow for each column
                thisKanban.Workflows.Add(new KanbanWorkflow()
                {
                    Category = deck.Name,
                    AllowedTransitions = allSupportedCategories                    
                });
            }

            // Initializing all the KanbanModels with data from the list of cards
            var cardList = new ObservableCollection<KanbanModel>();
            foreach (Card card in thisBoard.Cards)
            {
                cardList.Add(new KanbanModel()
                {
                    ID = card.Id,
                    Title = card.Name,
                    Category = card.ParentDeck                   
                });
            }
            thisKanban.ItemsSource = cardList;








            // trying to animation

            thisKanban.DragOver += (e, a) =>
            {
                // Check this out in documentation
                ObjectAnimator.
                    //thisKanban.ItemsSource.Cast<KanbanModel>().ElementAt(a.TargetIndex).ColorKey = Color.BlueViolet;
                //a.Cancel = false;
                
            };

















            // When card is clicked it will prompt within a dialog "Details" OR "Edit"
            thisKanban.ItemTapped += (e, a) =>
            {
                // Casting once instead of 3 timers for performance, and basically making a pointer to it
                KanbanModel kanbanModelptr = (KanbanModel)a.Data;
                // caching the id for a lookup that can occur later depending on if the user clicks "edit" or "details" in our dialog we are instanciating
                cachedId = (long)kanbanModelptr.ID;
                // data is the kanban but as a object so we cast it so we can provide the information needed
                new ClickedCardOptionsDialog(this, kanbanModelptr.Title, kanbanModelptr.Description);
            };
        }

        /// 
        /// 
        ///     Takes our kanban values and loads them into the working board for saving
        /// 
        /// 
        public void LoadKanbanIntoBoard()
        {

            // Packing our KanbanColumn info into a list to be added onto the board's deck list
            List<Deck> decks = new List<Deck>();
            foreach (KanbanColumn deck in thisKanban.Columns)
            {
                decks.Add(new Deck(deck.Title, deck.ContentDescription));
            }
            thisBoard.Decks = decks;

            // Packing our KanbanModel into the a list to be added to the board's cards list
            List<Card> cards = new List<Card>();
            foreach (KanbanModel card in thisKanban.ItemsSource)
            {
                cards.Add(new Card(card.Title, card.Description, card.Category.ToString()));
            }
            thisBoard.Cards = cards;            
        }

        // Overrinding the back button to save
        /// 
        /// 
        ///     Overriding the back button so we automatically save before we exit
        /// 
        /// 
        public override void OnBackPressed()
        {
            LoadKanbanIntoBoard();
            base.OnBackPressed();
        }
    }
}


// SfKanban workingKanban = FindViewById<SfKanban>(Resource.Id.kanban);

//workingKanban = thisBoard.Kanban;

//            // Need to test this and stuff laters

//            thisBoard.Kanban.ColumnMappingPath = "Category";

//            KanbanColumn todoColumn = new KanbanColumn(this);
//todoColumn.Title = "To Do";
//            todoColumn.MinimumLimit = 5;
//            todoColumn.MaximumLimit = 10;
//            todoColumn.ErrorBarSettings.Color = Color.Green;
//            todoColumn.ErrorBarSettings.MinValidationColor = Color.Orange;
//            todoColumn.ErrorBarSettings.MaxValidationColor = Color.Red;
//            todoColumn.ErrorBarSettings.Height = 4;
//            todoColumn.Categories = new List<object>() { "Open" };
//            thisBoard.Kanban.Columns.Add(todoColumn);

//            KanbanColumn progressColumn = new KanbanColumn(this);
//progressColumn.Title = "In Progress";
//            progressColumn.Categories = new List<object>() { "In Progress" };
//            thisBoard.Kanban.Columns.Add(progressColumn);

//            KanbanColumn codeColumn = new KanbanColumn(this);
//codeColumn.Title = "Code Review";
//            codeColumn.Categories = new List<object>() { "Code Review" };
//            thisBoard.Kanban.Columns.Add(codeColumn);

//            KanbanColumn doneColumn = new KanbanColumn(this);
//doneColumn.Title = "Done";
//            doneColumn.Categories = new List<object>() { "Done" };
//            thisBoard.Kanban.Columns.Add(doneColumn);

//            thisBoard.Kanban.ItemsSource = ItemsSourceCards();

//List<KanbanWorkflow> workflows = new List<KanbanWorkflow>();

//KanbanWorkflow openWorkflow = new KanbanWorkflow();
//openWorkflow.Category = "Open";
//            openWorkflow.AllowedTransitions = new List<object> { "In Progress" };

//            KanbanWorkflow progressWorkflow = new KanbanWorkflow();
//progressWorkflow.Category = "In Progress";
//            progressWorkflow.AllowedTransitions = new List<object> { "Open", "Code Review", "Closed-No Code Changes" };

//            workflows.Add(openWorkflow);
//            workflows.Add(progressWorkflow);
//            thisBoard.Kanban.Workflows = workflows;

            

//            ObservableCollection<KanbanModel> ItemsSourceCards()
//{
//    ObservableCollection<KanbanModel> cards = new ObservableCollection<KanbanModel>();

//    cards.Add(
//        new KanbanModel()
//        {
//            ID = 1,
//            Title = "iOS - 1002",
//            ImageURL = "Image1.png",
//            Category = "Open",
//            Description = "Analyze customer requirements",
//            ColorKey = "Red",
//            Tags = new string[] { "Incident", "Customer" }
//        }
//    );

//    cards.Add(
//        new KanbanModel()
//        {
//            ID = 6,
//            Title = "Xamarin - 4576",
//            ImageURL = "Image2.png",
//            Category = "Open",
//            Description = "Show the retrieved data from the server in grid control",
//            ColorKey = "Green",
//            Tags = new string[] { "SfDataGrid", "Customer" }
//        }
//    );

//    cards.Add(
//        new KanbanModel()
//        {
//            ID = 13,
//            Title = "UWP - 13",
//            ImageURL = "Image4.png",
//            Category = "In Progress",
//            Description = "Add responsive support to application",
//            ColorKey = "Brown",
//            Tags = new string[] { "Story", "Kanban" }
//        }
//    );

//    cards.Add(
//        new KanbanModel()
//        {
//            ID = 2543,
//            Title = "Xamarin_iOS - 2543",
//            Category = "Code Review",
//            ImageURL = "Image12.png",
//            Description = "Provide swimlane support kanban",
//            ColorKey = "Brown",
//            Tags = new string[] { "Feature", "SfKanban" }
//        }
//    );

//    cards.Add(
//        new KanbanModel()
//        {
//            ID = 1975,
//            Title = "iOS - 1975",
//            Category = "Done",
//            ImageURL = "Image11.png",
//            Description = "Fix the issues reported by the customer",
//            ColorKey = "Purple",
//            Tags = new string[] { "Bug" }
//        }
//    );

//    return cards;