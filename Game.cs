using System;
using Players;
using static Functions.consoleFunctions;

namespace Game{

    interface gameFunctions{
        
        void displayTitle(); // shows the title
        void showGUI(); // display player scores
        void showGameBoard();
        void newGame(); // restart the game
        void clearGameBoard(); // reset the game board, normally called by new game
        void switchPlayerTurns(); // change which symbol each player is using
        bool checkEmpty(int r, int c); // check a particular space is empty
        void updateGrid(char s,int r, int c); // insert the character s at grid location r,c
        bool checkWin(char s); // test if the supplied character s has a win condition on the game board
        bool checkCatsGame(); // test if the game cannot be completed
        void updateScores(int player); // Updates the score for the specified player (1 or 2)
        void decideNextPlayer(); // Used to decide who is starting the next game
        void startGame(); // contains the code for the main game loop
        bool checkNewGame(); // check if the players wish to play another game
        void checkWinner(); // check which player has won the game overall at the end of the game and display win message or draw
        void selectNextPlayer(); // change next player at end of current turn

    }

    class XnOs : gameFunctions
    {   
        string[,] gameboard = new string[3,3];
        player[] players = new player[2];
        private string colHeader = "  Columns ";
        private string colNumbers = " 0   1   2 ";
        private string emptyRow = "   |   |   ";
        private string gridline = "-----------";
        private bool playerInfoCollected = false;
        private int currentPlayer;

        public XnOs(){
            players[0] = new player();
            players[1] = new player();
        }

        private int protectRCLimits(int x){
            // this is to ensure the entered number is within array dimension limits
            if(x>2){x=2;}
            if(x<0){x=0;}
            return x;
        }

        public void startGame(){
            // this is main game loop
            // execution gets into here by calling newGame() which will check player registrations

            bool applicationFinished = false;
            bool gameFinished = false;
            bool _continue = false;
            int r;
            int c;
            string _errmsg = "";

            // obligatory first new game call to get started
            newGame();

            while(!applicationFinished){

                while(!gameFinished){

                    // logic for single turn condsidering the current player, valid entry must be recieved to advance the game
                    while(!_continue){
                        displayTitle();
                        showGUI();
                        showGameBoard();
                        if(_errmsg != ""){CWR(_errmsg);}
                        CWR("It is " + players[currentPlayer].name + "'s turn - you are " + players[currentPlayer].symbol + "'s");
                        r = RUI("Enter a row number");
                        r = protectRCLimits(r);
                        c = RUI("Enter a col number");
                        c = protectRCLimits(c);
                        if(checkEmpty(r,c)){
                            updateGrid(players[currentPlayer].symbol,r,c);
                            _continue = true;
                        } else {
                            _errmsg = "Selection not empty, please choose another";
                        }
                    }

                    _errmsg = "";

                    // 1 turn is complete, perform check logic for game win etc
                    if (checkWin(players[currentPlayer].symbol)){
                        updateScores(currentPlayer);
                        displayTitle();
                        showGUI();
                        showGameBoard();
                        CWR(players[currentPlayer].name + " is the winnner, well done!");
                        SLEEP(2);
                        gameFinished = true;
                    }

                    if(checkCatsGame()){
                        if(!gameFinished){
                            displayTitle();
                            showGUI();
                            showGameBoard();
                            CWR("Game over, no one wins!");
                        SLEEP(2);
                        gameFinished = true;
                        }
                    }

                    _continue = false;
                    selectNextPlayer();
                    
                }

                // game finished, check new game wanted or not
                if(!checkNewGame()){
                    applicationFinished = true;
                } else {
                    // start a new game
                    newGame();
                    gameFinished = false;
                }

            }

            // application is finished
            checkWinner();
            //Environment.Exit(0);

        }

        public void selectNextPlayer(){
            if(currentPlayer==0){
                currentPlayer = 1;
            } else {
                currentPlayer = 0;
            }
        }

        public bool checkCatsGame(){
            // this function gets called after check win
            // basically if you get in here and all grid points are not = " "
            // then the game is over
            // more advanced scenarios may be entered in here later such as 7 or 8 filled but no possible wins exist
            bool isCatsGame = true;

            // checking for any positions with empty chars present
            if (gameboard[0,0]==" "){isCatsGame = false;}
            if (gameboard[0,1]==" "){isCatsGame = false;}
            if (gameboard[0,2]==" "){isCatsGame = false;}

            if (gameboard[1,0]==" "){isCatsGame = false;}
            if (gameboard[1,1]==" "){isCatsGame = false;}
            if (gameboard[1,2]==" "){isCatsGame = false;}

            if (gameboard[2,0]==" "){isCatsGame = false;}
            if (gameboard[2,1]==" "){isCatsGame = false;}
            if (gameboard[2,2]==" "){isCatsGame = false;}

            return isCatsGame;
        }

        public bool checkNewGame(){
            bool decision = false;
            string answer = CRI("Would you like to play another game (y/n)");
            if(answer=="y"){
                decision = true;
            } else {
                decision = false;
            }
            return decision;
        }

