using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Syncfusion.SfKanban.Android;
using Solitaire.Lang;
using Android.Support.V7.App;
using System.Threading.Tasks;
using Solitaire.CustomGestures;

/*

    This is definatly not the most efficient way of performing the operations needed in this Activity.
    Due to time constraints and lack of indepth documention at https://help.syncfusion.com/xamarin-android/sfkanban/overview

    I await the day I can use my fixed, unsafe, stackalloc keywords!
    
*/

/*

    CURRENT BUG: When the project only contains finished cards an error will occur once you save -> re-open board -> show finished 
        then clicking on a card will result in a exception

*/
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
        public KanbanModel clickedKanbanModel;
        // The for our details activity, readonly because accessing this variable through a pointer to 
        // it within DoubleClickGesture will cause an error
        public readonly int DETAILS_ACTIVITY_CODE = 2;
        // Identifies whether the current click is the first click or the second click in a chain of clicks
        public bool clickIdentifier = true;
        // Pointer needed when using the DoubleClickGesture to have access to the instance 
        DoubleClickGesture thisDoubleClickGestureListener;
        // Contains all the finished kanbanModels
        // List<KanbanModel> finishedKanbanModels;
        // Finished kanbanModels are marked via the collor swatch on the bottem right of their card
        private const string FINISHED_CARD_COLOR = "Red";
        private const string UNFINISHED_CARD_COLOR = "Green";


        /*
         
            Test:

         */
        
        public static List<KanbanModel> kanbanModels = new List<KanbanModel>();


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Applying this SfKanban as the content view for the app
            SetContentView(Resource.Layout.use_board_activity);

            // Setting our custom toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Your Board";
            SetSupportActionBar(toolbar);

            // Calling our initalizer class
            new SetupBoardAndSfkanban(this).InvokeInitEvent();

            // Getting the extra "id" we passed which will enable use to reference our Board
            //long boardId = this.Intent.GetLongExtra("BoardId", -1);

            // We then get the SfKanban which will be how the user interacts with the board's data
            //thisKanban = FindViewById<SfKanban>(Resource.Id.kanban);
            //// If the board needs initalization run:
            //// We dont need to get the data, if that
            //if (this.Intent.HasExtra("NeedInit"))
            //    InitDefaultBoard(boardId);
            //// Otherwise load a pre-existing board:
            //else                            
            //    LoadBoardIntoKanban(boardId);            
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
                /*

                    I wrap our dialogs in a using statement because they the IDisposable interface which will provide deallocation
                    I really don't want a memeory leak because this is already an expensive app to run
                    Amazing Article: https://www.codeproject.com/Articles/29534/IDisposable-What-Your-Mother-Never-Told-You-About

                */
                case "Add Deck":
                    new CreateDeckDialog(this);
                    break;
                case "Add Card":
                    new CreateCardDialog(this);
                        break;
                case "Add Contact":
                    new ContributorOptionsDialog(this);
                    break;
                case "Show Finished Cards":
                    ShowFinishedCards();
                    break;
                case "Hide Finished Cards":
                    HideAllFinishedCards();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        ///
        /// 
        ///     Adds all the finished cards to the UI
        /// 
        /// 
        private void ShowFinishedCards()
        {
            //var cards = new List<KanbanModel>();   
            //cards = thisKanban.ItemsSource.Cast<KanbanModel>().ToList();
            //cards.AddRange(finishedKanbanModels);

            //// Need to clear the finished cards list
            //finishedKanbanModels.Clear();
            thisKanban.ItemsSource = kanbanModels;
        }

        /// 
        /// 
        ///     Removes all finished cards from the UI
        /// 
        ///     THE ERROR IS SOMEWHERE HERE BECAUSE WHEN WE TRY AND HIDE FINSIHED THAT CAUSES THE SEG FAULT
        /// 
        /// 
        private void HideAllFinishedCards()
        {
            //var cards = new List<KanbanModel>();

            //foreach (KanbanModel kanbanModel in thisKanban.ItemsSource)
            //{
            //    if ((string)kanbanModel.ColorKey == UNFINISHED_CARD_COLOR)
            //        cards.Add(kanbanModel);
            //    else
            //        finishedKanbanModels.Add(kanbanModel);
            //}
            // Need to clear the finished cards list            
            thisKanban.ItemsSource = kanbanModels.Where(kanbanModel => (string)kanbanModel.ColorKey == UNFINISHED_CARD_COLOR).ToList();
        }

        /// 
        /// 
        ///     Will add a kanbanModel to a specified column within the current working board
        /// 
        /// 
        public void AddCard(string _nameCard, string _descriptionCard, string _parentDeck)
        {
            kanbanModels.Add(new KanbanModel()
            {
                // Generating Unique Ids
                ID = IdManager.GenerateId(),
                Title = _nameCard,
                Description = _descriptionCard,
                // Category Determines which deck this will be put in
                Category = _parentDeck,
                ColorKey = UNFINISHED_CARD_COLOR
                
                /*

                    TODO: MAKE THIS PATH TO A IMAGE WORK

                */

                // ImageURL = something...
            });
            thisKanban.ItemsSource = kanbanModels.Where(kanbanModel => (string)kanbanModel.ColorKey == UNFINISHED_CARD_COLOR).ToList();
        }

        /// 
        /// 
        ///     Will add a column with the user's provided values
        ///  
        ///
        public void AddDeck(string _nameColumn, string _descriptionColumn)
        {
            allSupportedCategories.Add(_nameColumn);

            KanbanColumn newColumn = new KanbanColumn(this)
            {
                Title = _nameColumn,
                MinimumLimit = 0,
                MaximumLimit = 10,
                // Categories is a list of all the "categories" this deck supports
                Categories = new List<object>() {  _nameColumn }
            };

            // Decorating the new KanbanColumn
            newColumn.ContentDescription = _descriptionColumn;
            newColumn.ErrorBarSettings.Color = Color.Green;
            newColumn.ErrorBarSettings.MinValidationColor = Color.Orange;
            newColumn.ErrorBarSettings.MaxValidationColor = Color.Red;
            newColumn.ErrorBarSettings.Height = 4;

            // We need to add this kanbanWorkflow because it will be used when moving and deciding where cards shall be placed 
            thisKanban.Workflows.Add(new KanbanWorkflow()
            {
                Category = _nameColumn,
                AllowedTransitions = allSupportedCategories
            });   
            
            // Assigning in the new column
            thisKanban.Columns.Add(newColumn);
        }

        /// 
        /// 
        ///     Creates default board & applies it to the UI
        ///
        ///
        //private void InitDefaultBoard(long _id)
        //{
        //    // Assigning the board to our(Boardptr *) basically which will be the board we will be modifing
        //    thisBoard = AssetManager.boards.Single(board => board.Id == _id);

        //    // Intializing the colors needed to indicate whether a card is finished or not
        //    List<KanbanColorMapping> colorModels = new List<KanbanColorMapping>();
        //    colorModels.Add(new KanbanColorMapping(UNFINISHED_CARD_COLOR, Color.Green));
        //    colorModels.Add(new KanbanColorMapping(FINISHED_CARD_COLOR, Color.Red));
        //    thisKanban.IndicatorColorPalette = colorModels;

        //    thisKanban.ItemTapped += KanbanModelClicked;

        //    // Initalizing our Workflows collection
        //    thisKanban.Workflows = new List<KanbanWorkflow>();
        //    // Initalizing our ItemSource collection
        //    kanbanModels = new ObservableCollection<KanbanModel>();
        //    thisKanban.ItemsSource = new ObservableCollection<KanbanModel>();

        //    // We need to initialize our Custom double click gesture before we can use it
        //    thisDoubleClickGestureListener = new DoubleClickGesture();
        //    thisDoubleClickGestureListener.InitDoubleClickGesture(this);
        //}        

        /// 
        /// 
        ///     Loads an pre-existing board & applies it to the UI <>
        /// 
        ///
        //private void LoadBoardIntoKanban(long _id)
        //{
        //    // Assign the board instance we desire to our *pointer
        //    thisBoard = AssetManager.boards.Single(board => board.Id == _id);

        //    // Intializing the colors needed to indicate whether a card is finished or not
        //    List<KanbanColorMapping> colorModels = new List<KanbanColorMapping>();
        //    colorModels.Add(new KanbanColorMapping(UNFINISHED_CARD_COLOR, Color.Green));
        //    colorModels.Add(new KanbanColorMapping(FINISHED_CARD_COLOR, Color.Red));
        //    thisKanban.IndicatorColorPalette = colorModels;


        //    // We need to initialize our Custom double click gesture before we can use it
        //    thisDoubleClickGestureListener = new DoubleClickGesture();
        //    thisDoubleClickGestureListener.InitDoubleClickGesture(this);

        //    /*
             
        //        First we init the Decks and their workflows

        //        Then we init the cards
                
        //    */

        //    // Initializing all KanbanColumns with data from the list of decks in board
        //    foreach (Deck deck in thisBoard.Decks)
        //    {
        //        var newDeck = new KanbanColumn(this)
        //        {
        //            Title = deck.Name,
        //            ContentDescription = deck.Description,
        //            MinimumLimit = 0,
        //            MaximumLimit = 5,
        //            Categories = new List<object>() { deck.Name }
        //        };
        //        newDeck.ErrorBarSettings.Color = Color.Green;
        //        newDeck.ErrorBarSettings.MinValidationColor = Color.Orange;
        //        newDeck.ErrorBarSettings.MaxValidationColor = Color.Red;
        //        newDeck.ErrorBarSettings.Height = 4;
                
        //        thisKanban.Columns.Add(newDeck);
        //        allSupportedCategories.Add(newDeck.Title);

        //        // Initializing the kanban's workflow for each column
        //        thisKanban.Workflows.Add(new KanbanWorkflow()
        //        {
        //            Category = deck.Name,
        //            AllowedTransitions = allSupportedCategories                    
        //        });
        //    }
            
        //    // Initializing all the KanbanModels with data from the list of cards
        //    var unfinishedCards = new ObservableCollection<KanbanModel>();
        //    foreach (Card card in thisBoard.Cards)
        //    {
        //        // We add the finished cards to their own collection
        //        if (card.IsFinished)
        //        {
        //            finishedKanbanModels.Add(new KanbanModel()
        //            {
        //                ID = card.Id,
        //                Title = card.Name,
        //                Category = card.ParentDeck,
        //                Description = card.Description,
        //                ColorKey = FINISHED_CARD_COLOR
        //            });
        //        }    
        //        // The unfinished cards get added to their own collection
        //        else
        //        {
        //            unfinishedCards.Add(new KanbanModel()
        //            {
        //                ID = card.Id,
        //                Title = card.Name,
        //                Category = card.ParentDeck,
        //                Description = card.Description,
        //                ColorKey = UNFINISHED_CARD_COLOR
        //            });
        //        }
        //    }

        //    thisKanban.ItemsSource = unfinishedCards;            
        //    thisKanban.ItemTapped += KanbanModelClicked;            
        //}

        /// 
        /// 
        ///     Takes our kanban values and loads them into the working board for saving
        /// 
        /// 
        public Task SaveKanbanIntoBoard()
        {
            lock (thisBoard)
            {
                // Packing our KanbanColumn info into a list to be added onto the board's deck list
                List<Deck> decks = new List<Deck>();
                foreach (KanbanColumn deck in thisKanban.Columns)
                {
                    decks.Add(new Deck(deck.Title, deck.ContentDescription));
                }
                thisBoard.Decks = decks;
                


                // TODO: Use some fany Linq boys for creating cards from the kanbanModels




                var test = thisKanban.Columns.SelectMany(kanbanColumn => new Deck(kanbanColumn.Title, kanbanColumn.ContentDescription));

                thisBoard.Decks = thisKanban.Columns.SelectMany(kanbanColumn => new Deck(kanbanColumn.Title, kanbanColumn.ContentDescription));

                // var customerOrders2 = customers.SelectMany(c => orders.Where(o => o.CustomerId == c.Id), (c, o) => new { CustomerId = c.Id, OrderDescription = o.Description });



                // Packing our KanbanModel into the a list to be added to the board's cards list
                List<Card> cards = new List<Card>();
                foreach (KanbanModel kanbanModel in thisKanban.ItemsSource)
                {
                    cards.Add(new Card(kanbanModel.Title, kanbanModel.Description, kanbanModel.Category.ToString())
                    {
                        // Determining if this kanban model is finished and then assigning it to the color
                        IsFinished = ((string)kanbanModel.ColorKey) == FINISHED_CARD_COLOR ? true : false
                    });
                }
                // Want to overwrite the old data first
                thisBoard.Cards = cards;

                //if (finishedKanbanModels.Count != 0)
                //{
                //    // Need to add all the finished cards to the board card collection
                //    AddFinshedCardsToBoard();
                //}  
            }

            return Task.CompletedTask;
        }

        ///
        /// 
        ///     Converts finished kanbanModels into cards and adds them the currently selected board card list
        /// 
        ///
        //private void AddFinshedCardsToBoard()
        //{
        //    // Now we can add to it since the old setup was overwritten
        //    foreach (KanbanModel kanbanModel in finishedKanbanModels)
        //    {
        //        thisBoard.Cards.Add(new Card(kanbanModel.Title, kanbanModel.Description, kanbanModel.Category.ToString())
        //        {
        //            // Stating whether this card was finished or not
        //            IsFinished = true
        //        });
        //    }
        //}

        ///
        /// 
        ///     Handles a click event on one of our kanbanmodels inside our Sfkanban
        /// 
        ///         
        private void KanbanModelClicked(object sender, KanbanTappedEventArgs e)
        {
            KanbanModel currentClickedKanbanModel = (KanbanModel)e.Data;

            // If the current click is the first click on the item:
            if (clickIdentifier)
            {
                clickIdentifier = false;
                clickedKanbanModel = currentClickedKanbanModel;
                thisDoubleClickGestureListener.timer.Start();
            }
            // If the current click is the second click on the item:
            else if (!clickIdentifier && clickedKanbanModel.Equals(currentClickedKanbanModel))
            {
                thisDoubleClickGestureListener.timer.Stop();
                clickIdentifier = true;
                // Now we need to make the current card as finished
                MarkCardAsFinished(e.Column ,(long)((KanbanModel)e.Data).ID);
                //Toast.MakeText(this, "Fire Double Click Event", ToastLength.Short).Show();
            }
            // A new kanbanModel was clicked therefore we are starting a new DoubleClickGesture for that object
            else if (!clickIdentifier && !currentClickedKanbanModel.Equals(clickedKanbanModel))
            {
                // First we stop the timer
                thisDoubleClickGestureListener.timer.Stop();
                // Secondly we sent the click identifier to true
                clickIdentifier = true;
                // Thirdly we assign the currently clicked kanbanmodel as the last
                clickedKanbanModel = currentClickedKanbanModel;
                // Then we start an intent on the currently click
                Intent showDetailsActivity = new Intent(this, typeof(DetailsCardActivity));
                showDetailsActivity.PutExtra("kanbanModelId", (long)clickedKanbanModel.ID);
                StartActivityForResult(showDetailsActivity, DETAILS_ACTIVITY_CODE);
            }
        }

        /// 
        /// 
        ///     Hides card from the kanbanWorkflow because the card is "finished"
        /// 
        /// 
        private void MarkCardAsFinished(KanbanColumn _kanbanColumn, long _id)
        {
            // Adding this card to the list of finished cards
            //clickedKanbanModel.ColorKey = FINISHED_CARD_COLOR;
            //finishedKanbanModels.Add(clickedKanbanModel);
            //// Removing it from the column so it won't be displayed.. was hard to find this
            //_column.RemoveItem(clickedKanbanModel);

            //// Removing it from the ItemSource Collection
            //try
            //{
            //    KanbanModel kanbanModelptr = thisKanban.ItemsSource.Cast<KanbanModel>().ToList().Single(kanban => kanban.Equals(clickedKanbanModel));
            //}
            //catch 
            //{
            //    Android.Widget.Toast.MakeText(this, "Failed find card", Android.Widget.ToastLength.Short).Show();
            //}          

            //var tempKanbanModels = thisKanban.ItemsSource.Cast<KanbanModel>().ToList();
            //tempKanbanModels.Remove(clickedKanbanModel);            
            //thisKanban.ItemsSource = tempKanbanModels;


            _kanbanColumn.RemoveItem(clickedKanbanModel);


            foreach (var kanbanModel in kanbanModels)
            {
                if (kanbanModel.ID == _id) kanbanModel.ColorKey = FINISHED_CARD_COLOR;
            }

            thisKanban.ItemsSource = kanbanModels.Where(kanbanModel => (string)kanbanModel.ColorKey == UNFINISHED_CARD_COLOR).ToList();
        }

        /// 
        /// 
        ///     Updates the UI depending on whether the EditCardActivity was launched or not
        /// 
        ///      
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            // If the resultCode is equal to Result.Ok then we will manually tell the UI to refresh
            if (requestCode == DETAILS_ACTIVITY_CODE && resultCode == Result.Ok)
            {
                // Getting the instance within the ItemSource list that matches the edditted 
                // KanbanModel thisKanbanModel = thisKanban.ItemsSource.Cast<KanbanModel>().Single(kanbanModel => kanbanModel.Equals(clickedKanbanModel));

                // We need to assign a new ObservableCollection because we needed the UI to update
                var cardList = new List<KanbanModel>();
                foreach (KanbanModel card in thisKanban.ItemsSource)
                {
                    cardList.Add(card);
                }
                thisKanban.ItemsSource = cardList; 
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
            await SaveKanbanIntoBoard();
            // AssetManager.WriteToBoardsOnFile();
            base.OnBackPressed();
        }

        /// 
        /// 
        ///     Responsible for setting up the board and sfkanban for the UseBoardActivity
        /// 
        ///
        private sealed class SetupBoardAndSfkanban
        {
            // Event that will guide the initialization process... Events are awesome and so is C#
            private event Action setupBoardAndSfKanban;
            private event Action loadPreExistingBoardData;
            private readonly UseBoardActivity useBoardActivity;

            public SetupBoardAndSfkanban(UseBoardActivity _useboardActivity)
            {
                useBoardActivity = _useboardActivity;

                setupBoardAndSfKanban += GetRefToSfKanbanFromResource;
                setupBoardAndSfKanban += GetRefToBoardFromAssetsManager;
                setupBoardAndSfKanban += InitSfKanbanWorkflow;
                setupBoardAndSfKanban += InitSfKanbanColorKey;
                setupBoardAndSfKanban += InitSfKanbanGestures;
                // Initializing our kanbanModels to new collection each time we run this activity
                setupBoardAndSfKanban += delegate { UseBoardActivity.kanbanModels = new List<KanbanModel>(); };
                // If we have board data to load... load it, otherwise skip
                setupBoardAndSfKanban += delegate { loadPreExistingBoardData?.Invoke(); };
            }

            /// 
            /// 
            ///     Allows for safe public invoking for setupBoardAndSfKanban
            /// 
            /// 
            public void InvokeInitEvent()
            {
                setupBoardAndSfKanban?.Invoke();
            }

            /// 
            /// 
            ///     Getting a ref to the SfKanban from the inflated layout from XML, therefore getting it from the Resource.Id class
            /// 
            /// 
            private void GetRefToSfKanbanFromResource()
            {
                UseBoardActivity.thisKanban = useBoardActivity.FindViewById<SfKanban>(Resource.Id.sfKanban);
            }

            ///
            /// 
            ///     Getting a ref to the Board that was created or was clicked on that started the UseBoardActivity
            /// 
            /// 
            private void GetRefToBoardFromAssetsManager()
            {
                // If our board has extra data needing to be loaded, attach a method to event for that
                if (!useBoardActivity.Intent.HasExtra("IsNew")) loadPreExistingBoardData += LoadDataFromBoardIntoSfKanban;

                // Getting a ref to the board we "working" with using an Intent.Extra
                useBoardActivity.thisBoard = AssetManager.boards.Single(board => board.Id == useBoardActivity.Intent.GetLongExtra("BoardId", -1));
            }

            /// 
            /// 
            ///     Initializes the workflow for the SfKanban
            /// 
            /// 
            private void InitSfKanbanWorkflow()
            {
                UseBoardActivity.thisKanban.Workflows = new List<KanbanWorkflow>();
            }

            /// 
            ///
            ///     Initializes the color key used for detecting and displaying whether a kanbanModel is "finished" 
            /// 
            /// 
            private void InitSfKanbanColorKey()
            {
                UseBoardActivity.thisKanban.IndicatorColorPalette = new List<KanbanColorMapping>
                {
                    new KanbanColorMapping(UNFINISHED_CARD_COLOR, Color.Green),
                    new KanbanColorMapping(FINISHED_CARD_COLOR, Color.Red)
                };
            }

            /// 
            /// 
            ///     Initializing the single click and double click gestures
            /// 
            /// 
            private void InitSfKanbanGestures()
            {
                // Single click
                UseBoardActivity.thisKanban.ItemTapped += useBoardActivity.KanbanModelClicked;

                // Double click
                useBoardActivity.thisDoubleClickGestureListener = new DoubleClickGesture();                
                useBoardActivity.thisDoubleClickGestureListener.InitDoubleClickGesture(useBoardActivity);
            }

            /// 
            /// 
            ///     Loads data from board ref into our Sfkanban (optional method), look GetRefToBoard()
            /// 
            ///     Steps:
            ///         1. Init KanbanColumn
            ///             A. Add this KanbanColumn instance's Category property to the allSupportedCategories list
            ///             B. Add new workflow using this KanbanColumn instance's Category and the allSupportedCategories list
            ///             C. Add this KanbanColumn instance to the SfKanban.Columns list
            ///             
            ///         2. Init KanbanModel
            ///             A. Determine whether the card is finished or unfinished when creating the kanbanModel
            ///             B. Add this kanbanModel to the main KanbanModel list
            ///             
            ///         3. Assign the fully setup list of KanbanModels to the SfKanban.ItemsSource
            ///
            private void LoadDataFromBoardIntoSfKanban()
            {

                // Initializing KanbanColumns
                foreach (Deck KanbanColumn in useBoardActivity.thisBoard.Decks)
                {
                    var newColumn = new KanbanColumn(useBoardActivity)
                    {
                        Title = KanbanColumn.Name,
                        ContentDescription = KanbanColumn.Description,
                        MinimumLimit = 0,
                        MaximumLimit = 5,
                        Categories = new List<object>() { KanbanColumn.Name }
                    };
                    newColumn.ErrorBarSettings.Color = Color.Green;
                    newColumn.ErrorBarSettings.MinValidationColor = Color.Orange;
                    newColumn.ErrorBarSettings.MaxValidationColor = Color.Red;
                    newColumn.ErrorBarSettings.Height = 4;

                    UseBoardActivity.thisKanban.Columns.Add(newColumn);
                    useBoardActivity.allSupportedCategories.Add(newColumn.Title);

                    // Initializing the workflows
                    UseBoardActivity.thisKanban.Workflows.Add(new KanbanWorkflow()
                    {
                        Category = KanbanColumn.Name,
                        AllowedTransitions = useBoardActivity.allSupportedCategories
                    });
                }


                // Initializing KanbanModels
                // UseBoardActivity.kanbanModels = new List<KanbanModel>();
                foreach (Card card in useBoardActivity.thisBoard.Cards)
                {
                    UseBoardActivity.kanbanModels.Add(new KanbanModel()
                    {
                        ID = card.Id,
                        Title = card.Name,
                        Category = card.ParentDeck,
                        Description = card.Description,
                        ColorKey = card.IsFinished ? FINISHED_CARD_COLOR : UNFINISHED_CARD_COLOR
                    });
                }

                // Assigning the kanbanModels to the ItemSource
                UseBoardActivity.thisKanban.ItemsSource = kanbanModels;
            }
        }
    }    
}