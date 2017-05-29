using System.ComponentModel;

namespace Messenger
{
    public enum ServerMessagesCodes
    {
        [Description("100 LOGIN")]
        SERVER_USER,

        [Description("101 PASSWORD")]
        SERVER_PASSWORD,

        [Description("102 MOVE")]
        SERVER_MOVE,

        [Description("103 TURN LEFT")]
        SERVER_TURN_LEFT,

        [Description("104 TURN RIGHT")]
        SERVER_TURN_RIGHT,

        [Description("105 GET MESSAGE")]
        SERVER_PICK_UP,

        [Description("200 OK")]
        SERVER_OK,

        [Description("300 LOGIN FAILED")]
        SERVER_LOGIN_FAILED,

        [Description("301 SYNTAX ERROR")]
        SERVER_SYNTAX_ERROR,

        [Description("302 LOGIC ERROR")]
        SERVER_LOGIC_ERROR
    }
}
