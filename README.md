# Solitaire

Solitaire is an early-stage management app where you can organize your project. The app’s objective is to allow the user to create projects and structure them effectively using the app. The personal objective is for this to be a great learning experience.

  - A Board is like the project, it contains all the contents that make up the project. It's the main wrapper for everything that has to do with that project. 
  - A Deck is a container inside the board that will hold all the user’s objectives. 
  - A Card is a task or requirement that needs to be completed.
  - A Contributor is an individual who can be added to cards to contribute towards its completion.

## Main Features 

  - Add collapsible decks to a board
  - Add cards to decks
  - Add contributors to any cards
  - Cards can be moved to different decks 
  - Cards can be marked as finished
  - Remove or Add contributors to cards
  - Mark cards as finished and show or hide them from view

### Platform(s)

  - Android

### Plugins
Solitaire is currently extended with the following plugins. Intructions on how to implement them into your own projects are linked below.

| Plugin | Link |
| ------ | ------ |
| Syncfusion | https://help.syncfusion.com/xamarin-android/introduction/overview |
| Newtonsoft | https://www.newtonsoft.com/json |
| CircleImageView | https://github.com/jamesmontemagno/CircleImageView-Xamarin.Android
### Upcoming Updates

- [x] Implement being able to view board’s total decks within DetailsBoardActivity.
- [x] Implement being able to view board’s total cards within DetailsBoardActivity.
- [x] Change create contributor dialog UI colors to match other dialog UI colors.
- [ ] Implement UI offset when keyboard is toggled.
- [ ] Find a way to keep reference to leader of cards for EditCardActivity.
- [ ] Either remove or add functionality for “Min / Max” card limit for decks.
- [ ] Implement ability to delete boards
- [ ] Implement ability to delete decks
- [ ] Implement ability to delete cards
- [ ] Implement ability to delete contributors
- [ ] Improve the spinner UI for adding card to specific deck.
- [ ] Fix image issue with KanbanModel.Url not displaying desired images.
- [ ] Implement the ability for the user to set a custom image for boards and cards.
- [ ] Finish setting up client and server socket connection.
- [x] Improve the UI for contributor rows.
- [ ] Improve UI for ALL edit based Activities.
- [ ] Fix SHA1 status code 10 error with google login.
- [ ] Allow the user to view and edit the contributors.
- [ ] Implement demographics for boards.

### Powered By
[![Foo](https://upload.wikimedia.org/wikipedia/commons/f/f2/Xamarin-logo.svg)](https://dotnet.microsoft.com/apps/xamarin/)
