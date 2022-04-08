using System.Threading;

namespace Functions{

    public static class consoleFunctions
    {
        public static void BR(){
            CWR(" ");
        }
        public static void CWR(string str){
            Console.WriteLine(str);
        }
        public static void CLR(){
            Console.Clear();
        }
        public static int RUI(string str){
            // will always return at least 0 becuase the calling code of this will fail if a default is not provided here for failed input values
            int val = 0;
            Console.WriteLine(str);
            string? inp = Console.ReadLine();
            try{
                val = Convert.ToInt32(inp);
            }
            catch(Exception){
                val = 0;
            }
            return val;
        }
        public static void SLEEP(int sleepTimeS){
            int _sleepTimeMS = sleepTimeS * 1000;
            Thread.Sleep(_sleepTimeMS);
        }

        public static string CRI(string str){
            // returns an empty not NULL string in the case of no user input to be caught by the calling function
            Console.WriteLine(str);
            string? inp = Console.ReadLine();
            if(inp != null){
                return inp;
            }
            else{
                return "";
            }
        }

    }
}