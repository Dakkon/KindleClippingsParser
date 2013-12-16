using KindleClippingsParser.Model;

namespace KindleClippingsParser.Controller
{
    public class KindleClippingsParserController
    {
        #region Ctors

        public KindleClippingsParserController()
        {
            this.test_CreateClippingsFileParserInstance();
        }

        #endregion Ctors

        #region FOR TESTING PURPOSES

        private void test_CreateClippingsFileParserInstance()
        {
            ClippingsFileParser testView = new ClippingsFileParser("C:\\My Clippings.txt");
        }       

        #endregion FOR TESTING PURPOSES
    }
}