        public void checkWinner(){

            // this is the end of the application 

            int s1 = players[0].score;
            int s2 = players[1].score;
            string n1 = players[0].name;
            string n2 = players[1].name;

            displayTitle();
            showGUI();
            CWR("Game over - thank you for playing");
            BR();

            if(s1>s2){CWR(n1 + " is the winner well done");}
            if(s2>s1){CWR(n2 + " is the winner well done");}
            if(s1==s2){CWR("The game was a draw, better luck next time");}

            CWR("Press ENTER key to exit application");
            CRI("");

        }

        public void updateScores(int player){
            players[player].updateScore();
        }

        public bool checkWin(char s){

            bool result = false;
            string c = Convert.ToString(s);
            string[,] g = gameboard; // get gameboard context with shortened variable name ;)

            // rows
            if( g[0,0] == c && g[0,1] == c && g[0,2] == c ){ result = true; }
            if( g[1,0] == c && g[1,1] == c && g[1,2] == c ){ result = true; }
            if( g[2,0] == c && g[2,1] == c && g[2,2] == c ){ result = true; }
            // columns
            if( g[0,0] == c && g[1,0] == c && g[2,0] == c ){ result = true; }
            if( g[0,1] == c && g[1,1] == c && g[2,1] == c ){ result = true; }
            if( g[0,2] == c && g[1,2] == c && g[2,2] == c ){ result = true; }
            // diagonals
            if( g[0,0] == c && g[1,1] == c && g[2,2] == c ){ result = true; }
            if( g[0,2] == c && g[1,1] == c && g[2,0] == c ){ result = true; }

            return result;

        }

        public void updateGrid(char s, int r, int c){
            gameboard[r,c] = Convert.ToString(s);
        }

        public void displayTitle(){
            CLR();
            CWR("-----------------------------------------------------------------");
            CWR("----------------X and O's Console Edition (tm)-------------------");
            CWR("-----------------------------------------------------------------");
            BR();
        }
        public void showGUI(){
            CWR(players[0].getPlayerStatus());
            CWR(players[1].getPlayerStatus());
        }
        public void showGameBoard(){

            string[] rows = new string[3]{
                getGameBoardLineByRow(0),
                getGameBoardLineByRow(1),
                getGameBoardLineByRow(2),
            };
            BR();

            // header
            CWR(colHeader);
            CWR(colNumbers);

            // row 1
            CWR(emptyRow);
            CWR(rows[0] + " 0  - Rows ");
            CWR(emptyRow);
            CWR(gridline);

            // row 2
            CWR(emptyRow);
            CWR(rows[1] + " 1");
            CWR(emptyRow);
            CWR(gridline);

            // row 3
            CWR(emptyRow);
            CWR(rows[2] + " 2");
            CWR(emptyRow);

            BR();
        }

        private string getGameBoardLineByRow(int r){
            string str = "";
            str = " " + gameboard[r,0] + " | " + gameboard[r,1] + " | " + gameboard[r,2] + " ";
            return str;
        }

        public void newGame(){
            bool nextPlayerDecided = false;
            if(!playerInfoCollected){
                getPlayerInfo();
                playerInfoCollected = true;
                currentPlayer = 0;
                nextPlayerDecided = true;
            }
            clearGameBoard();
            switchPlayerTurns();
            if(!nextPlayerDecided){decideNextPlayer();}
            //startGame();
        }

        public void decideNextPlayer(){
            if(currentPlayer == 0){
                currentPlayer = 1;
            } else {
                currentPlayer = 0;
            }
        }

        public bool checkEmpty(int r, int c){
            bool isEmpty = false;
            if(gameboard[r,c] == " "){ isEmpty=true; }
            return isEmpty;
        }

        public void switchPlayerTurns(){
            if (players[0].symbol == 'X'){
                players[0].symbol = 'O';
                players[1].symbol = 'X';
            } else {
                players[0].symbol = 'X';
                players[1].symbol = 'O';
            }
        }

        public void clearGameBoard(){
            for(int r = 0;r<3;r++){
                for(int c = 0;c<3;c++){
                    gameboard[r,c] = " ";
                }
            }
        }

        private void getPlayerInfo(){
            bool _continue = false;

            // get player 1 name, no errors accepted, username must be a valid string with at least 1 character
            displayTitle();
            CWR("Welcome to the game, to start lets gather players names");
            while(!_continue){
                players[0].name = CRI("Player 1, what is your name?");
                if(players[0].name != ""){
                    players[0].symbol = 'X';
                    _continue = true;
                } else {
                    displayTitle();
                    CWR("Welcome to the game, to start lets gather players names");
                    CWR("Error input detected, please enter a valid username!");
                }
            }

            // get player 2 name, no errors accepted, username must be a valid string with at least 1 character
            _continue = false;
            displayTitle();
            CWR(players[0].name + " is player 1, they will start with the X piece");
            while(!_continue){
                players[1].name = CRI("Player 2, what is your name?");
                if(players[1].name != ""){
                    players[1].symbol = 'O';
                    _continue = true;
                } else {
                    displayTitle();
                    CWR(players[0].name + " is player 1, they will start with the X piece");
                    CWR("Error input detected, please enter a valid username!");
                }
            }

            displayTitle();
            CWR(players[0].name + " is player 1, they will start with the X piece");
            CWR(players[1].name + " is player 2, they will start with the O piece");
            SLEEP(3);
            CWR("Excellent, now we have both players, lets get on with the game");
            RUI("Press ENTER to continue...");

        }
        
    }
    
}