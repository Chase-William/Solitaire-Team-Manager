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
using System.Threading.Tasks;
using System.Timers;
using Solitaire.CustomGestures;

namespace Solitaire
{
    [Activity(Label = "UseBoardActivity")]
    public class UseBoardActivity : AppCompatActivity
    {
        // The board acts as a *pointer to the working board, therefore all changes will occur to the original - NOT A COPY
        public Board thisBoard;
        // The SfKanban is merly used as a way for the user to interact with their board and change its data
        public static SfKanban thisKanban;
        // Contains a list off all the categories so we can keep track of all the categories each board needs to support
        private List<object> allSupportedCategories = new List<object>();
        // When we click on a card, we will save which card was clicked
        public long clickedKanbanModelId;
        // The for our details activity, readonly because accessing this variable through a pointer to 
        // it within DoubleClickGesture will cause an error
        public readonly int DETAILS_ACTIVITY_CODE = 2;
        // Identifies whether the current click is the first click or the second click in a chain of clicks
        public bool clickIdentifier = true;
        // Pointer to our custom double click gesture
        DoubleClickGesture doubleClickGesture;

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
            long boardId = this.Intent.GetLongExtra("BoardId", -1);
            
            // If the board needs initalization run:
            // We dont need to get the data, if that
            if (this.Intent.HasExtra("NeedInit"))
                InitDefaultBoard(boardId);
            // Otherwise load a pre-existing board:
            else                            
                LoadBoardIntoKanban(boardId);

            List<Contributor> ree = new List<Contributor>();
            ree.Add(new Contributor("Kyle Murphy", "ksm3091@rit.edu"));
            ree.Add(new Contributor("ree goddamn", "fuuuuck"));

            //Updates all contributors
            AssetManager.contributors = ree;
            //Updates board contributors
            thisBoard.Contributors = ree;

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
                case "Add Contributor":
                    new ContributorOptionsDialog(this);
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
                Description = _descriptionCard,
                // Category determines which workflow inside of a deck this will be placed to start
                Category = _parentDeck,
                // DOCUMENTATION DOESNT STATE WHERE THE DIR STARTS - maybe i need to find where the default image is in the module and dir from there
                //ImageURL = "Assets/card_task.png"
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
            thisBoard = AssetManager.boards.Single(board => board.Id == _id);

            // We then get the SfKanban which will be how the user interacts with the board's data
            thisKanban = FindViewById<SfKanban>(Resource.Id.kanban);
            thisKanban.ItemTapped += KanbanModelClicked;

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
            thisBoard = AssetManager.boards.Single(board => board.Id == _id);

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
                allSupportedCategories.Add(newDeck.Title);

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
                    Category = card.ParentDeck,
                    Description = card.Description
                });
            }
            thisKanban.ItemsSource = cardList;            
            thisKanban.ItemTapped += KanbanModelClicked;            
        }

        /// 
        /// 
        ///     Takes our kanban values and loads them into the working board for saving
        /// 
        /// 
        public Task LoadKanbanIntoBoard()
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

            return Task.CompletedTask;
        }

        ///
        /// 
        ///     Handles a click event on one of our kanbanmodels inside our Sfkanban
        /// 
        ///         
        private void KanbanModelClicked(object sender, KanbanTappedEventArgs e)
        {
            long tempClickedKanbanId = (long)((KanbanModel)e.Data).ID;

            // If the current click is the first click on the item:
            if (clickIdentifier)
            {
                clickIdentifier = false;
                clickedKanbanModelId = tempClickedKanbanId;
                doubleClickGesture = new DoubleClickGesture(this);                            
            }   
            // If the current click is the second click on the item:
            else if (!clickIdentifier && clickedKanbanModelId == tempClickedKanbanId)
            {
                doubleClickGesture.timer.Stop();
                Toast.MakeText(this, "Fire Double Click Event", ToastLength.Short).Show();
                clickIdentifier = true;
            }
            // A new kanbanModel was clicked therefore we are starting a new DoubleClickGesture for that object
            else if (clickIdentifier && tempClickedKanbanId != clickedKanbanModelId)
            {
                clickIdentifier = false;
                clickedKanbanModelId = tempClickedKanbanId;
                doubleClickGesture = new DoubleClickGesture(this);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            // If the resultCode is equal to Result.Ok then we will manually tell the UI to refresh
            if (requestCode == DETAILS_ACTIVITY_CODE && resultCode == Result.Ok)
            {
                KanbanModel thisKanbanModel = thisKanban.ItemsSource.Cast<KanbanModel>().Single(kanbanModel => kanbanModel.ID == clickedKanbanModelId);

                // We need to assign a new ObservableCollection because we needed the UI to update
                var cardList = new ObservableCollection<KanbanModel>();
                foreach (KanbanModel card in thisKanban.ItemsSource)
                {
                    cardList.Add(card);
                }
                thisKanban.ItemsSource = cardList;                

                Console.WriteLine();
            }
        }

        /// 
        /// 
        ///     Overriding the back button so we automatically save before we exit
        /// 
        /// 
        public override async void OnBackPressed()
        {
            // Loading our kanban data back into the board
            await LoadKanbanIntoBoard();
            // AssetManager.WriteToBoardsOnFile();
            base.OnBackPressed();
        }        
    }
}