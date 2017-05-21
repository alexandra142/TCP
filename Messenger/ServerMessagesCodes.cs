using System.ComponentModel;

namespace Messenger
{
    public enum ServerMessagesCodes
    {
        [Description("100 LOGIN\r\n ")]
        SERVER_USER,

        [Description("101 PASSWORD\r\n ")]
        SERVER_PASSWORD,

        [Description("102 MOVE\r\n ")]
        SERVER_MOVE,

        [Description("103 TURN LEFT\r\n ")]
        SERVER_TURN_LEFT,

        [Description("104 TURN RIGHT\r\n ")]
        SERVER_TURN_RIGHT,

        [Description("105 GET MESSAGE\r\n ")]
        SERVER_PICK_UP,

        [Description("200 OK\r\n ")]
        SERVER_OK,

        [Description("300 LOGIN FAILED\r\n ")]
        SERVER_LOGIN_FAILED,

        [Description("301 SYNTAX ERROR\r\n")]
        SERVER_SYNTAX_ERROR,

        [Description("302 LOGIC ERROR\r\n")]
        SERVER_LOGIC_ERROR
    }
}
