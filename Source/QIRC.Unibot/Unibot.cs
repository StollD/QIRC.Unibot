/** 
 * .NET Bot for Internet Relay Chat (IRC)
 * Copyright (c) ThomasKerman 2016
 * QIRC is licensed under the MIT License
 */

using ChatSharp;
using QIRC.Configuration;
using QIRC.IRC;
using QIRC.Plugins;
using System;
using System.IO;
using System.Linq;
using PathIO = System.IO.Path;
using ChatSharp.Events;

namespace QIRC.Commands
{
    /// <summary>
    /// Unibot is a bot that enforces non ascii communication on a channel, by kicking people who use basic Latin
    /// </summary>
    public class Unibot : IrcPlugin
    {
        /// <summary>
        /// Random messages returned by the bot
        /// </summary>
        public static String[] messages = new String[]
        {
            "𝐓𝐡𝐢𝐬 𝐜𝐡𝐚𝐧𝐧𝐞𝐥 𝐢𝐬 𝐔𝐧𝐢𝐜𝐨𝐝𝐞 𝐨𝐧𝐥𝐲",
            "𝐍𝐨 𝐛𝐚𝐬𝐢𝐜 𝐋𝐚𝐭𝐢𝐧 𝐡𝐞𝐫𝐞!",
            "𝐋𝐞𝐚𝐫𝐧 𝐲𝐨𝐮𝐫 𝐔𝐧𝐢𝐜𝐨𝐝𝐞",
            "𝐃𝐨𝐰𝐧 𝐰𝐢𝐭𝐡 𝐀𝐦𝐞𝐫𝐢𝐜𝐚𝐧 𝐌𝐚𝐢𝐧𝐬𝐭𝐫𝐞𝐚𝐦"
        };

        /// <summary>
        /// Gets called when a user writes something to a channel and checks for basic latin
        /// </summary>
        public override void OnChannelMessageRecieved(IrcClient client, PrivateMessageEventArgs e)
        {
            // Wrappers
            ProtoIrcMessage message = new ProtoIrcMessage(e);
            IrcUser user = client.Users[message.User];

            // Check for OP status
            IrcChannel channel = client.Channels[message.Source];
            if (user.ChannelModes[channel] != 'o' && user.ChannelModes[channel] != 'O')
                return;

            // Check for basic latin and if it is there, kick
            if (!NoBasicLatin(message.Message))
                client.KickUser(message.Source, message.User, messages[new Random().Next(0, messages.Length)]);
        }

        public Boolean NoBasicLatin(String s)
        {
            for (Int32 i = 0; i < s.Length; i++)
            {
                if (s[i] < 128)
                    return false;
            }
            return true;
        }
    }
}