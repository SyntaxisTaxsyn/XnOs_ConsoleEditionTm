namespace Players{
        class player{
        private string _name = "";
        private int _score;
        private char _symbol;

        public player(){
            _name = "";
            _score = 0;
            _symbol = ' ';
        }

        public string name{
            get{return _name;}
            set{_name = value;}
        }
        public int score{
            get{return _score;}
            set{_score = value;}
        }

        public char symbol{
            get{return _symbol;}
            set{_symbol = value;}
        }

        public void updateScore(){
            _score++;
        }

        public string getPlayerStatus(){
            return _name + " - score: " + Convert.ToString(_score) + " - playing as " + _symbol;
        }
    }
}
