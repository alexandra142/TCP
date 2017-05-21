using System.ComponentModel;

namespace Messenger
{
    public enum ClientMessageCodes
    {
        [Description("OK")]
        CLIENT_CONFIRM,

        [Description("RECHARGING")]
        CLIENT_RECHARGING,

        [Description("FULL POWER")]
        CLIENT_FULL_POWER,
    }
}
